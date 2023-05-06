# **Store APIs**
Store APIs is an application built on **_`.NET Core 7`_** and **_`Entity Framework`_**. 
It provides a set of APIs that allow developers to interact with a database 
that can be either **_`MSSQL`_** or **_`MYSQL`_**.
This **_README file_** serves as a guide for compiling and running the application, as well 
as changing database configurations and using the exposed endpoints.

The application features the following tables:
* **_Categories_**
* **_Orders_**
* **_Products_**
* **_Exlogs_**
* **_SystemSettings_**
* **_Users_**
* **_Roles_**
* **_UserRole_**

## Requirements
<p align="center">
    <a href="https://www.docker.com/" target="_blank" rel="noreferrer">
        <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/docker/docker-original-wordmark.svg" alt="docker" width="150" height="150"/>
    </a>
    <a href="https://learn.microsoft.com/en-us/dotnet/" target="_blank" rel="noreferrer">
        <img src="https://www.vectorlogo.zone/logos/dotnet/dotnet-vertical.svg" alt="docker" width="150" height="150"/>
    </a>
    <a href="https://dev.mysql.com/doc/" target="_blank" rel="noreferrer">
        <img src="https://www.vectorlogo.zone/logos/mysql/mysql-official.svg" alt="docker" width="150" height="150"/>
    </a>
    <a href="https://docs.docker.com/compose/" target="_blank" rel="noreferrer">
        <img src="https://raw.githubusercontent.com/docker/compose/v2/logo.png" alt="docker" width="150" height="150"/>
    </a>
</p>

* **_.NET Core 7_**
* **_Entity Framework_**
* **_MSSQL or MySQL_**
* **_Docker_**
* **_Docker Compose_**
* **_Packages:_**
  1. Microsoft.AspNetCore.Identity.EntityFrameworkCore
  2. Microsoft.EntityFrameworkCore.SqlServer
  3. Pomelo.EntityFrameworkCore.MySql
  4. Microsoft.Extensions.DependencyInjection
  5. Microsoft.AspNetCore.Authentication.JwtBearer
  6. Swashbuckle.AspNetCore

## Installation
1. Clone the repository or download the code.
2. Open the project in Visual Studio or any other preferred IDE.
3. Configure the connection string to the desired database server and database.
4. Run the database migrations to create the necessary tables and relationships.
5. Build the application:
```bash
$ docker build -t store-apis .
```

## Usage
1. Open a terminal and navigate to the root directory of your application.
2. Run the following command:
```bash
$ docker-compose up --build
```
This will start the application and a MySQL container, which the application will use as its database

3. To access the Swagger documentation, go to `http://localhost:5000/swagger/index.html`
4. The system comes with a predefined user with the following credentials:
```bash
UserName: Superuser
Password: Su_per#User72
```
This user has all privileges in the system.
5. Once the application has started, you can access its API endpoints at `http://localhost:5000/api` 

The following endpoints are available:
### **_/Auth_**
This system has **_three roles_**: Admin, User, 
and Customer. Each role has its own set of permissions and access levels.

 |HTTP Verb|Endpoint URL|Action|Role|
 |---------|------------|------|----|
 |POST|**_`/Login`_**|Login with an existing user account|Anonymous|
 |POST|**_`/SignUp`_**|Register a new user account|Anonymous|
 |GET|**_`/Role?page={page}&size={size}`_**|Retrieve a list of all roles|Admin & User|
 |GET|**_`/Role/GetById?id={id}`_**|Retrieve a specific role by its ID|Admin & User|
 |GET|**_`/Role/GetByUserId?userId={userId}`_**|Retrieve a user roles by its ID|Admin & User|
 |PUT|**_`/Role?id={id}&roleNewName={roleNewName}`_**| Update an existing role|Admin|
 |POST|**_`/Role/AddUserRole`_**|Add a role to a user|Admin|
 |DELETE|**_`/Role/RemoveRoleFromUser`_**| Remove a role from a user|Admin|

 **_Payloads:_**
 
 1. **_`/Login`_**:  
 ```json
{
   "UserName": "Hazim",
   "Password": "P@ssw0rd"
}
 ```
 2. **_`/SignUp`_**
  ```json
{
    "Name": "Hazim",
    "UserName": "Hazim",
    "Password": "P@ssw0rd",
    "Email": "test@yahoo.com",
    "PhoneNumber": "0788888888",
    "Latitude": "38.8951",
    "Longitude": "-77.0364"
}
 ```
  3. **_`/AddUserRole`_** & **_`/RemoveRoleFromUser`_**
  ```json
{
    "UserName": "Hazim",
    "RoleName": "Admin"
}
 ```
 ---------------------------------------
 ### **_/Category_**
