# MvcMovie

MvcMovie is a simple ASP.NET Core MVC application that demonstrates the basic functionality of a movie management system. This project was created as a learning tool to understand the basics of ASP.NET Core MVC, Entity Framework Core, and CRUD operations.

## Features

- Create, Read, Update, and Delete (CRUD) operations for movies.
- Validation of movie data, including required fields, string length, and regular expressions.
- Integration with Entity Framework Core for data access.
- Bootstrap-based UI for responsive design.

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with ASP.NET and web development workload installed.

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/katlicia/MVC-MovieApp.git
   cd MVC-MovieApp
2. Open the project in Visual Studio.

3. Restore the dependencies:
   ```bash
   dotnet restore
4. Update the database using Entity Framework Core migrations:
   ```bash
   dotnet ef database update
5. Run the application:
   ```bash
   dotnet run
Project Structure:
- Controllers/: Contains MVC controllers to handle user requests and return views.
- Models/: Defines the ViewModels used for data transfer between the UI and controllers, including data validation.
- Entities/: Defines the data entities that map to the database tables, representing the core data structure of the application.
- Views/: Contains the Razor views that render the UI.
- Data/: Includes the database context used for interacting with the database using Entity Framework Core.
- wwwroot/: Holds static files like CSS, JavaScript, and images.
  
Technologies Used:
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap
  
Contributing
Contributions are welcome! If you have any ideas, improvements, or suggestions, feel free to open an issue or submit a pull request.

License
This project is licensed under the MIT License - see the LICENSE file for details.

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)


