# E-commerce Management API

## Product Reviews

The Product Reviews feature allows customers to leave a review on products they have purchased. Customers can submit ratings and reviews, view reviews left by others, and manage their own reviews. Authentication is required for customer-specific operations.

## Leave Product Review

Allows a customer to leave a review for a purchased product.<br>
Customer authentication is required.

```js
POST "/products/{{id_product}}/reviews"
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

- 201 CREATED: The review was submitted successfully.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The user is not allowed to leave a review for this product.

## Get Product Reviews

Retrieves a list of active reviews for a specific product.

```js
GET "/products/{{id_product}}/reviews"
```

### Response Format

- 200 OK: The request was approved and the product reviews were returned.

Example Response:

```json
[
  {
    "id": 1,
    "title": "Very Good Product",
    "content": "Yes it is",
    "starRating": 5,
    "createdAt": "2025-03-04T12:00:00Z",
    "updatedAt": "2025-03-04T12:00:00Z",
    "user": {
      "id": 2,
      "name": "Victor F"
    }
  }
]
```

## Get Customer Product Reviews

Retrieves a list of active product reviews left by a specific customer. <br>
Customer or admin authentication is required.

```js
GET "/users/customers/{{id_user}}/reviews/"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 200 OK: The request was approved and the customer reviews were returned.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The user is not authorized to view the reviews.

Example Response:

```json
[
  {
    "id": 1,
    "productId": 2,
    "title": "Very Good Product",
    "content": "Yes it is",
    "starRating": 5,
    "createdAt": "2025-03-04T12:00:00Z",
    "updatedAt": "2025-03-04T12:00:00Z"
  }
]
```

## Deactivate Customer Product Review

Deactivates a specific product review from a customer. <br>
Customer or admin authentication is required.

```js
DELETE "/users/customers/{{id_user}}/reviews/{{id_review}}"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 204 NO_CONTENT: The review was successfully deactivated.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The user is not authorized to deactivate the review.
- 404 NOT_FOUND: The specified review does not exist.
