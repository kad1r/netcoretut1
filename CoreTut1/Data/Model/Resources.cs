using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public partial class Resources
    {
        public int Id { get; set; }
        [Required]
        [StringLength(5)]
        public string Culture { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
    }
}