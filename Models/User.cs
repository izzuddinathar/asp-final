using System;

namespace Final.Models
{
    public class User
    {
        public ulong UserId { get; set; }
        public string Nama { get; set; } // Full Name
        public string NoTelp { get; set; } // Phone Number
        public string Email { get; set; } // Unique Email
        public string Foto { get; set; } // Photo Path
        public string Username { get; set; } // Unique Username
        public string Password { get; set; } // Hashed Password
        public string Role { get; set; } // Enum: admin, owner, etc.
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
