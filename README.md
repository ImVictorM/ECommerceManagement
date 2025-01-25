# E-commerce Management API

## Overview 💡

The E-commerce Management API is a study project designed to apply and deepen knowledge of software architecture principles and patterns. This project focuses on implementing core functionalities of real-world e-commerce systems, including product management, user management, order processing, and payment handling.

Key principles and patterns applied in the project include:

- Domain-Driven Design (DDD)
- Clean Architecture
- CQRS (Command Query Responsibility Segregation)
- SOLID Principles

Although this is a study project, significant effort has been dedicated to ensuring the system is as close as possible to a production-ready e-commerce application.

### Payment Integration

The project includes an abstraction layer that emulates the behavior of real-world payment gateways. While no actual payment gateways are integrated and no real payments or charges are created, the abstraction mimics how payment processing works, including webhooks and transaction handling.

### Features

[Click here](./docs/) to learn more about the API features and to see additional documentation.

## Main technologies used 🧰

<img
  alt="Static Badge"
  src="https://img.shields.io/badge/csharp-purple?style=for-the-badge&logo=csharp"
  style="margin-bottom: 4px;"
  height="30px"
/>
<img
  alt="Static Badge"
  src="https://img.shields.io/badge/dotnet-purple?style=for-the-badge&logo=dotnet"
  style="margin-bottom: 4px;"
  height="30px"
/>
<img
  alt="Static Badge"
  src="https://img.shields.io/badge/Entity%20Framework%20Core-%20purple?style=for-the-badge&logo=amazonredshift&logoColor=white"
  style="margin-bottom: 4px;"
  height="30px"
/> <br/>
<img
  alt="Static Badge"
  src="https://img.shields.io/badge/xUnit-black?style=for-the-badge&logoColor=white"
  style="margin-bottom: 4px;"
  height="30px"
/>
<img
  alt="Static Badge"
  src="https://img.shields.io/badge/MediatR-blue?style=for-the-badge"
  style="margin-bottom: 4px;"
  height="30px"
/>
<img
  alt="Static Badge"
  src="https://img.shields.io/badge/PostgreSQL-%234169E1?style=for-the-badge&logo=postgresql&logoColor=white" style="margin-bottom: 4px;"
  height="30px"
/>

## Running the application ⚙️

<details>
<summary><h4>Running with docker 🐋 (recommended)</h4></summary>

> You must have docker installed

1. Clone and enter the repository

```sh
git clone git@github.com:ImVictorM/ECommerceManagement.git && cd ECommerceManagement
```

2. Create a `.env` file at the root to configure environment variables for Docker. Use the `.env-example` file as a reference.

3. Build and run the containers

```sh
docker-compose up -d --build
```

4. Run the migrations

```sh
dotnet ef database update -p .\src\Infrastructure -s .\src\WebApi --connection "Host=localhost;Port=8010;Username=postgres;Password=postgres;Database=ecommerce-management;Trust Server Certificate=true;"
```

</details>

<details>
<summary><h4>Running locally 🖥️<h4></summary>

> You must have dotnet and postgres installed

1. Clone and enter the repository

```sh
git clone git@github.com:ImVictorM/ECommerceManagement.git && cd ECommerceManagement
```

2. Configure the file `src/WebApi/appsettings.json`. Update the `DbConnectionSettings` section with your database credentials as follows:

```json
{
  "DbConnectionSettings": {
    "Host": "localhost",
    "Port": "5432",
    "Database": "ecommerce-management",
    "Username": "your-username",
    "Password": "your-password"
  }
}
```

3. Restore the dependencies

```sh
dotnet restore ECommerceManagement.sln
```

4. Run the migrations

```sh
dotnet ef database update -p .\src\Infrastructure -s .\src\WebApi --connection "Host=localhost;Port=8010;Username=postgres;Password=postgres;Database=ecommerce-management;Trust Server Certificate=true;"
```

5. Run the WebApi project

```sh
dotnet run --project ./src/WebApi
```

</details>

## Testing 🛠️

Note: The integration tests use [TestContainer](https://testcontainers.com/) and [Respawn](https://github.com/jbogard/Respawn) to manage test databases dynamically using Docker containers. Ensure Docker is running on your system to execute integration tests successfully.

Command to run all tests:

```
dotnet test ECommerceManagement.sln
```

Command to run the unit tests:

> Replace `{layer_name}` with the specific layer you want to test, such as `Domain`, `Application`, or `SharedKernel`.

```
dotnet test ./tests/UnitTests/{layer_name}.UnitTests
```

Command to run the integration tests:

```
dotnet test ./tests/IntegrationTests
```

## References 📌

I separated this section to share some cool (and free) stuff I found when studying and creating this project.

[Amichai Mantiband Playlist on Clean Architecture + DDD](https://www.youtube.com/playlist?list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k) <br/>
[Specifications by Eric Evans and Martin Fowler](https://martinfowler.com/apsupp/spec.pdf)<br/>
[Polymorphism in Entity Framework](https://www.learnentityframeworkcore.com/inheritance)<br/>
[CQRS and MediatR in ASP.NET Core](https://codewithmukesh.com/blog/cqrs-and-mediatr-in-aspnet-core/)<br/>
[How Payment is Done using the Mercado Pago Service](https://www.mercadopago.com.br/developers/pt/docs/checkout-api/landing)<br/>
[Webhooks and Security](https://stytch.com/blog/webhooks-security-best-practices/)
