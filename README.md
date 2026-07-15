
# Projectname

Self Service Hub

## LIVE-DEMO on Azure

<https://selfservicehub-cacgged0fagpd5hb.westeurope-01.azurewebsites.net/>

## Features

- Server-Side Rendered (SSR) Self Service Portal
- Registration/Login
- Token-Based Email Verification
- Password Reset
- Cookie-Authentification
- Role- and Claim-Based Access Control

Further planned features:
- Management of Agents and Customers (as part of the Tenant)
- Ticketing System
- Reporting

## Technologies

- .NET 9 Sdk
- ASP.NET Core Razor Pager
- Entity Framework Core
- ASP.NET Core Identity
- MySql Database
- Azure Deployment

## How to run the application on your local machine

### 1. Installation

```bash
git clone https://github.com/msass89/Self-Service-Portal.git
cd SelfServiceHub
```

### 2. Restore packages:

```bash
dotnet restore
```

### 3. Configuration

`appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "Database": {
    "ServerVersion": "..."
  }
}
```

Please enter the Version of your MySql Server and provide the connection string.
It's recommended to set up a user secret or an environment variable for the connection string instead of writing it in plain text into the code.

### 4. Run database migrations

```bash
dotnet ef database update
```

### 5. Start Application

```bash
dotnet run
```

## Authentification and Authorization

You can register as a new user by clicking on 'Register' on the upper right corner of the UI. 
You can use a fake email address to register, as for demonstration purposes the link for the email verification is sent directly to the UI.
After registered succesfully as a new user, you can login with your password.

## Projectstructure

```
Areas\Identity\Pages/
    Account/
Data/
  Seed/
Migrations/
Models/
  DTO/
  Entities/
  ViewModels/
Pages/
    Shared/

Program.cs

```

## Licence

Copyright (c) 2026 Maike Saß - MIT Licence