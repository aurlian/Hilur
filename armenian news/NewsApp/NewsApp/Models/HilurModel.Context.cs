﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HilurEntities : DbContext
    {
        public HilurEntities()
            : base("name=HilurEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<SiteUser> SiteUsers { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
