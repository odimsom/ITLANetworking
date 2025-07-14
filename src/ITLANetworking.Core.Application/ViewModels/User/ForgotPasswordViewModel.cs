using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.ViewModels.User
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Debe colocar el correo")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
