namespace Demir_Bakiu_Employees.Models
{
    public class PairOfEmployeesResponse
    {
        public int EmployeeId1 { get; set; }
        public int EmployeeId2 { get; set; }
        public List<EmployeeProjectDetails> CommonProjectDetails { get; set; } = new();
    }

    public class EmployeeProjectDetails
    {
        public int ProjectId { get; set; }
        public double DaysWorked { get; set; }
    }
}
