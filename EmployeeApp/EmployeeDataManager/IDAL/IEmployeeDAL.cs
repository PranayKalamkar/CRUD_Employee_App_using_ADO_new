using EmployeeApp.Models;

namespace EmployeeApp.EmployeeDataManager.IDAL
{
    public interface IEmployeeDAL
    {
        public List<EmployeeModel> GetEmployeeList();

        public EmployeeModel AddEmployee(EmployeeModel employeemodel);

        //public EmployeeModel PopulateData(int? emp_id);
    }
}
