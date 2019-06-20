using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public partial class AboutMe
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Heading { get; set; }
        [Required]
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateInserted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateUpdated { get; set; }
        public int IsActive { get; set; }
    }
}