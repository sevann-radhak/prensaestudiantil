using System.ComponentModel.DataAnnotations;

namespace prensaestudiantil.Common.Models
{
    public class UserRequest
    {

        [Required]
        [StringLength(60)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(60)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
