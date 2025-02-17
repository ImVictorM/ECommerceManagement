# E-commerce Management API

## Categories

The Categories feature provides functionality for managing product categories within the e-commerce system. It enables administrators to create, update, and delete product categories. Product categories can be publicly retrieved. Secure authentication and role-based permissions ensure that only administrators can perform sensitive actions, while public access is allowed for viewing products categories.

### Create Product Category

Creates a new product category. Admin authentication is required.

```js
POST "/products/categories"
```

#### Headers

- `Content-Type: application/json`
- `Authorization: Bearer {{token}}`

#### Request Format

Field Rules:

- name - must not be empty

```json
{
  "name": "fashion"
}
```

#### Response Format

- 201 CREATED: The new category was created successfully.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.

### Delete Product Category

Deletes an existing product category. Admin authentication is required.

```js
DELETE "/products/categories/{{id_category}}"
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

- 204 NO_CONTENT: The category was deleted successfully.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The category does not exist.

### Update Product Category

Updates an existing product category name. Admin authentication is required.

```js
PUT "/products/categories/{{id_category}}"
```

#### Headers

- `Content-Type: application/json`
- `Authorization: Bearer {{token}}`

#### Request Format

Field Rules:

- name - must not be empty

```json
{
  "name": "sports"
}
```

#### Response Format

- 204 NO_CONTENT: The category was updated successfully.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The category to be updated does not exist.

### Get Product Categories

Retrieves all available product categories. No authentication is required.

```js
GET "/products/categories"
```

#### Response Format

- 200 OK: The request was approved and the available product categories were returned.

```json
[
  {
    "id": "1",
    "name": "fashion"
  },
  {
    "id": "2",
    "name": "sports"
  }
]
```

### Get Product Category by Id

Retrieves a product category by its identifier. No authentication is required.

```js
GET "/products/categories/{{id_category}}"
```

#### Response Format

- 200 OK: The request was approved and the product category was returned.

```json
{
  "id": "1",
  "name": "fashion"
}
```

- 404 NOT_FOUND: The product category being queried does not exist.
