using EmployeeApp.CommonCode;
using EmployeeApp.EmployeeDataManager.IDAL;
using EmployeeApp.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace EmployeeApp.EmployeeDataManager.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {

        readonly IDBManager _dBManager;
        public EmployeeDAL(IDBManager dBManager)
        {
            _dBManager = dBManager;
            //_configuration = configuration;
            //_connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<EmployeeModel> GetEmployeeList()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            _dBManager.InitDbCommand("GetAllEmployees");

            DataSet ds =_dBManager.ExecuteDataSet();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                EmployeeModel employeemodel = new EmployeeModel();

                //employeemodel.Id = (int)reader["id"];
                employeemodel.Id = item["id"].ConvertDBNullToInt();
                //employeemodel.firstName = CommonConversion.ConvertDBNullToString(item["first_name"]);
                employeemodel.firstName = item["first_name"].ConvertDBNullToString();
                employeemodel.lastName = item["last_name"].ConvertDBNullToString();
                employeemodel.contactNumber = item["contact_number"].ConvertDBNullToString();
                employeemodel.emailId = item["emailid"].ConvertDBNullToString();
                employeemodel.age = item["age"].ConvertDBNullToString();
                employeemodel.imagePath = item["image"].ConvertDBNullToString();

                employeeList.Add(employeemodel);

            }
            //const string storedProcedure = "GetAllEmployees";


            //using (MySqlConnection connection = new MySqlConnection())
            //using (MySqlCommand command = new MySqlCommand(storedProcedure, connection))
            //{
            //    command.CommandType = System.Data.CommandType.StoredProcedure;
            //    connection.Open();

            //    MySqlDataReader reader = command.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        EmployeeModel employeemodel = new EmployeeModel();

            //        employeemodel.Id = (int)reader["id"];
            //        employeemodel.firstName = reader["first_name"].ToString();
            //        employeemodel.lastName = reader["last_name"].ToString();
            //        employeemodel.contactNumber = reader["contact_number"].ToString();
            //        employeemodel.emailId = reader["emailid"].ToString();
            //        employeemodel.age = reader["age"].ToString();
            //        employeemodel.imagePath = reader["image"].ToString();

            //        employeeList.Add(employeemodel);
            //    }
            //}
            return employeeList;
        }

       
    }
}
