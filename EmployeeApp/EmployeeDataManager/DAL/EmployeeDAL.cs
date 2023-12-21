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
            
            return employeeList;
        }

        public EmployeeModel AddEmployee(EmployeeModel employeemodel)
        {
            _dBManager.InitDbCommand("InsertEmployee");

            _dBManager.AddCMDParam("@firstName", employeemodel.firstName);
            _dBManager.AddCMDParam("@lastName", employeemodel.lastName);
            _dBManager.AddCMDParam("@contactNumber", employeemodel.contactNumber);
            _dBManager.AddCMDParam("@emailId", employeemodel.emailId);
            _dBManager.AddCMDParam("@age", employeemodel.age);
            _dBManager.AddCMDParam("@imagePath", employeemodel.imagePath);

            _dBManager.ExecuteNonQuery();

            return employeemodel;
        }

        public bool CheckEmailExist(string emailId)
        {
            _dBManager.InitDbCommand("CheckEmailExist");

            _dBManager.AddCMDParam("@newEmail", emailId);

            var result = _dBManager.ExecuteScalar();

            bool emailExist = Convert.ToBoolean(result);

            return emailExist;
        }

        public void DeleteEmployee(int id)
        {
            

            _dBManager.InitDbCommand("DeleteEmployeeById");

            _dBManager.AddCMDParam("@employeeId", id);

            _dBManager.ExecuteNonQuery();

            
        }

        public string GetEmployeeImageById1(int id)
        {
            string existingImage = null;

            _dBManager.InitDbCommand("GetEmployeeImageById1");

            _dBManager.AddCMDParam("@id_in", id);

            DataSet ds = _dBManager.ExecuteDataSet();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                existingImage = item["image"].ConvertJSONNullToString();
            }

            return existingImage;
        }

        public EmployeeModel PopulateData(int emp_id)
        {
            _dBManager.InitDbCommand("GetEmployeeById");

            EmployeeModel employeemodel = null;

            _dBManager.AddCMDParam("@id_in", emp_id);

            DataSet ds = _dBManager.ExecuteDataSet();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                employeemodel = new EmployeeModel();

                //employeemodel.Id = (int)reader["id"];
                employeemodel.Id = item["id"].ConvertDBNullToInt();
                //employeemodel.firstName = CommonConversion.ConvertDBNullToString(item["first_name"]);
                employeemodel.firstName = item["first_name"].ConvertDBNullToString();
                employeemodel.lastName = item["last_name"].ConvertDBNullToString();
                employeemodel.contactNumber = item["contact_number"].ConvertJSONNullToString();
                employeemodel.emailId = item["emailid"].ConvertJSONNullToString();
                employeemodel.age = item["age"].ConvertJSONNullToString();
                employeemodel.imagePath = item["image"].ConvertJSONNullToString();
            }
            return employeemodel;
        }

        public EmployeeModel UpdateEmployee(EmployeeModel employeemodel)
        {
            _dBManager.InitDbCommand("UpdateEmployeeById");

            _dBManager.AddCMDParam("employeeId", employeemodel.Id);
            _dBManager.AddCMDParam("first_name", employeemodel.firstName);
            _dBManager.AddCMDParam("last_name", employeemodel.lastName);
            _dBManager.AddCMDParam("contact_number", employeemodel.contactNumber);
            _dBManager.AddCMDParam("emailid", employeemodel.emailId);
            _dBManager.AddCMDParam("age", employeemodel.age);
            _dBManager.AddCMDParam("imagePath", employeemodel.imagePath);

            _dBManager.ExecuteNonQuery();

            return employeemodel;
        }
    }
}
