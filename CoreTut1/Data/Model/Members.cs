using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    public partial class Members
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128)]
        public string AspNetRoleId { get; set; }
        [Required]
        [StringLength(128)]
        public string AspNetUserId { get; set; }
        [Required]
        [StringLength(250)]
        public string FullName { get; set; }
        [Required]
        [StringLength(250)]
        public string Email { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateInserted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateUpdated { get; set; }
        public int IsActive { get; set; }

        [ForeignKey("AspNetRoleId")]
        [InverseProperty("Members")]
        public virtual AspNetRoles AspNetRole { get; set; }
        [ForeignKey("AspNetUserId")]
        [InverseProperty("Members")]
        public virtual AspNetUsers AspNetUser { get; set; }
    }
}