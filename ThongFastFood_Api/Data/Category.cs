using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Data
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
		
		[Column(TypeName = "nvarchar(50)")]
		public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
