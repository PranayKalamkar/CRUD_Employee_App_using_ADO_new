using EmployeeApp.Models;

namespace EmployeeApp.EmployeeDataManager.IDAL
{
    public interface IEmployeeDAL
    {
        public List<EmployeeModel> GetEmployeeList();

        public EmployeeModel AddEmployee(EmployeeModel employeemodel);

        public bool CheckEmailExist(string emailId);

        public EmployeeModel PopulateData(int emp_id);

        public EmployeeModel UpdateEmployee(EmployeeModel employeemodel);

        public void DeleteEmployee(int id);

        public string GetEmployeeImageById1(int id);
    }
}
