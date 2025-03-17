# E-commerce Management API

## Users

The Users feature provides functionality for managing user accounts within the e-commerce system. It allows users to retrieve and update their own details, as well as deactivate their accounts. Administrators can manage all user accounts, including retrieving user details, updating information, and deactivating non-administrator accounts. The feature supports filtering users by their active status and ensures secure authentication and role-based permissions, restricting sensitive actions to authorized users.

## Get User by Authentication Token

Retrieves the currently authenticated user's details. Requires authentication.

```js
GET "users/self"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 200 OK: The request was successfully processed and the current user details were returned.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 404 NOT_FOUND: The current user could not be found.

Example Response:

```json
{
  "id": "1",
  "name": "admin",
  "email": "admin@email.com",
  "phone": null,
  "addresses": [],
  "roles": ["Admin"]
}
```

## Get User by Id

Retrieves a specific user's details by their identifier. Admin authentication is required.

```js
GET "/users/{{id_user}}"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 200 OK: The request was successfully processed and the user details were returned.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The user could not be found.

Example Response:

```json
{
  "id": "1",
  "name": "admin",
  "email": "admin@email.com",
  "phone": null,
  "addresses": [],
  "roles": ["admin"]
}
```

## Get Users

Retrieves the users. Admin authentication is required.

```js
GET "/users?active=true"
```

### Headers

- `Authorization: Bearer {{token}}`

### Query Parameters

- `active` (optional, boolean): Filter the users by their active status.

### Response Format

- 200 OK: The request was successfully processed and the users were returned.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.

Example Response:

```json
[
  {
    "id": "1",
    "name": "admin",
    "email": "admin@email.com",
    "phone": null,
    "addresses": [],
    "roles": ["Admin"]
  },
  {
    "id": "2",
    "name": "Victor Figueiredo Mendes",
    "email": "victor7@email.com",
    "phone": null,
    "addresses": [
      {
        "postalCode": "47305",
        "street": "2856 Overlook Drive",
        "state": "IN",
        "city": "Muncie",
        "neighborhood": "Grove Street, home"
      }
    ],
    "roles": ["Customer"]
  }
]
```

## Update User

Updates a user's details. Users can only update their own details. Administrators can update any other non-administrator user's details.

```js
PUT "/users/{{id_user}}"
```

### Headers

- `Content-Type: application/json`
- `Authorization: Bearer {{token}}`

### Request Format

Field Specifications:

- `name` – A string with at least 3 characters. Cannot be empty.
- `email` – A unique and valid email address. Must have a valid format.
- `phone` (optional) – The user phone. Must have a valid format.

Example Request:

```json
{
  "name": "Fubumga Maloca Bocada",
  "phone": "52948572842",
  "email": "newemail@email.com"
}
```

### Response Format

- 204 NO_CONTENT: The request was successfully processed and the user was updated.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user does not have the necessary updated privileges.
- 404 NOT_FOUND: The user to be updated does not exist.
- 409 CONFLICT: The request email is already in use.

## Deactivate User

Deactivates a user by setting them as inactive. Users can deactivate their accounts, while administrators can deactivate any non-administrator user's account.

```js
DELETE "/users/{{id_user}}"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 204 NO_CONTENT: The request was successfully processed and the user was deactivated (or the user was already inactive).
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user does not have the necessary deactivation privileges.
