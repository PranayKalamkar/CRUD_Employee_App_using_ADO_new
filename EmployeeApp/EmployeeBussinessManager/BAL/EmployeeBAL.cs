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

            bool emailExist = CheckEmailExist(employeemodel.emailId);

            if(emailExist)
            {
                return null;
            }
            else
            {
                return _IEmployeeDAL.AddEmployee(employeemodel);
            }
        }

        public bool CheckEmailExist(string emailId)
        {
            return _IEmployeeDAL.CheckEmailExist(emailId);
        }

        public void DeleteEmployee(int id)
        {
            string existingImage = _IEmployeeDAL.GetEmployeeImageById1(id);

            if (!string.IsNullOrEmpty(existingImage))
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", existingImage);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _IEmployeeDAL.DeleteEmployee(id);
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

        public EmployeeModel PopulateData(int emp_id)
        {
            return _IEmployeeDAL.PopulateData(emp_id);
        }

        public EmployeeModel UpdateEmployee(int id,EmployeeModel employeemodel, IFormFile file)
        {
            employeemodel.Id = id;

            employeemodel.imageFile = file;

            string existingImage = _IEmployeeDAL.GetEmployeeImageById1(id);

            if(employeemodel.imageFile != null)
            {
                employeemodel.imagePath = uploadImage(employeemodel.imageFile);
            }
            else
            {
                employeemodel.imagePath = existingImage;
            }
            return _IEmployeeDAL.UpdateEmployee(employeemodel);
        }
    }
}