To authorize requests to protected endpoints, you need to 
include an **_`Authorization`_** header in your request with the value of **_`Bearer {JWT}`_**. 
The **_`{JWT}`_** placeholder should be replaced with the token obtained from the login
endpoint.

|HTTP Verb|Endpoint URL|Action|Role|
|---------|------------|------|----|
|GET |**_`/Category?page={page}&size={size}`_**|Retrieve a list of all categories|Admin & User & Customer|
|GET |**_`/Category/GetById?id={id}`_**|Retrieve a specific category by its ID	|Admin & User & Customer|
|POST |**_`/Category`_**|Add a Category|Admin & User|
|PUT |**_`/Category?id={id}`_**|Update an existing category|Admin & User|
|DELETE |**_`/Category?id={id}`_**| Remove an existing category|Admin & User|

 **_Payloads:_**
 
 1.  **_`/AddCategory`_** & **_`/UpdateCategory?id={id}`_**:  
 ```json
{
    "CategoryName": "test",
    "IsActive": true
}
 ```
---------------------------------------
 ### **_/Product_**
To authorize requests to protected endpoints, you need to 
include an `Authorization` header in your request with the value of `Bearer {JWT}`. 
The `{JWT}` placeholder should be replaced with the token obtained from the login
endpoint.

|HTTP Verb|Endpoint URL|Action|Role|
|---------|------------|------|----|
|GET |**_`/Product?page={page}&size={size}`_**|Retrieve a list of all Products|Admin & User & Customer|
|GET |**_`/Product/GetById?id={id}`_**|Retrieve a specific Product by its ID	|Admin & User & Customer|
|POST |**_`/Product`_**|Add a Product|Admin & User|
|PUT |**_`/Product?id={id}`_**|Update an existing Product|Admin & User|
|DELETE |**_`/Product?id={id}`_**| Remove an existing Product|Admin & User|

 **_Payloads:_**
 
 1.  **_`/AddProduct`_** & **_`/UpdateProduct?id={id}`_**:  
 ```json
{
    "ProductName": "string",
    "ProductBarCode": "string",
    "ProductPrice": 0,
    "ProductImageName": "string",
    "OldPrice": 0,
    "IsOffered": true,
    "PercentageDiscount": 0,
    "IsActive": true,
    "CategoryId": 0
}
 ```
---------------------------------------

 ### **_/Order_**
To authorize requests to protected endpoints, you need to 
include an `Authorization` header in your request with the value of `Bearer {JWT}`. 
The `{JWT}` placeholder should be replaced with the token obtained from the login
endpoint.

|HTTP Verb|Endpoint URL|Action|Role|
|---------|------------|------|----|
|GET |`/Order?userId={userId}`|Retrieve a list of all orders|Admin & User & Customer|
|GET |`/Order/GetById?guid={guid}`|Retrieve a specific Product by its GUID|Admin & User & Customer|
|POST |`/Order`|Add an Order|Admin & User|

 **_Payloads:_**
 1.  **_`/AddOrder`_**:  
 ```json
{
    "UserId": "",
    "NewLatitude":"",
    "NewLongitude":"",
    "Products": [
        {
            "ProductId": 1,
            "ProductPrice": 20.5
        }
    ]
}
 ```
---------------------------------------
 ### **_/SystemSetting_**
To authorize requests to protected endpoints, you need to 
include an `Authorization` header in your request with the value of `Bearer {JWT}`. 
The `{JWT}` placeholder should be replaced with the token obtained from the login
endpoint.

|HTTP Verb|Endpoint URL|Action|Role|
|---------|------------|------|----|
|GET |**_`/SystemSetting?page={page}&size={size}`_**|Retrieve a list of all SystemSettings|Admin & User|
|GET |**_`/SystemSetting?id={id}`_**|Retrieve a specific GetSystemSetting by its ID	|Admin & User|
|POST |**_`/SystemSetting`_**|Add a SystemSetting|Admin & User|
|PUT |**_`/SystemSetting?id={id}`_**|Update an existing SystemSetting|Admin & User|
|DELETE |**_`/SystemSetting?id={id}`_**| Remove an existing category|Admin|

 **_Payloads:_**
 1.  **_`/AddSystemSetting`_** & **_`/UpdateSystemSetting?id={id}`_**:  
 ```json
{
    "Key": "",
    "Value":""
}
 ```
---------------------------------------
 ### **_/Exlog_**
To authorize requests to protected endpoints, you need to 
include an `Authorization` header in your request with the value of `Bearer {JWT}`. 
The `{JWT}` placeholder should be replaced with the token obtained from the login
endpoint.

|HTTP Verb|Endpoint URL|Action|Role|
|---------|------------|------|----|
|GET |`/Exlog?page={page}&size={size}`|Get all categories|Admin & User & Customer|
|GET |`/Exlog/GetById?id={id}`|Register a new user account|Admin & User & Customer|