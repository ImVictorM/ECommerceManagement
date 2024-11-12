# E-commerce Management API

## Users

### Get User By Authentication Token

Retrieves the currently authenticated user's details. This endpoint is accessible to any authenticated user. It is meant to be used by customer to fetch data about themselves.

```js
GET {{base_endpoint}}/self
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

200 OK

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

### Get User By Id

Retrieves a specific user's details by ID. This endpoint is only accessible to administrators.

```js
GET {{base_endpoint}}/{{user_id}}

```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

200 OK

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

### Get Users

Retrieves a list of users. This endpoint is only accessible to administrators. The active query parameter is optional and can be used to filter active/inactive users.

```js
GET {{base_endpoint}}?active=true
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

200 OK

```json
{
  "users": [
    {
      "id": "1",
      "name": "admin",
      "email": "admin@email.com",
      "phone": null,
      "addresses": [],
      "roles": ["admin"]
    },
    {
      "id": "2",
      "name": "Fubumga Maloca Bocada",
      "email": "newemail@email.com",
      "phone": "52948572842",
      "addresses": [],
      "roles": ["customer"]
    }
  ]
}
```

### Update User

Updates a user's details. Users can only update their own details. Administrators can update any other non-administrator user's details.

```js
PUT {{base_endpoint}}/{{user_id}}
```

#### Headers

- `Content-Type: application/json`
- `Authorization: Bearer {{token}}`

#### Request Format

```json
{
  "name": "Fubumga Maloca Bocada",
  "phone": "52948572842",
  "email": "newemail@email.com"
}
```

#### Response Format

200 OK

### Deactivate User

Deactivates a user by setting them as inactive. Users can deactivate their own accounts, while administrators can deactivate any non-administrator user's account.

```js
DELETE {{base_endpoint}}/{{user_id}}
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

200 Ok
