using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ITLANetworking.Core.Application.ViewModels.User
{
    public class EditProfileViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe colocar el nombre")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe colocar el apellido")]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
        [RegularExpression(@"^(\+1|1)?[-.\s]?([809|829|849]{3})[-.\s]?[0-9]{3}[-.\s]?[0-9]{4}$", ErrorMessage = "El teléfono debe tener un formato válido.")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ProfilePictureFile { get; set; }

        public string? ProfilePicture { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }

        public int FriendsCount { get; set; }
        public int PostCount { get; set; }
        public int JuegosCount { get; set; }
    }
}
