using Microsoft.Data.SqlClient;
using Neosoft_Assignment_15_02_2025.Interface;
using Neosoft_Assignment_15_02_2025.Models;
using System.Data;

namespace Neosoft_Assignment_15_02_2025.DAL
{
    public class Employee_DAL : IEmployeeRepository
    {
        private readonly string _connectionString;

        public Employee_DAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> InsertEmployee(EmployeeMaster employee)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("stp_Emp_InsertEmployee", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@CountryId", employee.CountryId);
                    cmd.Parameters.AddWithValue("@StateId", employee.StateId);
                    cmd.Parameters.AddWithValue("@CityId", employee.CityId);
                    cmd.Parameters.AddWithValue("@EmailAddress", employee.EmailAddress);
                    cmd.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
                    cmd.Parameters.AddWithValue("@PanNumber", employee.PanNumber.ToUpper());
                    cmd.Parameters.AddWithValue("@PassportNumber", employee.PassportNumber.ToUpper());
                    cmd.Parameters.AddWithValue("@ProfileImage", employee.ProfileImage);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@IsActive", employee.IsActive);
                    cmd.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                    cmd.Parameters.AddWithValue("@DateOfJoinee", employee.DateOfJoinee);

                    conn.Open();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
                throw new Exception("An error occurred while inserting the employee record.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("An unexpected error occurred.", ex);
            }
        }

        public void UpdateEmployeeByCode(EmployeeMaster model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("stp_Emp_UpdateEmployeeByCode", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@EmployeeCode", model.EmployeeCode);
                    command.Parameters.AddWithValue("@FirstName", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", model.LastName);
                    command.Parameters.AddWithValue("@CountryId", model.CountryId);
                    command.Parameters.AddWithValue("@StateId", model.StateId);
                    command.Parameters.AddWithValue("@CityId", model.CityId);
                    command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                    command.Parameters.AddWithValue("@MobileNumber", model.MobileNumber);
                    command.Parameters.AddWithValue("@PanNumber", model.PanNumber);
                    command.Parameters.AddWithValue("@PassportNumber", model.PassportNumber);
                    command.Parameters.AddWithValue("@ProfileImage", model.ProfileImage);
                    command.Parameters.AddWithValue("@Gender", model.Gender);
                    command.Parameters.AddWithValue("@IsActive", model.IsActive);
                    command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                    command.Parameters.AddWithValue("@DateOfJoinee", model.DateOfJoinee);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the employee.", ex);
            }
        }

        public void DeleteEmployeeByCode(string employeeCode)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("stp_Emp_DeleteEmployee", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@EmployeeCode", employeeCode);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the employee.", ex);
            }
        }

        public  EmployeeMaster GetEmployeeByCode(string employeeCode)
        {
            EmployeeMaster employee = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("stp_Emp_GetEmployeeByID", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@EmployeeCode", employeeCode);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new EmployeeMaster
                        {
                            Row_Id = (int)reader["Row_Id"],
                            EmployeeCode = reader["EmployeeCode"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            CountryId = (int)reader["CountryId"],
                            StateId = (int)reader["StateId"],
                            CityId = (int)reader["CityId"],
                            EmailAddress = reader["EmailAddress"].ToString(),
                            MobileNumber = reader["MobileNumber"].ToString(),
                            PanNumber = reader["PanNumber"].ToString(),
                            PassportNumber = reader["PassportNumber"].ToString(),
                            ProfileImage = reader["ProfileImage"].ToString(),
                            Gender = reader["Gender"] as byte?,
                            //IsActive = reader["IsActive"] as byte?,
                            IsActive = (byte)((bool)reader["IsActive"] ? 1 : 0),
                            DateOfBirth = (DateTime)reader["DateOfBirth"],
                            DateOfJoinee = reader["DateOfJoinee"] as DateTime?

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the employee.", ex);
            }
            return employee;
        }

        public async Task<List<EmployeeMaster>> GetAllEmployees()
        {
            try
            {
                List<EmployeeMaster> employees = new List<EmployeeMaster>();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("stp_Emp_GetAllEmployees", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        employees.Add(new EmployeeMaster
                        {
                            Row_Id = (int)reader["Row_Id"],
                            EmployeeCode = reader["EmployeeCode"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            CountryId = (int)reader["CountryId"],
                            StateId = (int)reader["StateId"],
                            CityId = (int)reader["CityId"],
                            EmailAddress = reader["EmailAddress"].ToString(),
                            MobileNumber = reader["MobileNumber"].ToString(),
                            PanNumber = reader["PanNumber"].ToString(),
                            PassportNumber = reader["PassportNumber"].ToString(),
                            ProfileImage = reader["ProfileImage"].ToString(),
                            Gender = reader["Gender"] as byte?,
                            IsActive = (byte)((bool)reader["IsActive"] ? 1 : 0),
                            DateOfBirth = (DateTime)reader["DateOfBirth"],
                            DateOfJoinee = reader["DateOfJoinee"] as DateTime?
                        });
                    }
                }
                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the list of employees.", ex);
            }
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            var countries = new List<Country>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("stp_GetCountries", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            countries.Add(new Country
                            {
                                Row_Id = reader.GetInt32(reader.GetOrdinal("Row_Id")),
                                CountryName = reader.GetString(reader.GetOrdinal("CountryName"))
                            });
                        }
                    }
                    return countries;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Country is : " + ex);
            }


        }

