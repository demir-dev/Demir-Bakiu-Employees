using Demir_Bakiu_Employees.Models;

namespace Demir_Bakiu_Employees.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<PairOfEmployeesResponse>> GetEmployeePairsFromCsv(IFormFile file);
    }
}
