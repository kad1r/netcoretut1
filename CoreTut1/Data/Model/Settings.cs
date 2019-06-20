using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public partial class Settings
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string LogoUrl { get; set; }
        [StringLength(500)]
        public string SiteName { get; set; }
        [StringLength(500)]
        public string SiteUrl { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(500)]
        public string Keywords { get; set; }
        [StringLength(250)]
        public string FotoFes { get; set; }
        [StringLength(250)]
        public string Facebook { get; set; }
        [StringLength(250)]
        public string Twitter { get; set; }
        [StringLength(250)]
        public string Instagram { get; set; }
        [StringLength(250)]
        public string Pinterest { get; set; }
        [StringLength(250)]
        public string GooglePlus { get; set; }
        [StringLength(250)]
        public string Behance { get; set; }
        [StringLength(250)]
        public string Flickr { get; set; }
        [StringLength(250)]
        public string FiveHundredPx { get; set; }
        [StringLength(250)]
        public string Talentell { get; set; }
        [StringLength(250)]
        public string SmtpAddress { get; set; }
        [StringLength(250)]
        public string SmtpMail { get; set; }
        [StringLength(250)]
        public string SmtpPassword { get; set; }
        [StringLength(250)]
        public string SmtpPort { get; set; }
        public string TrackCodes { get; set; }
        [StringLength(500)]
        public string HeadMetaForHomePage { get; set; }
        public bool? ShowGalleriesOnHomePage { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastPingDate { get; set; }
    }
}