using System.ComponentModel.DataAnnotations;

namespace IdentityService.Pages.Account.Register
{
    public class RegisterViewModel
    {
        public string? ReturnUrl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string? Button { get; set; }
    }
}
