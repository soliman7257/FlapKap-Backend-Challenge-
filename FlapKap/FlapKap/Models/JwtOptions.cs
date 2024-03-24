
namespace FlapKap.Model
{
    public class JwtOptions
    {
        public string Issur { get; set; }
        public string Audience { get; set; }
        public int LifeTime { get; set; }
        public string SigningKey { get; set; }

    }
}
