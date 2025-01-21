using System.ComponentModel.DataAnnotations;

namespace Neosoft_Assignment_15_02_2025.ViewModel
{
    public class EmployeeUpdateViewModel
    {
        public string? EmployeeCode { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name can't be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name can't be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [Phone(ErrorMessage = "Invalid Mobile Number format.")]
        public string? MobileNumber { get; set; }

        [Required(ErrorMessage = "Pan Number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Pan Number must be 10 characters.")]
        [RegularExpression("^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid Pan Number format.")]
        public string? PanNumber { get; set; }

        [Required(ErrorMessage = "Passport Number is required.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Passport Number should be between 5 and 15 characters.")]
        [RegularExpression("^[A-Z0-9]+$", ErrorMessage = "Invalid Passport Number format.")]
        public string? PassportNumber { get; set; }

        // [Required(ErrorMessage = "Profile Picture is required.")]
        // [FileExtensions(Extensions = "jpg,png", ErrorMessage = "Profile picture must be in JPG or PNG format.")]
        // [MaxFileSize(200 * 1024, ErrorMessage = "Profile picture size must be less than 200 KB.")]
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public byte? Gender { get; set; }

        // [Required(ErrorMessage = "Status is required.")]
        public byte? IsActive { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [LessThanToday(ErrorMessage = "Date of Birth must be less than today's date.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Date of Joinee is required.")]
        [DataType(DataType.Date)]
        [LessThanToday(ErrorMessage = "Date of Joinee must be less than today's date.")]
        public DateTime? DateOfJoinee { get; set; }
        public string? ExistingPhotoPath { get; set; }
        public bool IsActiveBool { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public string? CityName { get; set; }
    }
}
