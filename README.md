GOTG Ronewa — Disaster Alleviation Foundation
GOTG Ronewa is a web application built to facilitate disaster relief efforts by connecting the foundation with volunteers and managing critical tasks. The application provides a platform for volunteers to register, browse and accept tasks, and for the foundation to manage volunteer profiles and coordinate relief efforts.
Key Features
•	Volunteer Registration: A secure registration process for individuals to join the volunteer team.
•	Profile Management: Volunteers can manage their skills, contact information, and view their assigned tasks.
•	Task Management: The application allows the foundation to create and assign disaster relief tasks.
•	Task Browsing: Volunteers can browse a list of available tasks and accept them.
•	Role-Based Access: Secure login and authorization ensure that users can only access the features relevant to their role (e.g., volunteer vs. administrator).
Technologies Used
•	Backend: ASP.NET Core MVC and C# for the server-side logic and API.
•	Database: Entity Framework Core is used for data persistence and database management via migrations.
•	Frontend: Razor Pages and a combination of standard HTML, CSS, and JavaScript for the user interface.
•	Authentication: ASP.NET Core Identity for user registration, login, and profile management.
•	Debugging: The Visual Studio debugger was instrumental in resolving issues related to server-side validation and model binding.
Setup and Installation
Follow these steps to get the project up and running on your local machine.
Prerequisites
•	.NET SDK (latest version recommended)
•	Visual Studio or Visual Studio Code with the C# extension.
•	SQL Server LocalDB (typically included with Visual Studio)
Steps
1.	Clone the Repository:
   git clone [repository_url]
   cd GOTG.Ronewa
2.	Restore Dependencies:
   dotnet restore
3.	Update the Database:
   Add-Migration InitialCreate
   Update-Database
   (If migrations already exist, just run Update-Database.)
4.	Run the Application:
   dotnet run
   The application will launch and be accessible at https://localhost:7075.
Project Structure
•	Controllers/: Contains the MVC controllers that handle user requests (e.g., VolunteersController.cs).
•	Models/: Defines the application's data models (e.g., VolunteerProfile.cs, VolunteerTask.cs).
•	Services/: Houses the business logic and data access layer.
•	Views/: Contains the Razor views (.cshtml files) that render the user interface.
•	wwwroot/: Stores static assets such as CSS, JavaScript, and images.
Contributing
We welcome contributions from the community. Please follow standard Git practices:
5.	Fork the repository.
6.	Create a new branch (git checkout -b feature/your-feature-name).
7.	Make your changes and commit them (git commit -m 'Add new feature').
8.	Push to the branch (git push origin feature/your-feature-name).
9.	Open a Pull Request.
License
This project is licensed under the MIT License. See the LICENSE file for details.
<img width="432" height="646" alt="image" src="https://github.com/user-attachments/assets/64c086d7-57c2-4fcf-955e-74772f0e18fa" />





