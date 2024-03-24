using FlapKap.Data;
using System.ComponentModel.DataAnnotations;

namespace FlapKap.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Deposit { get; set; }
        public String Role { get; set; }
    }
    
}

