using Demir_Bakiu_Employees.Interfaces;
using Demir_Bakiu_Employees.Models;
using System.Globalization;

namespace Demir_Bakiu_Employees.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly CultureInfo[] _allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        public EmployeeService()
        {

        }

        public async Task<List<PairOfEmployeesResponse>> GetEmployeePairsFromCsv(IFormFile file)
        {
            Stream fileStream = file.OpenReadStream();
            var records = await ParseCsvLines(fileStream);

            var projects = records.GroupBy(r => r.ProjectId);
            var pairMap = new Dictionary<(int, int), PairOfEmployeesResponse>();

            foreach (var projectGroup in projects)
            {
                var allocatedProjects = projectGroup.OrderBy(a => a.DateFrom).ToList();
                int count = allocatedProjects.Count;

                for (int i = 0; i < count; i++)
                {
                    var employeeOne = allocatedProjects[i];

                    for (int j = i + 1; j < count; j++)
                    {
                        var employeeTwo = allocatedProjects[j];
                        if (employeeTwo.DateFrom >= employeeOne.DateTo)
                            break;

                        DateTime overlapStart = employeeOne.DateFrom > employeeTwo.DateFrom ? employeeOne.DateFrom : employeeTwo.DateFrom;
                        DateTime? overlapEnd = employeeOne.DateTo < employeeTwo.DateTo ? employeeOne.DateTo : employeeTwo.DateTo;

                        double totalDays = (overlapEnd - overlapStart)!.Value.TotalDays;
                        if (totalDays > 0)
                        {
                            UpdatePairMap(pairMap, employeeOne.EmployeeId, employeeTwo.EmployeeId, projectGroup.Key, totalDays);
                        }
                    }
                }
            }
            return pairMap.Values.ToList();
        }

        private async Task<List<EmployeeCsvRecord>> ParseCsvLines(Stream stream)
        {
            var list = new List<EmployeeCsvRecord>();
            using var reader = new StreamReader(stream);
            int lineNumber = 0;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',').Select(p => p.Trim()).ToArray();

                if (parts.Length != 4)
                {
                    throw new FormatException($"Line {lineNumber} is invalid. The CSV must have exactly 4 columns, but found {parts.Length}.");
                }

                if (parts[0].Equals("EmpID", StringComparison.InvariantCultureIgnoreCase) ||
                    parts[1].Equals("ProjectID", StringComparison.InvariantCultureIgnoreCase) ||
                    parts[2].Equals("DateFrom", StringComparison.InvariantCultureIgnoreCase) ||
                    parts[3].Equals("DateTo", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                int empId = ParseIdOrThrow(parts[0], "Employee ID");
                int projectId = ParseIdOrThrow(parts[1], "Project ID");

                list.Add(new EmployeeCsvRecord
                {
                    EmployeeId = empId,
                    ProjectId = projectId,
                    DateFrom = ParseDateAccordingToCultureAndFormat(parts[2]),
                    DateTo = string.Equals("NULL", parts[3], StringComparison.InvariantCultureIgnoreCase)
                        ? DateTime.Today
                        : ParseDateAccordingToCultureAndFormat(parts[3])
                });
            }
            return list;
        }

        private DateTime ParseDateAccordingToCultureAndFormat(string dateString)
        {
            if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime invariantCultureDate))
            {
                return invariantCultureDate;
            }

            if (DateTime.TryParse(dateString, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime currentCultureDate))
            {
                return currentCultureDate;
            }

            foreach (var culture in _allCultures)
            {
                if (DateTime.TryParse(dateString, culture, DateTimeStyles.None, out DateTime cultureDate))
                {
                    return cultureDate;
                }
            }

            throw new FormatException($"Unable to parse date string: {dateString}");
        }

        private void UpdatePairMap(Dictionary<(int, int), PairOfEmployeesResponse> map, int employeeOneId, int employeeTwoId, int projectId, double days)
        {
            int emp1 = Math.Min(employeeOneId, employeeTwoId);
            int emp2 = Math.Max(employeeOneId, employeeTwoId);
            var key = (emp1, emp2);

            if (!map.TryGetValue(key, out var response))
            {
                response = new PairOfEmployeesResponse
                {
                    EmployeeId1 = emp1,
                    EmployeeId2 = emp2
                };
                map[key] = response;
            }

            response.CommonProjectDetails.Add(new EmployeeProjectDetails()
            {
                ProjectId = projectId,
                DaysWorked = days
            });

        }

        private int ParseIdOrThrow(string value, string fieldName)
        {
            if (!int.TryParse(value, out int result))
            {
                throw new FormatException($"The value '{value}' for {fieldName} cannot be parsed, and this means that the CSV is not correct, please re-check your CSV file.");
            }
            return result;
        }

    }
}
