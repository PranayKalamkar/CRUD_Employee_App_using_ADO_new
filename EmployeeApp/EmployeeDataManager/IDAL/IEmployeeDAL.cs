using EmployeeApp.Models;

namespace EmployeeApp.EmployeeDataManager.IDAL
{
    public interface IEmployeeDAL
    {
        public List<EmployeeModel> GetEmployeeList();
    }
}
