//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TTWinAPp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblCatFact
    {
        public int Id { get; set; }
        public decimal TwitID { get; set; }
        public string text { get; set; }
        public Nullable<decimal> userid { get; set; }
        public string name { get; set; }
        public string screenname { get; set; }
        public string location { get; set; }
        public bool IsVerified { get; set; }
        public string ProfileUrl { get; set; }
        public string MediaUrl { get; set; }
        public string Url { get; set; }
        public string Display_Url { get; set; }
        public string expanded_url { get; set; }
        public string MediaType { get; set; }
        public decimal RT_Count { get; set; }
        public decimal Fav_Count { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
    }
}
