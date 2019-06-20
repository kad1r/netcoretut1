using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public partial class AlbumGalleries
    {
        public int GalleryId { get; set; }
        public int AlbumId { get; set; }

        [ForeignKey("AlbumId")]
        [InverseProperty("AlbumGalleries")]
        public virtual Albums Album { get; set; }
        [ForeignKey("GalleryId")]
        [InverseProperty("AlbumGalleries")]
        public virtual Galleries Gallery { get; set; }
    }
}