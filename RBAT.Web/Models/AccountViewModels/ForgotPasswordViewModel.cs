using System.ComponentModel.DataAnnotations;

namespace RBAT.Web.Models.AccountViewModels {

    public class ForgotPasswordViewModel {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }

}
