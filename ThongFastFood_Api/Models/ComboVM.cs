using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Models
{
    public class ComboVM
    {
        public int ComboId { get; set; }
        public string ComboName { get; set; }
        public int ComboPrice { get; set; }
        public string? ComboImage { get; set; }
    }
}
