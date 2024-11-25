# E-commerce Management API

## Authentication

### Register

This endpoint allows a new user to register for an account.

```js
POST "/auth/register"
```

#### Headers

- `Content-Type: application/json`

#### Request Format

Field Rules:

- name - must be at least 3 characters long
- email - must be unique and have a valid format
- password - Length must be greater or equal to 6 and contain at least one digit and one character

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

Users can log in using their email and password.
PS: Inactive users cannot log in.

```js
POST "/auth/login"
```

#### Headers

- `Content-Type: application/json`

#### Request Format

Field Rules:

- email - must not be empty
- password - must not be empty

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
