using Microsoft.EntityFrameworkCore;
using Neosoft_Assignment_15_02_2025.Models;

namespace Neosoft_Assignment_15_02_2025.Interface
{
    public interface IEmployeeRepository
    {
        public Task<int> InsertEmployee(EmployeeMaster employee);
        public void UpdateEmployeeByCode(EmployeeMaster model);
        public void DeleteEmployeeByCode(string employeeCode);
        public Task<List<EmployeeMaster>> GetAllEmployees();
        public EmployeeMaster GetEmployeeByCode(string employeeCode);
        public Task<List<Country>> GetCountriesAsync();
        public Task<List<State>> GetStatesAsync(int countryId);
        public Task<List<City>> GetCitiesAsync(int stateId);
        public Task<EmployeeMaster> GetEmployeeByCodeAsync(string employeeCode);
        public Task<List<City>> GetAllCitiesAsync();
        public Task<List<State>> GetAllStatesAsync();
        public Task<bool> IsEmailUnique(string email);
        public Task<bool> IsMobileUnique(string mobile);
        public Task<bool> IsPanUnique(string pan);
        public Task<bool> IsPassportUnique(string passport);  

    }
}
