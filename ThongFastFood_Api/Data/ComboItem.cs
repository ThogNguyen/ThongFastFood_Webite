using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Data
{
    [Table("ComboItem")]
    public class ComboItem
    {
        public int Combo_Id { get; set; }
        public int Product_Id { get; set; }

        [Range(0, 50)]
        public int Quantity { get; set; }

        // khóa ngoại 
        [ForeignKey("Combo_Id")]
        public Combo Combo { get; set; }

        [ForeignKey("Product_Id")]
        public Product Product { get; set; }
    }
}
