using EmployeeApp.Models;

namespace EmployeeApp.EmployeeBussinessManager.IBAL
{
    public interface IEmployeeBAL
    {
        public List<EmployeeModel> GetEmployeeList();
    }
}
