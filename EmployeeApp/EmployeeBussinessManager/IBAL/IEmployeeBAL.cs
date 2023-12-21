using EmployeeApp.Models;

namespace EmployeeApp.EmployeeBussinessManager.IBAL
{
    public interface IEmployeeBAL
    {
        public List<EmployeeModel> GetEmployeeList();

        public EmployeeModel AddEmployee(EmployeeModel employeemodel, IFormFile imageFile);

        public bool CheckEmailExist(string emailId);

        public void DeleteEmployee(int id);

        public EmployeeModel PopulateData(int emp_id);

        public EmployeeModel UpdateEmployee(int id, EmployeeModel employeemodel, IFormFile file);

        public string uploadImage(IFormFile imageFile);
    }
}
