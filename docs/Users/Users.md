# E-commerce Management API

## Users

### Get User By Authentication Token

Retrieves the currently authenticated user's details. Requires authentication.

```js
GET "users/self"
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

Retrieves a specific user's details by identifier. Admin authentication is required.

```js
GET "/users/{{user_id}}"

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

Retrieves the users. The {{active}} query parameter is optional and can be used to filter active/inactive users. Admin authentication is required.

```js
GET "/users?active=true"
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
PUT "/users/{{user_id}}"
```

#### Headers

- `Content-Type: application/json`
- `Authorization: Bearer {{token}}`

#### Request Format

Field Rules:

- name - must be at least 3 characters long
- phone - can be null
- email - must be unique and have a valid format

```json
{
  "name": "Fubumga Maloca Bocada",
  "phone": "52948572842",
  "email": "newemail@email.com"
}
```

#### Response Format

204 NO_CONTENT

### Deactivate User

Deactivates a user by setting them as inactive. Users can deactivate their accounts, while administrators can deactivate any non-administrator user's account.

```js
DELETE "/users/{{user_id}}"
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

204 NO_CONTENT
