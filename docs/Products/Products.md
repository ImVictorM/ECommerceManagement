# E-commerce Management API

## Products

The Products feature provides functionality for managing product categories and products within the e-commerce system. It enables administrators to create, update, and delete product categories and products, as well as manage product inventory and deactivate products. Active product and product categories can be publicly retrieved, with optional filtering and pagination for products. Secure authentication and role-based permissions ensure that only administrators can perform sensitive actions, while public access is allowed for viewing active products and categories.

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

### Create Product

Creates a new product. Admin authentication is required.

```js
POST "/products"
```

#### Headers

- `Authorization: Bearer {{token}}`
- `Content-Type: application/json`

#### Request Format

Field Rules:

- name - must not be empty
- description - must not be empty
- initialQuantity - must be greater than 0
- basePrice - must be greater than 0
- categoryIds - must have at least one category
- images - must have at least one image representing the product

```json
{
  "name": "Mens Cotton Jacket",
  "description": "Great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions.",
  "initialQuantity": 50,
  "basePrice": 200,
  "categoryIds": ["1"],
  "images": ["jacket.png"]
}
```

#### Response Format

- 204 NO_CONTENT: The request was successfully processed and a new product was created.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.

### Update Product

Updates the details of an active product. Admin authentication is required.

```js
PUT "/products/{{product_id}}"
```

#### Header

- `Authorization: Bearer {{token}}`
- `Content-Type: application/json`

#### Request Format

Field Rules:

- name - must not be empty
- description - must not be empty
- basePrice - must be greater than 0
- categoryIds - must have at least one category
- images - must have at least one image representing the product

```json
{
  "name": "Mens Cotton Jacket - Black",
  "description": "Great outerwear jackets for Spring/Autumn/Winter.",
  "basePrice": 250,
  "images": ["jacket.png", "jacket-behind.png"],
  "categoryIds": ["1"]
}
```

#### Response Format

- 204 NO_CONTENT: The request was successfully processed and the product was updated.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The product to be updated does not exist.

### Update Product Inventory

Increments the inventory quantity available for an active product. Admin authentication is required.

```js
PUT "/products/{{product_id}}/inventory"
```

#### Headers

- `Authorization: Bearer {{token}}`
- `Content-Type: application/json`

#### Request Format

Field Rules:

- quantityToIncrement - must be greater than 0

```json
{
  "quantityToIncrement": 25
}
```

#### Response Format

- 204 NO_CONTENT: The request was successfully processed and the product inventory was updated.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The product to be updated does not exist.

### Deactivate Product

Deactivates a product and sets the product inventory to 0 items. Admin authentication is required.

```js
DELETE "/products/{{product_id}}"
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

- 204 NO_CONTENT: The request was successfully processed and the product was deactivated.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The product to be deactivated does not exist.

### Get Products

Retrieves all active products. It will retrieve the first 20 products if no limit is specified.
It is possible to filter the products by the specified categories appending &category={category_id} to the URL.
No authentication is required.

```js
GET "/products/?category=1&limit=2"
```

#### Response Format

- 200 OK: The request was successfully processed and all the active products were returned.

```json
[
  {
    "id": "2",
    "name": "Mens Cotton Jacket",
    "description": "Great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions.",
    "basePrice": 200,
    "priceWithDiscount": 200,
    "quantityAvailable": 73,
    "categories": ["sports_outdoor"],
    "images": ["jacket.png"]
  }
]
```

### Get Product By Id

Retrieve an active product by its identifier. No authentication is required.

```js
GET "/products/{{product_id}}"
```

#### Response Format

- 200 OK: The request was successfully processed and the product was returned.

```json
{
  "id": "2",
  "name": "Mens Cotton Jacket",
  "description": "Great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions.",
  "basePrice": 200,
  "priceWithDiscount": 200,
  "quantityAvailable": 73,
  "categories": ["sports_outdoor"],
  "images": ["jacket.png"]
}
```

- 404 NOT_FOUND: The product being queried does not exist (or was deactivated).
