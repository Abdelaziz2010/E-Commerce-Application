
# 🛒 Ecom - Ecommerce Backend API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2019+-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![Redis](https://img.shields.io/badge/Redis-%23DC382D.svg?logo=redis&logoColor=white)](https://redis.io)
[![Stripe](https://img.shields.io/badge/Stripe-626CD9?logo=stripe&logoColor=white)](https://stripe.com)

---

# Project Mind Map:

![Image](https://github.com/user-attachments/assets/f1e8539b-fb42-47a1-a9d0-000f4a878f39)

## Glimpse of the working solution:


![Image](https://github.com/user-attachments/assets/089be866-6d81-428a-91d6-2273480d18a2)
![Image](https://github.com/user-attachments/assets/19d1b91c-91df-4fc8-9cf8-d77045bb5c32)
![Image](https://github.com/user-attachments/assets/8b5ab360-218b-44ba-9d9a-5b3da061ab01)
![Image](https://github.com/user-attachments/assets/63d3c14d-e1a3-4619-a69e-aa0a629dd1fa)
![Image](https://github.com/user-attachments/assets/1785b1be-a53f-46a6-8cdf-f75fa68d3b98)
![Image](https://github.com/user-attachments/assets/84c4f266-ca90-493c-9433-62d28c8e0357)

---


## Project Overview:

**Objective:** 

A comprehensive backend API for ecommerce system, featuring secure payment processing, product catalog management, cart management, review functionality, checkout and placing order process. Built with RESTful principles and JWT authentication.


---


## 🚀 Key Features:

### 🛍️ Core Functionality

- **Product Catalog Management** with categories
- **Advanced Product Search** with sorting, filtering, and pagination
- **Shopping Cart System** with CRUD operations
- **Order Management** with order history tracking
- **Product Reviews System** (post-purchase)
- **User Account Management** with authentication/authorization


### ⚡ Performance & Security

- **JWT Authentication** with Identity Framework
- **Redis Caching** for improved performance
- **Rate Limiting** (8 requests/30 sec per IP)
- **Security Headers** against XSS/CSRF attacks
- **Global Exception Handling** with standardized error responses


### 💳 Payment Processing

- **Stripe Integration** for secure payments
- **Webhook Support** for payment status updates
- **Payment Intent Management**

 
### 🌟 Additional Features

-**💡 Clean Architecture**: I have used clean arch, dividing the project into 4 layers Presentation, Infrastructure, Application, Domain. clean arch ensures maintainability, scalability, testability, separation of concerns and loose coupling between components.

-**🚦 CORS (Cross-Origin Resource Sharing)**: a security feature implemented by web browsers to prevent web pages from making requests to a different domain than the one that served the web page.

-**🔄 Automapper**: Utilized for efficient object mapping between models, improving data handling and reducing boilerplate code.

-**✅ Fluent Validation**: Ensured data integrity by effectively validating inputs, leading to user-friendly error messages.

-**🔑 Account Management**: Implemented features for user account management, including verify email, change password and reset password functionalities.

-**🚦 Rate Limiting**: Controlled the number of requests to prevent abuse, ensuring fair usage across all users.

-**🗃️ Redis Caching**: I have used redis as a temporary container to cache the shopping cart and shopping cart items to optimized performance and improving response times.

-**📧 Email Confirmation**: Managed user email confirmations, password changes, and resets seamlessly to enhance security.

-**🚨 Global Exception Handling**: Integrated centralized exception handling to manage errors gracefully, significantly enhancing the user experience.

-**🛠️ Swagger**: For documenting the API Endpoints

-**🚦 Dependency Injection**: For better code organization.


---


## 🛠️ Technology Stack:

| Category          | Technologies/Libraries                |
|-------------------|---------------------------------------|
| **Backend**       | .Net 8 & ASP.NET Core Web API         |
| **ORM**           | Entity Framework Core                 |
| **Database**      | SQL Server                            |
| **Caching**       | Redis                                 |
| **Authentication**| JWT, ASP.NET Core Identity            |
| **Payment**       | Stripe.NET                            |
| **Mapping**       | AutoMapper                            |
| **Validation**    | FluentValidation                      |
| **Dev Tools**     | Swagger/OpenAPI - Postman - Git - VS  |
  

---


## 📚 API Documentation:

### 👤 Account Controller - Authentication & User Management
| Method | Endpoint                                  | Description                          |
|--------|-------------------------------------------|--------------------------------------|
| POST   | `/api/Account/Register`                   | Register new user account            |
| POST   | `/api/Account/Login`                      | Authenticate user and return JWT     |
| GET    | `/api/Account/Logout`                     | Terminate current session            |
| GET    | `/api/Account/Get-User-Info`              | Retrieve authenticated user details  |
| GET    | `/api/Account/Get-User-Name`              | Get current user's display name      |
| POST   | `/api/Account/Activate-Account`           | Activate registered account          |
| POST   | `/api/Account/Send-Forgot-Password-Email` | Initiate password reset process      |
| POST   | `/api/Account/Reset-Password`             | Complete password reset              |
| GET    | `/api/Account/Get-User-Address`           | Retrieve user's saved address        |
| PUT    | `/api/Account/Update-Or-Create-Address`   | Create/update user address           |
| GET    | `/api/Account/Is-User-Authenticated`      | Check authentication status          |
| DELETE | `/api/Account/Delete-User`                | Delete user account                  |


### 🐞 Bug Controller (`/api/Bug`) - *Testing Endpoints*
| Method | Endpoint                          | Description                          |
|--------|-----------------------------------|--------------------------------------|
| GET    | `/api/Bug/Not-Found`              | Simulate 404 Not Found response      |
| GET    | `/api/Bug/Server-Error`           | Simulate 500 Server Error            |
| GET    | `/api/Bug/Bad-Request/{id}`       | Simulate 400 Bad Request (with ID)   |
| GET    | `/api/Bug/Bad-Request`            | Simulate generic 400 Bad Request     |


### 🛒 Cart Controller (`/api/Carts`)
| Method | Endpoint                          | Description                          |
|--------|-----------------------------------|--------------------------------------|
| GET    | `/api/Carts/Get-By-Id/{id}`       | Retrieve cart by ID                  |
| POST   | `/api/Carts/Update-Or-Create`     | Create or update shopping cart       |
| DELETE | `/api/Carts/Delete/{id}`          | Delete cart by ID                    |


### 📦 Categories Controller (`/api/Categories`)
| Method | Endpoint                               | Description                          |
|--------|----------------------------------------|--------------------------------------|
| GET    | `/api/Categories/Get-All`              | Get all product categories           |
| GET    | `/api/Categories/Get-By-Id/{id}`       | Get category by ID                   |
| POST   | `/api/Categories/Add-Category`         | Create new category                  |
| PUT    | `/api/Categories/Update-Category`      | Update existing category             |
| DELETE | `/api/Categories/Delete-Category/{id}` | Delete category by ID                |


### 🚨 Error Controller (`/errors`) - for requests that access nonexisting endpoints
| Method | Endpoint                          | Description                          |
|--------|-----------------------------------|--------------------------------------|
| GET    | `/errors/{statuscode}`            | Retrieve error details by status code|


### 📮 Order Controller (`/api/Orders`)
| Method | Endpoint                              | Description                          |
|--------|---------------------------------------|--------------------------------------|
| POST   | `/api/Orders/Create-Order`            | Create new order from cart           |
| GET    | `/api/Orders/Get-All-Orders-For-User` | Get user's order history             |
| GET    | `/api/Orders/Get-Order-By-Id/{id}`    | Get order details by ID              |
| GET    | `/api/Orders/Get-Delivery-Methods`    | List available delivery methods      |


### 💳 Payment Controller (`/api/Payments`)
| Method | Endpoint                                 | Description                                             |
|--------|------------------------------------------|---------------------------------------------------------|
| POST   | `/api/Payments/Create-Or-Update-Payment` | Initialize payment intent process                       |
| POST   | `/api/Payments/WebHook`                  | Handle Stripe payment webhooks & Listen to stripe events|


### 🛍️ Product Controller (`/api/Products`)
| Method | Endpoint                            | Description                                 |
|--------|-------------------------------------|---------------------------------------------|
| GET    | `/api/Products/Get-All`             | Get all products (paginated,filtered,sorted)|
| GET    | `/api/Products/Get-By-Id/{id}`      | Get product details by ID                   |
| POST   | `/api/Products/Add-Product`         | Create new product entry                    |
| PUT    | `/api/Products/Update-Product`      | Update existing product                     |
| DELETE | `/api/Products/Delete-Product/{id}` | Remove product by ID                        |


### ⭐ Review Controller (`/api/Reviews`)
| Method | Endpoint                                               | Description                          |
|--------|--------------------------------------------------------|--------------------------------------|
| GET    | `/api/Reviews/Get-All-Reviews-For-Product/{productId}` | Get product reviews                  |
| POST   | `/api/Reviews/Add-Review`                              | Submit product review (post-purchase)|
| DELETE | `/api/Reviews/Delete-Review/{reviewId}`                | Remove review by ID                  |


---


## Development Focus:

### 1. [Genaric Repository Pattern](#repository-pattern)
- **Description:** Implement the Repository Pattern to abstract data access logic, making the code more testable and maintainable. 
- **Functionality:**
  - **Genaric Repository Pattern:** Simplifies data access by providing a consistent API for CRUD operations.
  - **Unit of Work:** Manages transactions across multiple repositories, ensuring data integrity.


### 2. [Entity Framework Core](#entity-framework-core)
- **Description:** Handle database interactions using Entity Framework Core, allowing for seamless integration with the database. The use of code-first migrations ensures that the database schema is in sync with the application models.
- **Features:**
  - **Code-First Migrations:** Automatically generate database schema from your code.
  - **Entity Mapping:** Ensure proper mapping of domain entities to database tables.

		
### 3. [Auth Section](#auth-section)
- **Login:** Secure user authentication.
- **Activate Account** Sent Email verification to user to avoid fake emails.
- **Reset Password:** Provide password recovery options.
	

---


## Under Construction:
-**Role-Based Authorization**: Creating role for "Merchants", role for "Admin" and role for "NormalUser".

-**Enabling Refresh Tokens**: To automatically generate access token when it is expired without asking the user to login again, for enhancing user experience.

-**Unit-Testing**: Using XUnit to test the project to enhance the security.

---

## 📝 Request/Response Examples: 
*Sample Product Creation Request:*

POST /api/Products/Add-Product

Authorization: Bearer <your-jwt-token>

Content-Type: application/json
```json
{
  "name": "Premium Wireless Headphones",
  "description": "Noise-cancelling Bluetooth headphones",
  "Newprice": 299.99,
  "OldPrice": 350.34,
  "categoryId": 5,
  "Photos":[]
}
```

	
Response:



```json
{
  "statusCode": 200,
  "message": "Product created successfully"
}
```

---

## 🚀 Getting Started
1. Clone the repository
2. Configure database connection in `appsettings.json`
3. Run migrations: `dotnet ef database update`
4. Start the API: `dotnet run`



---
## Links
- **Project Repository: https://github.com/Abdelaziz2010/E-Commerce-Application**

- **Frontend demo (Vibe Coding using Lovable ai): https://commerce-forge-front.lovable.app/**