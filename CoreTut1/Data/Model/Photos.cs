using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public partial class Photos
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int? AlbumId { get; set; }
        [Required]
        [StringLength(500)]
        public string Heading { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [StringLength(500)]
        public string PhotoUrl { get; set; }
        [Required]
        [StringLength(500)]
        public string ThumbnailUrl { get; set; }
        public int Sequence { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateInserted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateUpdated { get; set; }
        public int IsActive { get; set; }

        [ForeignKey("AlbumId")]
        [InverseProperty("Photos")]
        public virtual Albums Album { get; set; }
    }
}