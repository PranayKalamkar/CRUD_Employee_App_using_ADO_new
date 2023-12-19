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

        public void DeleteEmployee(int? id)
        {
            

            _dBManager.InitDbCommand("DeleteEmployeeById");

            _dBManager.AddCMDParam("@employeeId", id);

            _dBManager.ExecuteNonQuery();

            
        }

        //public EmployeeModel PopulateData(int? emp_id)
        //{
        //    EmployeeModel employeemodel = null;

        //    _dBManager.InitDbCommand("GetEmployeeById");

        //    DataSet ds = _dBManager.ExecuteDataSet();

        //    return employeemodel;
        //}
    }
}
