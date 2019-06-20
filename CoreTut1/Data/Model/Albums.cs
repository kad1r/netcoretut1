using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public partial class Albums
    {
        public Albums()
        {
            AlbumGalleries = new HashSet<AlbumGalleries>();
            Photos = new HashSet<Photos>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Heading { get; set; }
        public string Description { get; set; }
        [StringLength(500)]
        public string Tags { get; set; }
        [StringLength(500)]
        public string CoverPic { get; set; }
        public int Sequence { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateInserted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateUpdated { get; set; }
        public int IsActive { get; set; }

        [InverseProperty("Album")]
        public virtual ICollection<AlbumGalleries> AlbumGalleries { get; set; }
        [InverseProperty("Album")]
        public virtual ICollection<Photos> Photos { get; set; }
    }
}