# E-commerce Management API

## Orders

The Orders feature provides functionality for managing orders within the e-commerce system. It enables authenticated customers to place orders and allows administrators to view and manage orders. Customers can retrieve their own orders, while administrators can access all orders. Secure authentication and role-based permissions ensure that only authorized users can perform specific actions, such as placing orders or retrieving sensitive order details.

### Place Order

Allows a customer to place an order. Customer authentication is required.

```js
POST "/orders"
```

#### Headers

- `Content-Type: application/json`
- `X-Idempotency-Key: request-id`
- `Authorization: Bearer {{token}}`

#### Request Format

Field Rules:

- products - must not be empty
- installments - must be greater or equal to 1

```json
{
  "products": [
    {
      "productId": "2",
      "quantity": 2
    }
  ],
  "billingAddress": {
    "postalCode": "47305",
    "street": "2856 Overlook Drive",
    "state": "IN",
    "city": "Muncie",
    "neighborhood": "Grove Street, home"
  },
  "deliveryAddress": {
    "postalCode": "47305",
    "street": "2856 Overlook Drive",
    "state": "IN",
    "city": "Muncie",
    "neighborhood": "Grove Street, home"
  },
  "paymentMethod": {
    "type": "credit_card"
  },
  "installments": 1
}
```

#### Response Format

- 201 CREATED: The order was placed successfully.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not a customer.

### Get All Orders

Retrieves all the orders. Admin authentication is required.
Can receive an optional status parameter to filter the orders.

```js
GET "/orders?status=pending"
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

- 200 OK: The request was approved and the orders were returned.

```json
[
  {
    "id": "1",
    "ownerId": "2",
    "description": "Order pending. Waiting for payment",
    "status": "Pending",
    "total": 400
  },
  {
    "id": "2",
    "ownerId": "2",
    "description": "Order pending. Waiting for payment",
    "status": "Pending",
    "total": 1500
  }
]
```

- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.

### Get Order by Id

Retrieves and order by its identifier. Admin authentication is required.

```js
GET "/orders/{{id_order}}"
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

- 200 OK: The request was approved and the order was found and returned.

```json
{
  "id": "1",
  "ownerId": "2",
  "description": "Order pending. Waiting for payment",
  "status": "Pending",
  "total": 400
}
```

- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The order being queried does not exist.

### Get Customer Orders

Retrieves all orders related to a customer. Self or admin authentication is required.
Can receive an optional status parameter to filter the orders.

```js
GET "/users/{{id_user}}/orders?status=pending"
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

- 200 OK: The request was approved and the orders were returned.

```json
[
  {
    "id": "1",
    "ownerId": "2",
    "description": "Order pending. Waiting for payment",
    "status": "Pending",
    "total": 400
  },
  {
    "id": "2",
    "ownerId": "2",
    "description": "Order pending. Waiting for payment",
    "status": "Pending",
    "total": 1500
  }
]
```

- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not the orders owner or an administrator.

### Get Customer Order by Id

Retrieves a customer's order by its identifier. Self or admin authentication is required.

```js
GET "/users/{{id_user}}/orders/{{id_order}}"
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

- 200 OK: The request was approved and the order was found and returned.

```json
{
  "id": "1",
  "ownerId": "2",
  "description": "Order pending. Waiting for payment",
  "status": "Pending",
  "total": 400
}
```

- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not the order owner or an administrator.
- 404 NOT_FOUND: The order being queried does not exist.
