﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TTServices.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class TwitRapDBEntities : DbContext
    {
        public TwitRapDBEntities()
            : base("name=TwitRapDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<tblCatAuto> tblCatAutoes { get; set; }
        public DbSet<tblCatBusiness> tblCatBusinesses { get; set; }
        public DbSet<tblCatFact> tblCatFacts { get; set; }
        public DbSet<tblCatGadget> tblCatGadgets { get; set; }
        public DbSet<tblCatHF> tblCatHFs { get; set; }
        public DbSet<tblCatJoke> tblCatJokes { get; set; }
        public DbSet<tblCatMovy> tblCatMovies { get; set; }
        public DbSet<tblCatMusic> tblCatMusics { get; set; }
        public DbSet<tblCatNew> tblCatNews { get; set; }
        public DbSet<tblCatSport> tblCatSports { get; set; }
        public DbSet<tblCatTravel> tblCatTravels { get; set; }
        public DbSet<tblDemoBeacon> tblDemoBeacons { get; set; }
        public DbSet<tblCatLove> tblCatLoves { get; set; }
        public DbSet<tblCatTech> tblCatTeches { get; set; }
        public DbSet<tblCatScience> tblCatSciences { get; set; }
        public DbSet<tblCatLove1> tblCatLove1 { get; set; }
    
        public virtual ObjectResult<USP_FetchCATLoveTweets_Result> USP_FetchCATLoveTweets(Nullable<int> fType, Nullable<int> fromId)
        {
            var fTypeParameter = fType.HasValue ?
                new ObjectParameter("fType", fType) :
                new ObjectParameter("fType", typeof(int));
    
            var fromIdParameter = fromId.HasValue ?
                new ObjectParameter("fromId", fromId) :
                new ObjectParameter("fromId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_FetchCATLoveTweets_Result>("USP_FetchCATLoveTweets", fTypeParameter, fromIdParameter);
        }
    
        public virtual ObjectResult<USP_FetchCATAutoTweets_Result> USP_FetchCATAutoTweets(Nullable<int> fType, Nullable<int> fromId)
        {
            var fTypeParameter = fType.HasValue ?
                new ObjectParameter("fType", fType) :
                new ObjectParameter("fType", typeof(int));
    
            var fromIdParameter = fromId.HasValue ?
                new ObjectParameter("fromId", fromId) :
                new ObjectParameter("fromId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_FetchCATAutoTweets_Result>("USP_FetchCATAutoTweets", fTypeParameter, fromIdParameter);
        }
    
        public virtual ObjectResult<USP_FetchCATFactsTweets_Result> USP_FetchCATFactsTweets(Nullable<int> fType, Nullable<int> fromId)
        {
            var fTypeParameter = fType.HasValue ?
                new ObjectParameter("fType", fType) :
                new ObjectParameter("fType", typeof(int));
    
            var fromIdParameter = fromId.HasValue ?
                new ObjectParameter("fromId", fromId) :
                new ObjectParameter("fromId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_FetchCATFactsTweets_Result>("USP_FetchCATFactsTweets", fTypeParameter, fromIdParameter);
        }
    
        public virtual ObjectResult<USP_FetchCATJokesTweets_Result> USP_FetchCATJokesTweets(Nullable<int> fType, Nullable<int> fromId)
        {
            var fTypeParameter = fType.HasValue ?
                new ObjectParameter("fType", fType) :
                new ObjectParameter("fType", typeof(int));
    
            var fromIdParameter = fromId.HasValue ?
                new ObjectParameter("fromId", fromId) :
                new ObjectParameter("fromId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_FetchCATJokesTweets_Result>("USP_FetchCATJokesTweets", fTypeParameter, fromIdParameter);
        }
    
        public virtual ObjectResult<USP_FetchCATNewsTweets_Result> USP_FetchCATNewsTweets(Nullable<int> fType, Nullable<int> fromId)
        {
            var fTypeParameter = fType.HasValue ?
                new ObjectParameter("fType", fType) :
                new ObjectParameter("fType", typeof(int));
    
            var fromIdParameter = fromId.HasValue ?
                new ObjectParameter("fromId", fromId) :
                new ObjectParameter("fromId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_FetchCATNewsTweets_Result>("USP_FetchCATNewsTweets", fTypeParameter, fromIdParameter);
        }
    
        public virtual ObjectResult<USP_FetchCATSportsTweets_Result> USP_FetchCATSportsTweets(Nullable<int> fType, Nullable<int> fromId)
        {
            var fTypeParameter = fType.HasValue ?
                new ObjectParameter("fType", fType) :
                new ObjectParameter("fType", typeof(int));
    
            var fromIdParameter = fromId.HasValue ?
                new ObjectParameter("fromId", fromId) :
                new ObjectParameter("fromId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_FetchCATSportsTweets_Result>("USP_FetchCATSportsTweets", fTypeParameter, fromIdParameter);
        }
    
        public virtual ObjectResult<USP_FetchCATTravelTweets_Result> USP_FetchCATTravelTweets(Nullable<int> fType, Nullable<int> fromId)
        {
            var fTypeParameter = fType.HasValue ?
                new ObjectParameter("fType", fType) :
                new ObjectParameter("fType", typeof(int));
    
            var fromIdParameter = fromId.HasValue ?
                new ObjectParameter("fromId", fromId) :
                new ObjectParameter("fromId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_FetchCATTravelTweets_Result>("USP_FetchCATTravelTweets", fTypeParameter, fromIdParameter);
        }
    
        public virtual ObjectResult<USP_FetchCATBusinessTweets_Result> USP_FetchCATBusinessTweets(Nullable<int> fType, Nullable<int> fromId)
        {
            var fTypeParameter = fType.HasValue ?
                new ObjectParameter("fType", fType) :
                new ObjectParameter("fType", typeof(int));
    
            var fromIdParameter = fromId.HasValue ?
                new ObjectParameter("fromId", fromId) :
                new ObjectParameter("fromId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_FetchCATBusinessTweets_Result>("USP_FetchCATBusinessTweets", fTypeParameter, fromIdParameter);
        }
    
        public virtual ObjectResult<USP_FetchCATScienceTweets_Result> USP_FetchCATScienceTweets(Nullable<int> fType, Nullable<int> fromId)
        {
            var fTypeParameter = fType.HasValue ?
                new ObjectParameter("fType", fType) :
                new ObjectParameter("fType", typeof(int));
    
            var fromIdParameter = fromId.HasValue ?
                new ObjectParameter("fromId", fromId) :
                new ObjectParameter("fromId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_FetchCATScienceTweets_Result>("USP_FetchCATScienceTweets", fTypeParameter, fromIdParameter);
        }
    
        public virtual ObjectResult<USP_FetchCATTechTweets_Result> USP_FetchCATTechTweets(Nullable<int> fType, Nullable<int> fromId)
        {
            var fTypeParameter = fType.HasValue ?
                new ObjectParameter("fType", fType) :
                new ObjectParameter("fType", typeof(int));
    
            var fromIdParameter = fromId.HasValue ?
                new ObjectParameter("fromId", fromId) :
                new ObjectParameter("fromId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_FetchCATTechTweets_Result>("USP_FetchCATTechTweets", fTypeParameter, fromIdParameter);
        }
    }
}
