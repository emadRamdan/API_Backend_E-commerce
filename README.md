# WebApplicationProject
This is a simple ASP.NET Core server application that handles user authentication and authorization.
The application provides endpoints for user registration, login, 
managing categories and products,
managing shopping carts,
and processing orders.

Table of Contents
. Endpoints
    Client Registration
    Employee Registration
    User Login
    Category Management
    Product Management
    Shopping Cart Management
    Order Management
. Configuration
. Dependencies
. Setup


Endpoints: 
 . Client Registration
    Endpoint: POST /api/account/clients/signup

This endpoint allows clients to register. The request should include the following fields in a RegisterDto object:

    UserName (string)
    Email (string)
    Password (string)
    PhoneNumber (string)
    City (string)
    State (string)
    PostalCode (string)
    Country (string)
    UserImage (IFormFile) - Optional. Defaults to user.jpg if not provided.
Responses:

    201 Created on successful registration.
    400 Bad Request if the input data is invalid.
    500 Internal Server Error in case of server issues.

Employee Registration
Endpoint: POST /api/account/employees/signup

This endpoint allows employees to register. The request should include the following fields in a RegisterDto object:

    UserName (string)
    Email (string)
    Password (string)
    PhoneNumber (string)
    City (string)
    State (string)
    PostalCode (string)
    Country (string)
    UserImage (IFormFile) - Optional. Defaults to user.jpg if not provided.

Responses:

    201 Created on successful registration.
    400 Bad Request if the input data is invalid.
    500 Internal Server Error in case of server issues.

User Login
Endpoint: POST /api/account/login

This endpoint allows users to log in. The request should include the following fields in a LoginDto object:

    Email (string)
    Password (string)
Responses:

    200 OK with a TokenDto object if login is successful.
    400 Bad Request if the input data is invalid.
    401 Unauthorized if the credentials are incorrect.
    
Category Management
Endpoint: GET /api/account/AllCategories

This endpoint retrieves all categories.

Responses:

    200 OK with a list of CategoryDto objects.
Endpoint: POST /api/account/AddCategory
This endpoint allows employees to add a new category. The request should include the following fields in an AddCategoryDto object:

    CategoryName (string)
    CategoryImageFile (IFormFile)
Responses:

    201 Created on successful addition.
    400 Bad Request if the input data is invalid or if no image file is uploaded.
    500 Internal Server Error in case of server issues.
Product Management
Endpoint: GET /api/account/AllProducts

This endpoint retrieves all products. Optionally, products can be filtered by category ID or product name.

Parameters:

    CategoryId (int) - Optional
    ProductName (string) - Optional
Responses:

    200 OK with a list of ProductDto objects.
Endpoint: POST /api/account/AddProduct

This endpoint allows employees to add a new product. The request should include the following fields in an AddProductDto object:
    
    ProductName (string)
    ProductDescription (string)
    CategoryId (int)
    ProductImageFile (IFormFile)
Responses:

    201 Created on successful addition.
    400 Bad Request if the input data is invalid.
    500 Internal Server Error in case of server issues.
Shopping Cart Management
Endpoint: GET /api/cart/GetCart

This endpoint retrieves the current user's shopping cart.

Responses:

    200 OK with a ShoppingCartDto object.
    400 Bad Request if the cart ID is not found.
Endpoint: POST /api/cart/AddToCart

This endpoint allows clients to add a product to their shopping cart.

Parameters:
    
    ProductId (int)
Responses:

    200 OK on successful addition.
    400 Bad Request if the input data is invalid.
Endpoint: POST /api/cart/RemoveFromCart

This endpoint allows clients to remove a product from their shopping cart.

Parameters:

    CartId (int)
    ItemId (int)
Responses:

    200 OK on successful removal.
Endpoint: PUT /api/cart/EditItemQuantity

This endpoint allows clients to edit the quantity of an item in their shopping cart.

Parameters:

    CartId (int)
    ItemId (int)
    Quantity (int)
Responses:

    200 OK on successful edit.
Order Management
Endpoint: POST /api/orders/CreateOrder

This endpoint allows clients to create an order from their shopping cart.

Parameters:

    TotalPrice (decimal)
Responses:

    201 Created on successful order creation.
Endpoint: POST /api/orders/CancelOrder

This endpoint allows clients or employees to cancel an order.

Parameters:

    OrderId (int)
Responses:

    200 OK on successful order cancellation.
Endpoint: GET /api/orders/AllOrders

This endpoint retrieves all orders. Only employees can access this endpoint.

Responses:

    200 OK with a list of OrderDto objects.
Endpoint: GET /api/orders/UserAllOrders

This endpoint retrieves all orders of the current user.

Responses:

    200 OK with a list of OrderDto objects.
Configuration
The application requires the following configuration settings in appsettings.json:

    ConnectionStrings
    Jwt:Key
    Jwt:Issuer
    Jwt:Audience
Dependencies
The application relies on the following NuGet packages:
    
    Microsoft.AspNetCore.App
    Microsoft.AspNetCore.Identity
    Microsoft.EntityFrameworkCore
    Microsoft.Extensions.Configuration
    System.IdentityModel.Tokens.Jwt
Setup
Clone the repository:

Copy code
git clone https://github.com/emadRamdan/API_Backend_E-commerce
Navigate to the project directory:

Copy code
cd your-repo
Install the necessary packages:

Copy code
dotnet restore
Update the appsettings.json file with your configuration settings.
Update the USERSECRETS file with your configuration settings for connection string and tokensecret or use mine :

      {
        "ConnectionStrings": {
          "MyConnectionString": "Server=.;Database=ITI_ApiProject3;Trusted_Connection=True;Encrypt=false;"
        },
      
        "TokenSecret": "EmadTokenSecretKey15475Emad1547Ramdan"
      
      }

Run the application:

Copy code
dotnet run
The server will start on the configured port, and the endpoints will be available for use.
