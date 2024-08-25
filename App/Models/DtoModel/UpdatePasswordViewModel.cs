namespace App.Models.DtoModel
{
    public class UpdatePasswordViewModel
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

}
