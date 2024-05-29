using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Data
{
    [Table("Combo")]
    public class Combo
    {
        [Key]
        public int ComboId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [MaxLength(50)]
        public string ComboName { get; set; }
        [Range(0, double.MaxValue)]
        public int ComboPrice { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(400)")]
        public string ComboImage { get; set; }

        public ICollection<ComboItem> ComboItems { get; set; }
        public ICollection<Cart> Carts { get; set; }

    }
}
