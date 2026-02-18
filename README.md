# Employee Collaboration Analyzer

Employee Collaboration Analyzer mini-project which serves the solution for the assignment given by Sirma Group.

---

## 🚀 Key Features

* **Advanced Date Parsing**: Implements a culture-agnostic parsing engine that iterates through `CultureInfo.GetCultures()` to support various international date formats (ISO, US, European, Textual).
* **High-Performance Algorithm**: Uses a **Sweep-Line** style optimization. By grouping by Project ID and sorting by `DateFrom`, the algorithm achieves an efficient $O(P \cdot K \log K)$ complexity with early-exit logic to minimize unnecessary comparisons.
* **Strict CSV Validation**: Enforces a robust data contract (4-column validation) with specific error messaging to identify malformed data (e.g., "The value '34F' cannot be parsed").
* **NULL Handling**: Automatically treats `NULL` or empty `DateTo` values as `DateTime.Today`.
* **Modern MVC Architecture**: Built with a clean separation of concerns using Interfaces, Services, and Strongly-Typed Models.

---

## 🛠️ Technical Stack

* **Framework**: .NET 9.0 (ASP.NET Core MVC)
* **Language**: C# 13
* **Frontend**: Razor Views, Bootstrap 5

---

## 📖 How It Works

1.  **Grouping**: The system reads the CSV and groups records by `ProjectId`.
2.  **Sorting**: For each project, employees are sorted by their start date.
3.  **Overlap Calculation**: The algorithm checks for overlaps between employee periods.
    * $OverlapStart = \max(Start1, Start2)$
    * $OverlapEnd = \min(End1, End2)$
    * $Duration = OverlapEnd - OverlapStart$
4.  **Aggregation**: Shared time is summed across all common projects for every unique pair found.
5.  **Selection**: The pair with the highest aggregate duration is returned to the UI.

---

## 📂 Project Structure

* `Controllers/EmployeeController.cs`: Handles routing and file upload orchestration.
* `Services/EmployeeService.cs`: Contains the core logic for CSV parsing and overlap calculation.
* `Models/`: 
    * `EmployeeCsvRecord`: Internal representation of a CSV row.
    * `PairOfEmployeesResponse`: The final DTO containing the winning pair and project breakdown.
    * `Result<T>`: A generic wrapper for consistent API/View responses.
* `Views/Employee/Index.cshtml`: The primary user interface.

---

## 📥 Getting Started

1.  Clone the repository:
    ```bash
    git clone [https://github.com/DemirBakiu/Demir-Bakiu-employees.git](https://github.com/DemirBakiu/Demir-Bakiu-employees.git)
    ```
2.  Navigate to the project directory and run:
