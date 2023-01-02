# Frontend and Backend

## CORS
* Web API could block React application if don't configure that we want to accept a HTTP request from an origin(https://localhost:3000)
* The CORS policy is only for the application that run in a web browser.

## Entity Framework Core
* Create and interact with a real database.
* **DbContext**: It is a class that contains central configuration of Entity Framework for our application. 
    * Like what tables will be in our database.
    * Centain configurations fot those tables.
* In entity framework core we are going to use our entities to model the tables of our database.
* Commands
    * **Add-Migration Initial**
    * **Update-Database**: Run the migration.