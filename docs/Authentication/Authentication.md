# E-commerce Management API

## Authentication

The Authentication feature enables users to register as customers and provides a secure mechanism for general users to authenticate themselves.
Upon successful authentication, a JSON Web Token (JWT) is generated, allowing users to securely access protected endpoints throughout the system.

## Register Customer

Allows a new customer user to register for an account.

```js
POST "/auth/register/users/customers"
```

### Headers

- `Content-Type: application/json`

### Request Format

Field Specifications:

- `name` – A string with at least 3 characters. Cannot be empty.
- `email` – A unique and valid email address. Must have a valid format.
- `password` – A string of at least 6 characters, containing at least one digit and one letter.

Example Request:

```json
{
  "name": "Victor Figueiredo Mendes",
  "email": "victor7@email.com",
  "password": "victor123!"
}
```

### Response Format

- 201 CREATED: Customer successfully registered.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 409 CONFLICT: The provided email is already in use.

Example Response:

```json
{
  "id": "1",
  "name": "Victor Figueiredo Mendes",
  "email": "victor7@email.com",
  "token": "eyJhbGciOiJIUzI1N...adQssw5c"
}
```

## Login User

Allows a user to log in using their email and password.<br/>
PS: Inactive users cannot log in.

```js
POST "/auth/login/users"
```

### Headers

- `Content-Type: application/json`

### Request Format

Field Specifications:

- `email` – A non-empty string representing the user's email.
- `password` – A non-empty string representing the user's password.

Example Request:

```json
{
  "email": "victor7@email.com",
  "password": "victor123!"
}
```

### Response Format

- 200 OK: User successfully authenticated.
- 400 BAD_REQUEST: The email or password is incorrect.

Example Response:

```json
{
  "id": "1",
  "name": "Victor Figueiredo Mendes",
  "email": "victor7@email.com",
  "token": "eyJhbGciOiJIUzI1N...adQssw5c"
}
```

## Login Carrier

Allows a carrier to log in using their email and password.

```js
POST "/auth/login/carriers"
```

### Headers

- `Content-Type: application/json`

### Request Format

Field Specifications:

- `email` – A non-empty string representing the carrier's email.
- `password` – A non-empty string representing the carrier's password.

Example Request:

```json
{
  "email": "carrier@email.com",
  "password": "carrier123"
}
```

### Response Format

- 200 OK: Carrier successfully authenticated.
- 400 BAD_REQUEST: The email or password is incorrect.

Example Response:

```json
{
  "id": "1",
  "name": "ECommerceManagementCarrier",
  "email": "carrier@email.com",
  "token": "eyJhbGciOiJIUzI1N...adQssw5c"
}
```
