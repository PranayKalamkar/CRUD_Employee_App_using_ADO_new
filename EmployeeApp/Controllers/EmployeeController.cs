using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting.Server;
using System.Data;
using System.Text.Json;

namespace EmployeeApp.Controllers
{
    public class EmployeeController : Controller
    {
        IConfiguration _configuration;
        string connectionString;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            const string storedProcedure = "GetAllEmployees";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(storedProcedure, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) 
                {
                    EmployeeModel employeemodel = new EmployeeModel();

                    employeemodel.Id = (int)reader["id"];
                    employeemodel.firstName = reader["first_name"].ToString();
                    employeemodel.lastName = reader["last_name"].ToString();
                    employeemodel.contactNumber = reader["contact_number"].ToString();
                    employeemodel.emailId = reader["emailid"].ToString();
                    employeemodel.age = reader["age"].ToString();
                    employeemodel.imagePath = reader["image"].ToString();

                    employeeList.Add(employeemodel);
                }
            }
            return View(employeeList);
        } 
        public IActionResult EmployeeList()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            const string storedProcedure = "GetAllEmployees";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(storedProcedure, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) 
                {
                    EmployeeModel employeemodel = new EmployeeModel();

                    employeemodel.Id = (int)reader["id"];
                    employeemodel.firstName = reader["first_name"].ToString();
                    employeemodel.lastName = reader["last_name"].ToString();
                    employeemodel.contactNumber = reader["contact_number"].ToString();
                    employeemodel.emailId = reader["emailid"].ToString();
                    employeemodel.age = reader["age"].ToString();
                    employeemodel.imagePath = reader["image"].ToString();

                    employeeList.Add(employeemodel);
                }
            }
            return Json(employeeList);
        }

        public IActionResult search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult search(string keyword)
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            const string storedProcedure = "SearchEmployee(keyword)";
            //const string queryString = "SELECT * from employee where id = @keyword;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(storedProcedure, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    EmployeeModel employeemodel = new EmployeeModel();
                    employeemodel.Id = (int)reader["id"];
                    employeemodel.firstName = reader["first_name"].ToString();
                    employeemodel.lastName = reader["last_name"].ToString();
                    employeemodel.contactNumber = reader["contact_number"].ToString();
                    employeemodel.emailId = reader["emailid"].ToString();
                    employeemodel.age = reader["age"].ToString();

                    employeeList.Add(employeemodel);
                }
            }
            return View("~/Views/Employee/index.cshtml", employeeList);
        }

        public IActionResult create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult create( string model,IFormFile file)
        {
            //AddEmployeeModel test = new();
            
            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;
            employee.imageFile = file;

            employee.imagePath = uploadImage(employee.imageFile);

            Console.WriteLine(employee.imagePath);
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    const string storedProcedure = "InsertEmployee";
                    using (MySqlCommand command = new MySqlCommand(storedProcedure, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.Open();
                        command.Parameters.AddWithValue("@firstName", employee.firstName);
                        command.Parameters.AddWithValue("@lastName", employee.lastName);
                        command.Parameters.AddWithValue("@contactNumber", employee.contactNumber);
                        command.Parameters.AddWithValue("@emailId", employee.emailId);
                        command.Parameters.AddWithValue("@age", employee.age);
                        command.Parameters.AddWithValue("@imagePath", employee.imagePath);
                        command.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
            return Json("Index");
        }

        public IActionResult populateUpdateData(int? emp_id)
        {
            EmployeeModel employeemodel = null;

            const string storedprocedure = "GetEmployeeById";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(storedprocedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@id_in", emp_id);

                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        employeemodel = new EmployeeModel();

                        employeemodel.Id = (int)reader["id"];
                        employeemodel.firstName = reader["first_name"].ToString();
                        employeemodel.lastName = reader["last_name"].ToString();
                        employeemodel.contactNumber = reader["contact_number"].ToString();
                        employeemodel.emailId = reader["emailid"].ToString();
                        employeemodel.age = reader["age"].ToString();
                        employeemodel.imagePath = reader["image"].ToString();
                    }
                }
            }

            return Json(employeemodel);
        }

        [HttpPost]
        public IActionResult update(int id, string model, IFormFile file)
        {
            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;

            employee.Id = id;

            employee.imageFile = file;

            employee.imagePath = uploadImage(employee.imageFile);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string queryString = "UpdateEmployeeById";
                    using (MySqlCommand command = new MySqlCommand(queryString, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        connection.Open();
                        command.Parameters.AddWithValue("employeeId", employee.Id);
                        command.Parameters.AddWithValue("first_name", employee.firstName);
                        command.Parameters.AddWithValue("last_name", employee.lastName);
                        command.Parameters.AddWithValue("contact_number", employee.contactNumber);
                        command.Parameters.AddWithValue("emailid", employee.emailId);
                        command.Parameters.AddWithValue("age", employee.age);
                        command.Parameters.AddWithValue("imagePath", employee.imagePath);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json("Index");
        }

        public IActionResult delete(int? id)
        {
            string existingImage = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // string queryString = "SELECT profile_image FROM employee WHERE emp_id = @Id;";

                string StoredProcedure = "GetEmployeeImageById1";

                using (MySqlCommand command = new MySqlCommand(StoredProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@id_in", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            existingImage = reader["image"].ToString();
                        }
                    }
                }
            }

            // Delete the old image file if it exists

            if (!string.IsNullOrEmpty(existingImage))
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", existingImage);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string queryString = "DeleteEmployeeById";

                using (MySqlCommand command = new MySqlCommand(queryString, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@employeeId", id);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
         
            return Json("Index");
        }

        public string uploadImage(IFormFile imageFile)
        {

            try
            {
                string uniqueFileName = null;

                if (imageFile != null)
                {
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image");

                    // Create the directory if it doesn't exist

                    Console.WriteLine(Directory.GetCurrentDirectory());

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                }
                else
                {
                    Console.WriteLine("Image file path is null");
                }

                Console.WriteLine(uniqueFileName);

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }
        }

        
    }
}
