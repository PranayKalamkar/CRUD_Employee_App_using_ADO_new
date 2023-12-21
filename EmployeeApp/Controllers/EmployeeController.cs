using Microsoft.AspNetCore.Mvc;
using EmployeeApp.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting.Server;
using System.Data;
using System.Text.Json;
using EmployeeApp.EmployeeBussinessManager.IBAL;

namespace EmployeeApp.Controllers
{
    public class EmployeeController : Controller
    {
        IConfiguration _configuration;
        string connectionString;
        IEmployeeBAL _IEmployeeBAL;
        public EmployeeController(IConfiguration configuration,IEmployeeBAL employeeBAL)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
            _IEmployeeBAL = employeeBAL;
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
            return Json(_IEmployeeBAL.GetEmployeeList());
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
            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;

            var result = _IEmployeeBAL.AddEmployee(employee, file);

            if(result == null)
            {
                return Json("Email already exist!");
            }
            
            return Json("Index");
        }

        public IActionResult populateUpdateData(int emp_id)
        {
            return Json(_IEmployeeBAL.PopulateData(emp_id));

            //EmployeeModel employeemodel = null;

            //const string storedprocedure = "GetEmployeeById";

            //using (MySqlConnection connection = new MySqlConnection(connectionString))
            //{
            //    using (MySqlCommand command = new MySqlCommand(storedprocedure, connection))
            //    {
            //        command.CommandType = CommandType.StoredProcedure;

            //        command.Parameters.AddWithValue("@id_in", emp_id);

            //        connection.Open();

            //        MySqlDataReader reader = command.ExecuteReader();

            //        while (reader.Read())
            //        {
            //            employeemodel = new EmployeeModel();

            //            employeemodel.Id = (int)reader["id"];
            //            employeemodel.firstName = reader["first_name"].ToString();
            //            employeemodel.lastName = reader["last_name"].ToString();
            //            employeemodel.contactNumber = reader["contact_number"].ToString();
            //            employeemodel.emailId = reader["emailid"].ToString();
            //            employeemodel.age = reader["age"].ToString();
            //            employeemodel.imagePath = reader["image"].ToString();
            //        }
            //    }
            //}

            //return Json(employeemodel);
        }

        [HttpPost, RequestSizeLimit(25 * 1000 * 1024)]
        public IActionResult update(int id, string model, IFormFile file)
        {
            EmployeeModel employee = JsonSerializer.Deserialize<EmployeeModel>(model)!;

            _IEmployeeBAL.UpdateEmployee(id, employee, file);

            return Json("Index");
        }

        public IActionResult delete(int id)
        {
            _IEmployeeBAL.DeleteEmployee(id);

            return RedirectToAction("Index");
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
           
        public IActionResult test()
        {
            return Json(_IEmployeeBAL.GetEmployeeList());
        }
    }
}
