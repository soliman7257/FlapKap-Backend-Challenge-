"# FlapKap-Backend-Challenge-" 
Features

- **REST API**: Implements CRUD operations for users and products, along with endpoints for depositing coins, making purchases, and resetting deposits.
- **Product Model**: Includes fields for `amountAvailable`, `cost`, `productName`, and `sellerId`.
- **User Model**: Includes fields for `username`, `password`, `deposit`, and `role`.
- **Deposit Coins**: Buyers can deposit 5, 10, 20, 50, and 100 cent coins into their vending machine account.
- **Make Purchases**: Buyers can purchase products with their deposited money, and receive change if applicable.
- **Reset Deposit**: Buyers can reset their deposit if needed.

- **Language/Framework**: C# .Net Core Api
- **Database**: SQl
- **Authentication/Authorization**: JWT tokens
## Setup

1. Dewonload the repository.
2. Install dependencies.
4. Set up your database and configure connection details.
    -migration (open Package Manager Console ) and run command 
     1-Add-Migration InitialCreate
     2-Update-Database


## API Endpoints

### Users
- **GET /users/:userId**: Get a specific user by ID .
- **POST /users**: Create a new user .
- **PUT /users/:userId**: Update a user's details .
- **DELETE /users/:userId**: Delete a user .

### Products
- **GET /products**: Get all products.
- **GET /products/:productId**: Get a specific product by ID.
- **POST /products**: Create a new product (authentication required).
- **PUT /products/:productId**: Update a product's details (authentication required).
- **DELETE /products/:productId**: Delete a product (authentication required).
### VendingMachineController
- **POST /deposit**: Deposit coins into the vending machine account (authentication required).
- **POST /buy**: Make a purchase using deposited money (authentication required).
- **POST /reset**: Reset the deposit amount for a user (authentication required).


