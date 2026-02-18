# Employee Collaboration Analyzer

Employee Collaboration Analyzer mini-project which serves the solution for the assignment given by Sirma Group.

---

## 📥 Instructions to Run the Project

This project is implemented as a **Web MVC Application** to specifically fulfill the assignment's bonus requirements regarding **UI implementation** and **extended date format support**. By choosing this architecture, the traditional console-based processing is replaced with a more intuitive, professional, and visually clear interface.

Once the application is started, you will be automatically directed to the analyzer interface.

### Steps to Execute:

1. **Clone the Repository**

2. **Run the Application**:
* **Via CLI**: Execute `dotnet run`
* **Via Visual Studio**: Open the `.sln` file and press **F5** or the **Play** button (Ensure the startup project is set to the MVC project).


3. **Analyze Your Data (UI Method)**:
* The application will automatically launch in your default web browser.
* Instead of manual console entries, use the **"Choose File"** button to select any `.csv` file from your local machine.
* Click **"Analyze Pairs"**.
* The system will instantly process the data and display the results on the UI, featuring the longest-working pair and a clear, styled table of their shared project history.

---

## 🚀 Key Features

* **Advanced Date Parsing**: Implements a culture-agnostic parsing engine that iterates through `CultureInfo.GetCultures()` to support various international date formats (ISO, US, European, Textual).
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

