# Finance Tracker Repository

A finance tracking web application built with Angular 19 and .NET 8 Web API, utilizing SQL Server for the database.

## Running the Project

You can run this app locally by following the steps below.

### Prerequisites

1. Docker
2. .NET SDK v8
3. NodeJS (at least version 20.11.1) - Optional if you want to run the Angular app separately in development mode

### Clone the Repository

Clone the project to your local machine:

```bash
git clone https://github.com/AnassEREKYSY/Finance_Tracker.git
cd Finance_Tracker

Restore Packages

From the solution folder, restore the .NET packages:
```bash
dotnet restore

For the client, install the necessary Node packages:
```bash
cd client
npm install

Set Up the Database

To create and start the SQL Server database, run Docker Compose inside the server folder:
```bash
cd server
docker compose up -d

Running the Application

To start the API, run the following command in the server folder:
```bash
cd server/API
dotnet run

Accessing the App

Once the API is running, you can access the application at https://localhost:5001.

If you wish to run the Angular client separately, run:
```bash
cd client
ng serve


Additional Configuration

Ensure you have the correct configuration for your database in appsettings.json. For local database setups, you can adjust the SQL Server connection string as necessary.
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FinanceTrackerDB;User Id=sa;Password=your_password;"
  }
}

For detailed installation instructions or troubleshooting, refer to the official documentation of Docker, Angular, and .NET.

Feel free to copy and paste this into your GitHub repository!

