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
    
    public partial class Vote
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public bool Score { get; set; }
        public System.DateTime Date { get; set; }
        public int UserId { get; set; }
    
        public virtual Entry Entry { get; set; }
        public virtual SiteUser SiteUser { get; set; }
    }
}