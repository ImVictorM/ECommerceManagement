# E-commerce Management API

## Products

The Products feature provides functionality for managing products within the e-commerce system. It enables administrators to create, update, and deactivate products, as well as manage product inventory. Active products can be publicly retrieved, with optional filtering and pagination. Secure authentication and role-based permissions ensure that only administrators can perform sensitive actions, while public access is allowed for viewing active products.

## Create Product

Creates a new product. Admin authentication is required.

```js
POST "/products"
```

### Headers

- `Authorization: Bearer {{token}}`
- `Content-Type: application/json`

### Request Format

Field Specifications:

- `name` - The product name. Cannot be empty.
- `description` - The product description. Cannot be empty.
- `initialQuantity` - The initial quantity in stock (≥ 1).
- `basePrice` - The product base price (≥ 0).
- `categoryIds` - Array of category IDs for which the product is part of. Cannot be empty.
- `images` - The images representing the product. Cannot be empty.

Request Example:

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

### Response Format

- 204 NO_CONTENT: The request was successfully processed and a new product was created.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.

## Update Product

Updates the details of an active product. Admin authentication is required.

```js
PUT "/products/{{id_product}}"
```

### Header

- `Authorization: Bearer {{token}}`
- `Content-Type: application/json`

### Request Format

Field Specifications:

- `name` - The product name. Cannot be empty.
- `description` - The product description. Cannot be empty.
- `basePrice` - The product base price (≥ 0).
- `categoryIds` - Array of category IDs for which the product is part of. Cannot be empty.
- `images` - The images representing the product. Cannot be empty.

Example Request:

```json
{
  "name": "Mens Cotton Jacket - Black",
  "description": "Great outerwear jackets for Spring/Autumn/Winter.",
  "basePrice": 250,
  "images": ["jacket.png", "jacket-behind.png"],
  "categoryIds": ["1"]
}
```

### Response Format

- 204 NO_CONTENT: The request was successfully processed and the product was updated.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The product to be updated does not exist.

## Add Stock

Increases the quantity available in inventory for an active product. Admin authentication is required.

```js
PUT "/products/{{id_product}}/inventory"
```

### Headers

- `Authorization: Bearer {{token}}`
- `Content-Type: application/json`

### Request Format

Field Specifications:

- `quantityToAdd` - The quantity to increase in the stock (≥ 1).

Example Request:

```json
{
  "quantityToAdd": 25
}
```

### Response Format

- 204 NO_CONTENT: The request was successfully processed and the product inventory was updated.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The product to be updated does not exist.

## Deactivate Product

Deactivates a product and sets the product inventory to 0 items. Admin authentication is required.

```js
DELETE "/products/{{id_product}}"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 204 NO_CONTENT: The request was successfully processed and the product was deactivated.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The product to be deactivated does not exist.

## Get Products

Retrieves all active products.

```js
GET "/products/?category=1&page=1&pageSize=20"
```

### Query Parameters

- `category` (optional) - Filters products by category id
- `page` (optional, default: 1) - Specifies the page number
- `pageSize` (optional, default: 20) - Defines the number of products per page

### Response Format

- 200 OK: The request was successfully processed and all the active products were returned.

Example Response:

```json
[
  {
    "id": "2",
    "name": "Mens Cotton Jacket",
    "description": "Great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions.",
    "basePrice": 200,
    "priceWithDiscount": 200,
    "quantityAvailable": 73,
    "categoryIds": ["1"],
    "images": ["jacket.png"]
  }
]
```

## Get Product By Id

Retrieve an active product by its identifier.

```js
GET "/products/{{id_product}}"
```

### Response Format

- 200 OK: The request was successfully processed and the product was returned.
- 404 NOT_FOUND: The product being queried does not exist (or was deactivated).

Example Response:

```json
{
  "id": "2",
  "name": "Mens Cotton Jacket",
  "description": "Great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions.",
  "basePrice": 200,
  "priceWithDiscount": 200,
  "quantityAvailable": 73,
  "categoryIds": ["1"],
  "images": ["jacket.png"]
}
```
