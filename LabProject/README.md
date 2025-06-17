# Sales Analytics Dashboard

This project is a .NET MVC application that provides analytics for customer sales data. It showcases various metrics including the highest spending customer, a list of all customers, and the top 5 most frequently purchased items.

## Features

1. **Highest Spending Customer**
   - Displays the customer with the highest total spending across all orders
   - Shows customer details including name, email, and country of residence

2. **Customer List**
   - Displays all customers in a responsive table
   - Shows basic customer information

3. **Top 5 Most Frequently Purchased Items**
   - Visualizes the top 5 items in a pie chart using Chart.js
   - Based on the total quantity of items sold across all orders

## Technical Implementation

### Architecture
- Follows Clean Architecture principles
- Implements SOLID principles
- Uses Repository pattern for data access
- Implements service layer for business logic

### Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- Chart.js for visualization
- xUnit for testing
- SQL Server for data storage

### Design Patterns
- Dependency Injection
- Repository Pattern
- Unit of Work Pattern

## Assumptions

1. **Data Model**
   - Customer can have multiple orders
   - Each order can have multiple order details
   - SKU is unique for each product
   - Prices are stored in double format for simplicity

2. **Business Rules**
   - Highest spending customer is calculated based on the sum of (quantity * unit price) for all items in all orders
   - Top items are ranked by total quantity sold across all orders
   - All monetary values are assumed to be in the same currency

3. **Testing**
   - Used in-memory database for testing
   - Focused on testing business logic in the service layer
   - Assumed data consistency and referential integrity

## Setup Instructions

1. Clone the repository
2. Update the connection strings in `appsettings.json`
3. Run database migrations:
   ```
   dotnet ef database update
   ```
4. Run the application:
   ```
   dotnet run
   ```
