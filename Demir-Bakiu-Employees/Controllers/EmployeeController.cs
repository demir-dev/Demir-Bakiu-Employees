using Demir_Bakiu_Employees.Interfaces;
using Demir_Bakiu_Employees.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demir_Bakiu_Employees.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View(new Result<PairOfEmployeesResponse>());
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAndExtractFileDetails(IFormFile file)
        {
            var resultModel = new Result<PairOfEmployeesResponse>();

            if (file == null || file.Length == 0)
            {
                resultModel.IsSuccess = false;
                resultModel.Message = "No file uploaded.";
                return View("Index", resultModel);
            }

            if (!CheckIfFileIsCsv(file))
            {
                resultModel.IsSuccess = false;
                resultModel.Message = "Invalid file format. Please upload a CSV file.";
                return View("Index", resultModel);
            }

            try
            {
                var result = await _employeeService.GetEmployeePairsFromCsv(file);

                if (result == null || !result.Any())
                {
                    resultModel.IsSuccess = false;
                    resultModel.Message = "No valid employee pairs found.";
                    return View("Index", resultModel);
                }

                var longestWorkingPair = result
                    .OrderByDescending(p => p.CommonProjectDetails.Sum(d => d.DaysWorked))
                    .FirstOrDefault();

                resultModel.IsSuccess = true;
                resultModel.Data = longestWorkingPair;

                return View("Index", resultModel);
            }
            catch (Exception ex)
            {
                resultModel.IsSuccess = false;
                resultModel.Message = $"Error: {ex.Message}";
                return View("Index", resultModel);
            }
        }

        #region Helper Methods
        private bool CheckIfFileIsCsv(IFormFile file)
        {
            var allowedExtensions = new[] { ".csv" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }

        #endregion
    }
}
