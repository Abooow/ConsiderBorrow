# ConsiderBorrow - Library Management Web Application
This is a web application developed using ASP.NET Core, .NET 7, and SQL Server. It allows you to manage library items and employees.

## Getting Started
To run this application, make sure you have the following installed:
* ASP.NET Core 7 Runtime
* .NET 7 SDK
* SQL Server

You can download .NET 7 [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## Getting Started
1. Clone the repository using the following command:
```sh
git clone https://github.com/Abooow/ConsiderBorrow.git
```

3. Navigate to the root directory of the project:
```sh
cd ConsiderBorrow/src/Server
```

4. Run the project using the .NET CLI:
```sh
dotnet run
```

The application will be accessible at [https://localhost:7267](https://localhost:7267)

## Testing the API
To test the API endpoints and interact with the application, you can [download the Postman collection](ConsiderBorrow.postman_collection.json) provided. The Postman collection file contains a set of pre-configured requests that you can import into Postman.

To get started with the Postman collection:
1. Download the [ConsiderBorrow.postman_collection.json](ConsiderBorrow.postman_collection.json) file from the repository.
2. Open Postman and navigate to the Collections tab.
3. Click on "Import" and select the downloaded JSON file.
4. The collection will be imported into Postman, and you can see the available requests in the sidebar.

Read more about how to import an collection into Postman [here](https://learning.postman.com/docs/getting-started/importing-and-exporting-data/#importing-data-into-postman)

## Features
* Manage library items: Add, update, and delete library items.
* Manage categories: Add, update, and delete categories.
* Manage employees: Add, update, and delete employees.
