# E-commerce Management API

## Product Feedback

The Product Feedback feature allows customers to leave feedback on products they have purchased. Customers can submit ratings and reviews, view feedback left by others, and manage their own feedback. Authentication is required for customer-specific operations.

## Leave Product Feedback

Allows a customer to leave feedback for a purchased product.<br>
Customer authentication is required.

```js
POST "/products/{{id_product}}/feedback"
```

### Headers

- `Content-Type: application/json`
- `Authorization: Bearer {{token}}`

### Request Format

Field Specifications:

- `title` - The review title. Cannot be empty.
- `content` - The review content message. Cannot be empty.
- `starRating` - A positive integer representing the rating (1-5).

Example Request:

```json
{
  "title": "Very Good Product",
  "content": "Yes it is",
  "starRating": 5
}
```

### Response Format

- 201 CREATED: The feedback was submitted successfully.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The user is not allowed to leave feedback for this product.

## Get Product Feedback

Retrieves a list of active feedback items for a specific product.

```js
GET "/products/{{id_product}}/feedback"
```

### Response Format

- 200 OK: The request was approved and the product feedback was returned.

Example Response:

```json
[
  {
    "id": 1,
    "title": "Very Good Product",
    "content": "Yes it is",
    "starRating": 5,
    "userId": 2,
    "productId": 2,
    "createdAt": "2025-03-04T12:00:00Z"
  }
]
```

## Get Customer Product Feedback

Retrieves a list of active product feedback items left by a specific customer. <br>
Customer or admin authentication is required.

```js
GET "/users/customers/{{id_user}}/feedback/"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 200 OK: The request was approved and the customer feedback was returned.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The user is not authorized to view this feedback.

Example Response:

```json
[
  {
    "id": 1,
    "title": "Very Good Product",
    "content": "Yes it is",
    "starRating": 5,
    "productId": 2,
    "createdAt": "2025-03-04T12:00:00Z"
  }
]
```

## Deactivate Customer Product Feedback

Deactivates a specific product feedback entry from a customer. <br>
Customer or admin authentication is required.

```js
DELETE "/users/customers/{{id_user}}/feedback/{{id_feedback}}"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 204 NO_CONTENT: The feedback was successfully deactivated.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The user is not authorized to delete this feedback.
- 404 NOT_FOUND: The specified feedback does not exist.
