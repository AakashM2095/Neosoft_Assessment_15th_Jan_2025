﻿using Microsoft.AspNetCore.Mvc;
using Neosoft_Assignment_15_02_2025.Interface;
using Neosoft_Assignment_15_02_2025.Models;
using Neosoft_Assignment_15_02_2025.ViewModel;

namespace Neosoft_Assignment_15_02_2025.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employee_DAL;
        private IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IEmployeeRepository employee_DAL, IWebHostEnvironment webHostEnvironment)
        {
            _employee_DAL = employee_DAL;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _employee_DAL.GetAllEmployees();
            var countries = (await _employee_DAL.GetCountriesAsync()).ToDictionary(c => c.Row_Id, c => c.CountryName);
            var states = (await _employee_DAL.GetAllStatesAsync()).ToDictionary(s => s.Row_Id, s => s.StateName);
            var cities = (await _employee_DAL.GetAllCitiesAsync()).ToDictionary(c => c.Row_Id, c => c.CityName);


            var viewModels = employees.Select(e => new EmployeeViewModel
            {
                EmployeeCode = e.EmployeeCode,
                FirstName = e.FirstName,
                LastName = e.LastName,
                CountryId = e.CountryId,
                StateId = e.StateId,
                CityId = e.CityId,
                CountryName = countries.ContainsKey(e.CountryId) ? countries[e.CountryId] : null,
                StateName = states.ContainsKey(e.StateId) ? states[e.StateId] : null,
                CityName = cities.ContainsKey(e.CityId) ? cities[e.CityId] : null,
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
        public async Task<IActionResult> AddEmployee()
        {
            ViewBag.Countries = await _employee_DAL.GetCountriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeViewModel model)
        {
            
            model.IsActive = model.IsActiveBool ? (byte)1 : (byte)0;
            bool email = await _employee_DAL.IsEmailUnique(model.EmailAddress);
            bool moblie = await _employee_DAL.IsMobileUnique(model.MobileNumber);
            bool pancard = await _employee_DAL.IsPanUnique(model.PanNumber);
            bool passport = await _employee_DAL.IsPassportUnique(model.PassportNumber);
            ViewBag.email = email? "Email already Exists":null;
            ViewBag.moblie = moblie ?  "Moblie Number already Exists":null;
            ViewBag.pancard = pancard ?  "Pancard already Exists":null;
            ViewBag.passport = passport ?  "Passport already Exists":null;
            if (!email && !moblie && !pancard && !passport)
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

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Countries = await _employee_DAL.GetCountriesAsync();
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(string employeeCode) 
        {
            ViewBag.Countries = await _employee_DAL.GetCountriesAsync();
            EmployeeMaster employee = await _employee_DAL.GetEmployeeByCodeAsync(employeeCode);
            EmployeeViewModel viewModel = new EmployeeViewModel();
            viewModel.EmployeeCode = employee.EmployeeCode;
            viewModel.FirstName = employee.FirstName;
            viewModel.LastName = employee.LastName;
            viewModel.CountryId = employee.CountryId;
            viewModel.StateId = employee.StateId;
            viewModel.CityId = employee.CityId;
            viewModel.EmailAddress = employee.EmailAddress;
            viewModel.MobileNumber = employee.MobileNumber;
            viewModel.PanNumber = employee.PanNumber;
            viewModel.PassportNumber = employee.PassportNumber;
            viewModel.ExistingPhotoPath = employee.ProfileImage;
            viewModel.Gender = employee.Gender;
            viewModel.IsActive = employee.IsActive;
            viewModel.DateOfBirth = employee.DateOfBirth;
            viewModel.DateOfJoinee  = employee.DateOfJoinee;
            viewModel.IsActiveBool = employee.IsActive == 1 ? true : false;

            return View(viewModel);
        }
                 

        [HttpPost]
        public IActionResult UpdateEmployee(EmployeeViewModel viewModel)
        {
            viewModel.IsActive = viewModel.IsActiveBool ? (byte)1 : (byte)0;
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


        [HttpGet]
        public async Task<IActionResult> GetCountries() => Json(await _employee_DAL.GetCountriesAsync());
        [HttpGet]
        public async Task<IActionResult> GetStates(int countryId) => Json(await _employee_DAL.GetStatesAsync(countryId));
        [HttpGet]
        public async Task<IActionResult> GetCities(int stateId) => Json(await _employee_DAL.GetCitiesAsync(stateId));
        [HttpGet]
        public async Task<IActionResult> GetEmployee(string employeeCode) => Json(await _employee_DAL.GetEmployeeByCodeAsync(employeeCode));

        [HttpGet]
        public async Task<IActionResult> GetAllStates(int countryId) => Json(await _employee_DAL.GetAllStatesAsync());
        [HttpGet]
        public async Task<IActionResult> GetAllCities(int stateId) => Json(await _employee_DAL.GetAllCitiesAsync());

        [HttpPost]
        public JsonResult IsUniqueEmail(string emailAddress)
        {
            var isUnique = _employee_DAL.IsEmailUnique(emailAddress); 
            return Json(isUnique);
        }

        [HttpPost]
        public JsonResult IsUniqueMobile(string mobileNumber)
        {
            var isUnique = _employee_DAL.IsMobileUnique(mobileNumber);
            return Json(isUnique);
        }

        [HttpPost]
        public JsonResult IsUniquePan(string panNumber)
        {
            var isUnique = _employee_DAL.IsPanUnique(panNumber);
            return Json(isUnique);
        }

        [HttpPost]
        public JsonResult IsUniquePassport(string passportNumber)
        {
            var isUnique = _employee_DAL.IsPassportUnique(passportNumber);
            return Json(isUnique);
        }


    }
}
