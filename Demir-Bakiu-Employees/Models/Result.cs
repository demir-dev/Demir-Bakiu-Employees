namespace Demir_Bakiu_Employees.Models
{
    public class Result<T>
    {
        public string Message { get; set; }
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
    }
}
