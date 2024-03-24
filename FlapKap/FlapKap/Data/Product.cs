using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlapKap.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int AmountAvailable { get; set; }
        public decimal Cost { get; set; }
        public int SellerId { get; set; }
        [ForeignKey("SellerId")]
        public virtual User User { get; set; }  
    }
}
