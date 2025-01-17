using Microsoft.AspNetCore.Mvc;
using Neosoft_Assignment_15_02_2025.DAL;
using Neosoft_Assignment_15_02_2025.Models;
using Neosoft_Assignment_15_02_2025.ViewModel;

namespace Neosoft_Assignment_15_02_2025.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly Employee_DAL _employee_DAL;
        private IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(Employee_DAL employee_DAL, IWebHostEnvironment webHostEnvironment)
        {
            _employee_DAL = employee_DAL;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employee_DAL.GetAllEmployees();
            var viewModels = employees.Select(e => new EmployeeViewModel
            {
                EmployeeCode = e.EmployeeCode,
                FirstName = e.FirstName,
                LastName = e.LastName,
                CountryId = e.CountryId,
                StateId = e.StateId,
                CityId = e.CityId,
                EmailAddress = e.EmailAddress,
                MobileNumber = e.MobileNumber,
                PanNumber = e.PanNumber,
                PassportNumber = e.PassportNumber,
                ExistingPhotoPath = e.ProfileImage,
                Gender = e.Gender,
                IsActive = e.IsActive,
                DateOfBirth = e.DateOfBirth,
                DateOfJoinee = e.DateOfJoinee
            }).ToList();

            return View(viewModels);
        }



        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Image != null)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFilePath = Path.Combine(uploadFolder, uniqueFileName);
                    model.Image.CopyTo(new FileStream(uniqueFilePath, FileMode.Create));
                }

                EmployeeMaster employee = new EmployeeMaster
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CountryId = model.CountryId,
                    StateId = model.StateId,
                    CityId = model.CityId,
                    EmailAddress = model.EmailAddress,
                    MobileNumber = model.MobileNumber,
                    PanNumber = model.PanNumber,
                    PassportNumber = model.PassportNumber,
                    ProfileImage = uniqueFileName,
                    Gender = model.Gender,
                    IsActive = model.IsActive,
                    DateOfBirth = model.DateOfBirth,
                    DateOfJoinee = model.DateOfJoinee
                };

                await _employee_DAL.InsertEmployee(employee);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateEmployee(EmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                EmployeeMaster employee = _employee_DAL.GetEmployeeByCode(viewModel.EmployeeCode);
                employee.FirstName = viewModel.FirstName;
                employee.LastName = viewModel.LastName;
                employee.CountryId = viewModel.CountryId;
                employee.StateId = viewModel.StateId;
                employee.CityId = viewModel.CityId;
                employee.EmailAddress = viewModel.EmailAddress;
                employee.MobileNumber = viewModel.MobileNumber;
                employee.PanNumber = viewModel.PanNumber;
                employee.PassportNumber = viewModel.PassportNumber;
                employee.ProfileImage = viewModel.ExistingPhotoPath;
                employee.Gender = viewModel.Gender;
                employee.IsActive = viewModel.IsActive;
                employee.DateOfBirth = viewModel.DateOfBirth;
                employee.DateOfJoinee = viewModel.DateOfJoinee;

                if (viewModel.Image != null)
                {
                    if (employee.ProfileImage != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", viewModel.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    employee.ProfileImage = ProcessUploadedFile(viewModel);
                }

                _employee_DAL.UpdateEmployeeByCode(employee);
            }
            return RedirectToAction("Index");
        }

        private string ProcessUploadedFile(EmployeeViewModel model)
        {
            string uniqueFileName = null;
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return uniqueFileName;

        }

        public IActionResult DeleteEmployee(string employeeCode)
        {
            _employee_DAL.DeleteEmployeeByCode(employeeCode);
            return RedirectToAction("Index");
        }

        public IActionResult GetEmployee(string employeeCode)
        {
            var employee = _employee_DAL.GetEmployeeByCode(employeeCode);
            var viewModel = new EmployeeViewModel
            {
                EmployeeCode = employee.EmployeeCode,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                CountryId = employee.CountryId,
                StateId = employee.StateId,
                CityId = employee.CityId,
                EmailAddress = employee.EmailAddress,
                MobileNumber = employee.MobileNumber,
                PanNumber = employee.PanNumber,
                PassportNumber = employee.PassportNumber,
                ExistingPhotoPath = employee.ProfileImage,
                Gender = employee.Gender,
                IsActive = employee.IsActive,
                DateOfBirth = employee.DateOfBirth,
                DateOfJoinee = employee.DateOfJoinee
            };

            return View(viewModel);
        }

    }
}
