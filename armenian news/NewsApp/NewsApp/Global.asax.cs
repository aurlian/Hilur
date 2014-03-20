using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Configuration;
using System.Web.Configuration;
using System.IO;

namespace NewsApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //load new content if not done since one day
            DateTime lastLoad = DateTime.Parse(ConfigurationManager.AppSettings["LastLoad"]);

            if ((DateTime.Now - lastLoad).Days >= 1)
            {
                SaveEntries(GetEntries());

                var config = WebConfigurationManager.OpenWebConfiguration("~");
                config.AppSettings.Settings["LastLoad"].Value = DateTime.Now.ToShortDateString();
                config.Save(ConfigurationSaveMode.Minimal, false);
            }

            
        }

        private List<RssFeedItem> GetEntries()
        {
            //create a new list of the rss feed items to return
            List<RssFeedItem> rssFeedItems = new List<RssFeedItem>();

            //create an http request which will be used to retrieve the rss feed
            HttpWebRequest rssFeed = (HttpWebRequest)WebRequest.Create("http://xecutrix.usc.edu/news/index.rss");

            //use a dataset to retrieve the rss feed
            using (DataSet rssData = new DataSet())
            {
                //read the xml from the stream of the web request
                rssData.ReadXml(rssFeed.GetResponse().GetResponseStream());

                //loop through the rss items in the dataset and populate the list of rss feed items
                foreach (DataRow dataRow in rssData.Tables["item"].Rows)
                {
                    string dateString = Convert.ToString(dataRow["pubDate"]).Replace(" (PDT)", "").Replace(" (PST)", "").Replace(",", "");
                    DateTime parsedDate = DateTime.ParseExact(dateString, "ddd d MMM yyyy HH:mm:ss zzz", null);
                    string link = Convert.ToString(dataRow["link"]);
                    string title = Convert.ToString(dataRow["title"]);
                    //fetch content
                    string post = FetchPost(link);
                    if (!string.IsNullOrEmpty(post))
                    {
                        rssFeedItems.Add(new RssFeedItem
                        {
                            Link = link,
                            PublishDate = parsedDate,
                            Title = title,
                            Post = post
                        });
                    }
                }
            }

            return rssFeedItems;
        }

        private string FetchPost(string link)
        {
            string html = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(link);
                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(data))
                {
                    html = sr.ReadToEnd();
                }
                int startIndex = html.IndexOf("<pre>") + "<pre>".Length;
                int endIndex = html.IndexOf("</pre>");

                html = html.Substring(startIndex, endIndex - startIndex);
            }
            catch (Exception e)
            {
                //log
            }
            return html;
        }

        public void SaveEntries(List<RssFeedItem> list)
        {
            string connection = ConfigurationManager.ConnectionStrings["Hilur"].ToString();
            using (SqlConnection sqlConn = new SqlConnection(connection))
            {
                sqlConn.Open();
                foreach (var entry in list)
                {
                    using (SqlCommand sqlCmd = new SqlCommand())
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandText = "Insert_Entry";
                        sqlCmd.Connection = sqlConn;
                        sqlCmd.Parameters.Add(new SqlParameter("@title", entry.Title));
                        sqlCmd.Parameters.Add(new SqlParameter("@pubDate", entry.PublishDate));
                        sqlCmd.Parameters.Add(new SqlParameter("@link", entry.Link));
                        sqlCmd.Parameters.Add(new SqlParameter("@post", entry.Post));
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                sqlConn.Close();
            }
        }

        /// <summary>
        /// RSS feed item entity
        /// </summary>
        public class RssFeedItem
        {
            /// <summary>
            /// Gets or sets the title
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// Gets or sets the link
            /// </summary>
            public string Link { get; set; }

            /// <summary>
            /// Gets or sets the post
            /// </summary>
            public string Post { get; set; }

            /// <summary>
            /// Gets or sets the publish date
            /// </summary>
            public DateTime PublishDate { get; set; }

        }
    }
}