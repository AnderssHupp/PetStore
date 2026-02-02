using Loja.Models.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;



namespace Loja.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity
    {
        public required string Name { get; set; }
        [Required,EmailAddress]
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;

        public Cart? Cart { get; set; }

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
