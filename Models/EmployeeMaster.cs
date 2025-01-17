using System.ComponentModel.DataAnnotations;

namespace Neosoft_Assignment_15_02_2025.Models
{
    public class EmployeeMaster
    {
        public int Row_Id { get; set; }
        public string EmployeeCode { get; set; } 
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int StateId { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(15)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Mobile number must contain only digits.")]
        public string MobileNumber { get; set; }
        [Required]
        [MaxLength(12)]
        public string PanNumber { get; set; }
        [Required]
        [MaxLength(20)]
        public string PassportNumber { get; set; }
        [MaxLength(100)]
        public string ProfileImage { get; set; }
        public byte? Gender { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfJoinee { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }
}
