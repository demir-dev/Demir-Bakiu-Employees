namespace Demir_Bakiu_Employees.Models
{
    public class EmployeeCsvRecord
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
