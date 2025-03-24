# E-commerce Management API

## Sales

The Sales feature allows administrators to create, update, delete, and retrieve sales within the e-commerce system. Each sale can be applied to specific products or categories, and must adhere to discount thresholds to ensure pricing integrity.

### Sale Restrictions

- A sale cannot be created if the final price of any product, after applying all applicable sales, falls below 10% of the product's base price.
- A sale must contain at least one product or one category.

## Create Sale

Creates a new sale, defining the products and/or categories. Admin authentication is required.

```js
POST "/sales"
```

### Headers

- `Authorization: Bearer {{token}}`
- `Content-Type: application/json`

### Request Format

Field Specifications:

- `discount`: Object containing:
  - `percentage` – A positive integer representing the discount percentage (1-100).
  - `description` – The discount description.
  - `startingDate` – The UTC ISO date when the discount starts.
  - `endingDate` – The UTC ISO date when the discount ends.
- `categoryOnSaleIds` – Array of category IDs for which the sale is applicable.
- `productOnSaleIds` – Array of product IDs for which the sale is applicable.
- `productExcludedFromSaleIds` – Array of product IDs that should be excluded from the sale.

Example Request:

```json
{
  "discount": {
    "percentage": 5,
    "description": "Weekend discount",
    "startingDate": "2025-03-28T16:00:44.355Z",
    "endingDate": "2025-03-30T16:00:44.355Z"
  },
  "categoryOnSaleIds": ["1"],
  "productOnSaleIds": [],
  "productExcludedFromSaleIds": []
}
```

### Response Format

- 201 CREATED: Sale successfully created.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.

## Update Sale

Updates an existing sale. Admin authentication is required.

```js
PUT "/sales/{{id_sale}}"
```

### Headers

- `Authorization: Bearer {{token}}`
- `Content-Type: application/json`

### Request Format

Field Specifications:

- `discount`: Object containing:
  - `percentage` – A positive integer representing the discount percentage (1-100).
  - `description` – The discount description.
  - `startingDate` – The UTC ISO date when the discount starts.
  - `endingDate` – The UTC ISO date when the discount ends.
- `categoryOnSaleIds` – Array of category IDs for which the sale is applicable.
- `productOnSaleIds` – Array of product IDs for which the sale is applicable.
- `productExcludedFromSaleIds` – Array of product IDs that should be excluded from the sale.

Example Request:

```json
{
  "discount": {
    "percentage": 15,
    "description": "Weekend discount",
    "startingDate": "2025-03-28T16:00:44.355Z",
    "endingDate": "2025-03-30T16:00:44.355Z"
  },
  "categoryOnSaleIds": ["1"],
  "productOnSaleIds": [],
  "productExcludedFromSaleIds": []
}
```

### Response Format

- 204 NO_CONTENT: Sale successfully updated.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The sale to be updated does not exist.

## Delete Sale

Deletes an existing sale. Admin authentication is required.

```js
DELETE "/sales/{{id_sale}}"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 204 NO_CONTENT: Sale successfully deleted.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The sale to be deleted does not exist.

## Get Sales

Retrieves a list of sales based on the specified filters. Admin authentication is required.

```js
GET "/sales"
```

### Headers

- `Authorization: Bearer {{token}}`

### Query Parameters

- `expiringAfter` (optional, dateTime): Filter sales that expire after the specified UTC date.
- `expiringBefore` (optional, dateTime): Filter sales that expire before the specified UTC date.
- `validForDate` (optional, dateTime): Filter sales that are valid on the specified UTC date.

### Response Format

- 200 OK: Sales successfully retrieved.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.

Example Response:

```json
[
  {
    "id": "1",
    "discount": {
      "percentage": 5,
      "description": "Weekend discount",
      "startingDate": "2025-03-28T16:00:44.355Z",
      "endingDate": "2025-03-30T16:00:44.355Z"
    },
    "categoryOnSaleIds": ["1"],
    "productOnSaleIds": [],
    "productExcludedFromSaleIds": []
  }
]
```

## Get Sale By Id

Retrieves a sale by its identifier. Admin authentication is required.

```js
GET "/sales/{{id_sale}}"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 200 OK: Sale successfully retrieved.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The requested sale does not exist.

Example Response:

```json
{
  "id": "1",
  "discount": {
    "percentage": 5,
    "description": "Weekend discount",
    "startingDate": "2025-03-28T16:00:44.355Z",
    "endingDate": "2025-03-30T16:00:44.355Z"
  },
  "categoryOnSaleIds": ["1"],
  "productOnSaleIds": [],
  "productExcludedFromSaleIds": []
}
```
