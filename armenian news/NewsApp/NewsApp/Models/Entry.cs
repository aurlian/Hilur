//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewsApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Entry
    {
        public Entry()
        {
            this.Comments = new HashSet<Comment>();
            this.Votes = new HashSet<Vote>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public System.DateTime PubDate { get; set; }
        public string Link { get; set; }
        public string Post { get; set; }
        public System.DateTime FetchDate { get; set; }
        public int Score { get; set; }
    
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}