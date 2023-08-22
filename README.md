# Product Repair - Web Portal

This ASP.Net Web Application is a proof of concept for an online product repair management portal, developed for B2B scenarios. Currently, this application is the 'front-end', and provides functionality for the initial intake process, and ongoing case management. It enables the user to:
  - Create and view repair cases
  - Add items and their respective issues to a case
  - Submit a case for approval
  
The technology stack includes:
  - ASP.Net MVC and Razor pages for UI
  - Dapper for ORM
  - Identity for Authentication and Authorization
  - SQL Server for storing user information and database managment.

![image](https://github.com/MikeNolan678/Product-Repair/assets/50291390/65401ced-e424-4e27-acce-2f1129e4fba6)


## Demo Environment
Feel free to explore the demo environment, which is hosted on Azure using Azure App Services and SQL Server - https://productrepairdealer.azurewebsites.net/

You can log in using the demo account, which is already connected to a mock account.
    
**User:** test@test.com

**Password:** Test123!

## Application Structure
The solution is divided into three key parts:

1. **UI Layer:** Built with ASP.NET MVC and Razor Pages. It contains all the Controllers, Views and related logic.

    - The pages are responsive and mobile-friendly, utilising Bootsrap 5.
    - The UI allows the user to create, view and edit repair cases.
    - Controller classes allow smooth navigation throughout the application.

2. **Data Access Layer:** This contains all data access and related business logic, including CRUD actions to the database layer. It utilises Dapper as an ORM for interacting with the database.

    - Various models are used to perform relevant logic, and support the ASP.Net MVC UI structure.
    - Case data is retrieved and processed, before being returned to the relevant UI Controller in ASP.Net Core MVC.
    - Various data is passed back and forth to SQL Server, including all CRUD actions.
    - Helper classes support the use of an MVC architecture.
      
4. **Database:** This includes various normalised tables supporting the Case > Items > Issues struture.
   

## Technology Stack

- ASP.NET Core MVC
- Razor Pages
- Entity Framework Core
- Dapper ORM
- SQL Server
- HTML, CSS, Javascript, Bootstrap 5

## Creating a Case
1. After logging in, using the test account, you can 'Create New Case' from the home page.

![image](https://github.com/MikeNolan678/Product-Repair/assets/50291390/7f88aa91-26e6-4d49-9ef4-4bd3f441f284)

2. Add items to the case, and add the relevant issues to each item. 

![image](https://github.com/MikeNolan678/Product-Repair/assets/50291390/1176289d-1a5f-4464-b623-ffc2d9dda05a)

3. Once complete, you can Submit the Case for review.

It's also posisble to 'Save Draft', and revisit the Case later by viewing the 'Draft Cases'. Additional information can be added before the Case is submitted.

![image](https://github.com/MikeNolan678/Product-Repair/assets/50291390/a2a11f9d-72c6-488a-a191-af4a9404d02e)

   
<hr />
