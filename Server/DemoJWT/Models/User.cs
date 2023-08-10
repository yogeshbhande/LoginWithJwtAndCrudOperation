using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DemoJWT.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string? Username { get; set; }

        public long? Contact { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }

    public class Jwt
    {
        public string key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
        public string Subject { get; set; }
    }
}
