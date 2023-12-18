using EmployeeApp.EmployeeBussinessManager.IBAL;
using EmployeeApp.EmployeeDataManager.DAL;
using EmployeeApp.EmployeeDataManager.IDAL;
using EmployeeApp.Models;

namespace EmployeeApp.EmployeeBussinessManager.BAL
{
    public class EmployeeBAL : IEmployeeBAL
    {
        IEmployeeDAL _IEmployeeDAL;
        public EmployeeBAL(IDBManager dBManager)
        {
            _IEmployeeDAL = new EmployeeDAL(dBManager);
        }
        List<EmployeeModel> IEmployeeBAL.GetEmployeeList()
        {
            return _IEmployeeDAL.GetEmployeeList();
        }
    }
}
