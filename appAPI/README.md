# TelerikWebApp1

## Overview
This is a web application and API for interfacing with a person database initially populated with students and teachers.
It also includes a REACT frontend for doing CRUD operations on the database.

## Prerequisites
- NPM
- Visual Studio 2019 or later
- .NET Framework 4.7.2
- .NET8.0
- Telerik UI for ASP.NET AJAX
- Newtonsoft.Json

## Setup Instructions

### 1. Clone the Repository
Clone the repository to your local machine using the following command: git clone https://github.com/WYSIWYGallant/DotNetAPITest.git

### 2. Open the Solution
Open the `TelerikWebApp1.sln` file in Visual Studio.

### 3. Restore NuGet Packages
Restore the required NuGet packages by right-clicking on the solution in the Solution Explorer and selecting `Restore NuGet Packages`.

### 4. Add Project References
Ensure that the `appAPI` project is referenced in the `TelerikWebApp1` project:
- Right-click on the `TelerikWebApp1` project in the Solution Explorer.
- Select `Add` > `Reference...`.
- In the `Reference Manager`, check the `appAPI` project under the `Projects` tab and click `OK`.

### 5. Update the Target Framework
Ensure that the `appAPI` project targets .NET 8.0:
- Right-click on the `appAPI` project in the Solution Explorer.
- Select `Properties`.
- In the `Application` tab, set the `Target framework` to `.NET 8.0`.

### 6. Build the Solution
Build the solution by selecting `Build` > `Build Solution` from the menu.

### 7. NPM Run Build
Do an NPM Run Build in powershell
 - Once it's built copy the files from the Build folder to the root of the React-App folder.

### 8. Run install scripts 
Navigate to AppAPI/appAPI/Scripts
Run The Create Database script followed by the Create Tables script
 - This will create and populate your database and tables.

### 9. Run the Application
Run the application by pressing `F5` or selecting `Debug` > `Start Debugging` from the menu.

### 10. Run the REACT application
Run the application REACT app by navigating to \appAPI\TelerikWebApp1\wwwroot\react-app\ in powershell
 - and running npm start
 - access the react application by going to http://localhost:3000/
 - make sure you have the API running in VS to get data.


## API Endpoints
The application interacts with the following API endpoints:
- `GET https://localhost:7203/api/Person` - Retrieves a list of persons.
- `GET https://localhost:7203/api/PersonType` - Retrieves a list of person types.
- `GET https://localhost:7203/api/Person/maxPersonID` - Retrieves the maximum person ID.
- `POST https://localhost:7203/api/Person` - Inserts a new person.
- `PUT https://localhost:7203/api/Person/{personID}` - Updates an existing person.
- `DELETE https://localhost:7203/api/Person/{personID}` - Deletes a person.

## Troubleshooting
If you encounter any issues, ensure that:
- All NuGet packages are restored.
- The `appAPI` project is correctly referenced.
- The target framework for both TelerikWebApp1 is set to .NET Framework 4.7.2
- API uses .Net8.0
- The API endpoints are running and accessible.

## Contact
For any questions or issues, please contact [wyattegallant@gmail.com].
