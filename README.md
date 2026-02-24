# Banking API
This project is an API meant to simulate a banking system.

## Functionality
This API allows for:
* Creation of an account
* Querying existing accounts with different parameters
* Fetching a specific account
* Making a deposit
* Making a withdraw
* Making a transfer between accounts
* Currency conversion for a specified account
* The acquisition of an authentication token

## Usage
This API can be interacted with through its endpoints. While launched in develper mode, these endpoint are exposed 
through a Swagger page with can be found at http://localhost:5079/swagger/index.html. This page contains documentation on each endpoint
and their parameters.
### Authentication
To use any of the endpoints which interact with account data, you first must acquired a JWT from the authentication
endpoint.

## Hosting
After cloning the project, the requirements to host this API on your machine are the following:
* An API key to the web address https://api.freecurrencyapi.com. 
This is how the conversion functionality is facilitated. The API key can be placed into the appsettings json as the 
`ApiWebAddress` variable.
* A SQL server to store the account data. Your connection string can be placed into the appsettings json as the 
`AccountContext` variable.

### Dependencies
This project uses the following packages which can all be obtained through the nuget package manager:
* `Microsoft.AspNetCore.Authentication.JwtBearer`
* `Microsoft.AspNetCore.OpenApi`
* `Microsoft.EntityFrameworkCore`
* `Microsoft.EntityFrameworkCore.SqlServer`
* `Microsoft.EntityFrameworkCore.Tools`
* `Microsoft.IdentityModel.Tokens`
* `Microsoft.VisualStudio.Web.CodeGeneration.Design`
* `Swashbuckle.AspNetCore`
* `System.IdentityModel.Tokens.Jwt`

