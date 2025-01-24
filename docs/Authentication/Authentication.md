# E-commerce Management API

## Authentication

The Authentication feature enables users to register as customers and provides a secure mechanism for general users to authenticate themselves.
Upon successful authentication, a JSON Web Token (JWT) is generated, allowing users to securely access protected endpoints throughout the system.

### Register

Allows a new user to register for an account as a customer.

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

- 201 CREATED: the request was successfully processed and a new customer was created.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 409 CONFLICT: The user email is already in use.

```json
{
  "id": "1",
  "name": "Victor Figueiredo Mendes",
  "email": "victor7@email.com",
  "token": "eyJhbGciOiJIUzI1N...adQssw5c"
}
```

### Login

Users can log in using their email and password.<br/>
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

- 200 OK: The user email an password were correct and the user was authenticated.

```json
{
  "id": "1",
  "name": "Victor Figueiredo Mendes",
  "email": "victor7@email.com",
  "token": "eyJhbGciOiJIUzI1N...adQssw5c"
}
```

- 400 BAD_REQUEST: The email or password is incorrect.
