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

        public EmployeeModel AddEmployee(EmployeeModel employeemodel, IFormFile imageFile)
        {
            employeemodel.imageFile = imageFile;

            employeemodel.imagePath = uploadImage(employeemodel.imageFile);

            return _IEmployeeDAL.AddEmployee(employeemodel);
        }

        public EmployeeModel DeleteEmployee(int? id)
        {
            return _IEmployeeDAL.DeleteEmployee(id);
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

        //EmployeeModel IEmployeeBAL.PopulateData(int? emp_id)
        //{
        //    return null;
        //}
    }
}