        public async Task<List<State>> GetStatesAsync(int countryId)
        {
            var states = new List<State>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("stp_GetStates", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CountryId", countryId);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            states.Add(new State
                            {
                                Row_Id = reader.GetInt32(reader.GetOrdinal("Row_Id")),
                                CountryId = reader.GetInt32(reader.GetOrdinal("CountryId")),
                                StateName = reader.GetString(reader.GetOrdinal("StateName"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in State is : " + ex);
            }

            return states;
        }

        public async Task<List<City>> GetCitiesAsync(int stateId)
        {
            var cities = new List<City>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("stp_GetCities", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StateId", stateId);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            cities.Add(new City
                            {
                                Row_Id = reader.GetInt32(reader.GetOrdinal("Row_Id")),
                                StateId = reader.GetInt32(reader.GetOrdinal("StateId")),
                                CityName = reader.GetString(reader.GetOrdinal("CityName"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in City is : " + ex);
            }

            return cities;
        }

        public async Task<EmployeeMaster> GetEmployeeByCodeAsync(string employeeCode)
        {
            EmployeeMaster employee = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("stp_Emp_GetEmployeeByID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeCode", employeeCode);

                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            employee = new EmployeeMaster
                            {
                                Row_Id = (int)reader["Row_Id"],
                                EmployeeCode = reader["EmployeeCode"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                CountryId = (int)reader["CountryId"],
                                StateId = (int)reader["StateId"],
                                CityId = (int)reader["CityId"],
                                EmailAddress = reader["EmailAddress"].ToString(),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                PanNumber = reader["PanNumber"].ToString(),
                                PassportNumber = reader["PassportNumber"].ToString(),
                                ProfileImage = reader["ProfileImage"].ToString(),
                                Gender = reader["Gender"] as byte?,
                                IsActive = (byte)((bool)reader["IsActive"] ? 1 : 0),
                                DateOfBirth = (DateTime)reader["DateOfBirth"],
                                DateOfJoinee = reader["DateOfJoinee"] as DateTime?
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the employee.", ex); ;
            }

            return employee;
        }

        public async Task<List<State>> GetAllStatesAsync()
        {
            var states = new List<State>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("stp_Get_All_States", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            states.Add(new State
                            {
                                Row_Id = reader.GetInt32(reader.GetOrdinal("Row_Id")),
                                CountryId = reader.GetInt32(reader.GetOrdinal("CountryId")),
                                StateName = reader.GetString(reader.GetOrdinal("StateName"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in State is : " + ex);
            }

            return states;
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            var cities = new List<City>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("stp_Get_All_Cities", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    conn.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            cities.Add(new City
                            {
                                Row_Id = reader.GetInt32(reader.GetOrdinal("Row_Id")),
                                StateId = reader.GetInt32(reader.GetOrdinal("StateId")),
                                CityName = reader.GetString(reader.GetOrdinal("CityName"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in City is : " + ex);
            }

            return cities;
        }
    }
}

