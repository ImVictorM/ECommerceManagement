# E-commerce Management API

## Authentication

### Register

This endpoint allows a new user to register for an account. Registration requires a valid name, email, and password.

```js
POST {{base_endpoint}}/register
```

#### Headers

- `Content-Type: application/json`

#### Request Format

```json
{
  "name": "Victor Figueiredo Mendes",
  "email": "victor7@email.com",
  "password": "victor123!"
}
```

#### Response Format

201 CREATED

```json
{
  "id": 1,
  "name": "Victor Figueiredo Mendes",
  "email": "victor7@email.com",
  "token": "eyJhbGciOiJIUzI1N...adQssw5c"
}
```

### Login

Users can log in using their email and password. Note: Inactive users will receive a `400 Bad Request` response.

```js
POST {{base_endpoint}}/login
```

#### Headers

- `Content-Type: application/json`

#### Request Format

```json
{
  "email": "victor7@email.com",
  "password": "victor123!"
}
```

#### Response Format

200 OK

```json
{
  "id": 1,
  "name": "Victor Figueiredo Mendes",
  "email": "victor7@email.com",
  "token": "eyJhbGciOiJIUzI1N...adQssw5c"
}
```
