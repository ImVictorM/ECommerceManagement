# E-commerce Management API

## Shipping Methods

The Shipping Methods feature provides functionality for managing shipping options in the e-commerce system. It enables administrators to create, update, and delete shipping methods while allowing public access to retrieve available shipping methods. Secure authentication and role-based permissions ensure that only administrators can perform modifications, while customers and visitors can view available shipping methods.

## Create Shipping Method

Creates a new shipping method. Admin authentication is required.

```js
POST "/shipping/methods"
```

### Headers

- `Content-Type: application/json`
- `Authorization: Bearer {{token}}`

### Request Format

Field Specifications:

- `name` - The shipping method name. Cannot be empty.
- `price` - The shipping method price (≥ 0).
- `estimatedDeliveryDays` - The estimated delivery day when using the shipping method (≥ 1).

Example Request:

```json
{
  "name": "FreeDelivery",
  "price": 0,
  "estimatedDeliveryDays": 7
}
```

### Response Format

- 201 CREATED: The new shipping method was created successfully.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.

## Update Shipping Method

Updates a shipping method. Admin authentication is required.

```js
PUT "/shipping/methods/{{id_shipping_method}}"
```

### Headers

- `Content-Type: application/json`
- `Authorization: Bearer {{token}}`

### Request Format

Field Specifications:

- `name` - The shipping method name. Cannot be empty.
- `price` - The shipping method price (≥ 0).
- `estimatedDeliveryDays` - The estimated delivery day when using the shipping method (≥ 1).

Example Request:

```json
{
  "name": "ExpressDelivery",
  "price": 15,
  "estimatedDeliveryDays": 3
}
```

### Response Format

- 204 NO_CONTENT: The shipping method was updated successfully.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The shipping method to be updated does not exist.

## Delete Shipping Method

Deletes an existing shipping method. Admin authentication is required.

```js
DELETE "/shipping/methods/{{id_shipping_method}}"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 204 NO_CONTENT: The shipping method was deleted successfully.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The shipping method does not exist.

## Get Shipping Method by Id

Retrieves a shipping method by its identifier.

```js
GET "/shipping/methods/{{id_shipping_method}}"
```

### Response Format

- 200 OK: The request was approved and the shipping method was returned.

Example Response:

```json
{
  "id": "2",
  "name": "ExpressDelivery",
  "price": 15,
  "estimatedDeliveryDays": 3
}
```

## Get Shipping Methods

Retrieves all available shipping methods.

```js
GET "/shipping/methods"
```

### Response Format

- 200 OK: The request was approved and the available shipping methods were returned.

Example Response:

```json
[
  {
    "id": "1",
    "name": "FreeDelivery",
    "price": 0,
    "estimatedDeliveryDays": 7
  },
  {
    "id": "2",
    "name": "ExpressDelivery",
    "price": 15,
    "estimatedDeliveryDays": 3
  }
]
```
