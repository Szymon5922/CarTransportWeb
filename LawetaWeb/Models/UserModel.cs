using System.Security.Cryptography;
using System.Text;

namespace LawetaWeb.Models
{
    public class UserModel
    {
        private string _passwordHash;
        public string UserName { get; set; }
        public string Password { get => _passwordHash; set => _passwordHash = HashPassword(value); }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create()) 
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                
                foreach(byte b in hashedBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
