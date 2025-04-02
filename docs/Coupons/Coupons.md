# E-commerce Management API

## Coupons

The Coupons feature provides functionality for managing discount coupons within the e-commerce system. Administrators can create, update, delete, and toggle coupons, as well as retrieve a list of coupons using various filters. Authentication ensures that only administrators can perform these operations.

### Coupon Restrictions

Coupon restrictions allow administrators to control how and when a coupon can be applied. These restrictions are polymorphic, meaning different types of restrictions can be applied to a single coupon. The following restriction types are supported:

#### CouponCategoryRestriction

Specifies restrictions based on product categories.

Field Specifications:

- `$type` - Must be set to "CouponCategoryRestriction".
- `categoryAllowedIds` - Array of category IDs for which the coupon is applicable.
- `productFromCategoryNotAllowedIds` (optional) - Array of product IDs from the allowed categories that should not receive the discount.

Example:

```json
{
  "$type": "CouponCategoryRestriction",
  "categoryAllowedIds": ["1"],
  "productFromCategoryNotAllowedIds": []
}
```

#### CouponProductRestriction

Specifies restrictions based on individual products.

Field Rules

- `$type` - Must be set to "CouponProductRestriction".
- `productAllowedIds` - Array of product IDs for which the coupon is applicable.

Example:

```json
{
  "$type": "CouponProductRestriction",
  "productAllowedIds": ["1"]
}
```

## Create Coupon

Creates a new coupon. Admin authentication is required.

```js
POST "/coupons"
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
- `code` – A unique string identifier for the coupon. Cannot be empty.
- `usageLimit` – The maximum number of times the coupon can be applied (≥ 1).
- `autoApply` – A boolean flag to indicate if the coupon should be applied automatically.
- `minPrice` – The minimum purchase price required for the coupon to be valid (≥ 0).
- `restrictions` (optional) – Array of restriction objects to control coupon applicability.

Example Request:

```json
{
  "discount": {
    "percentage": 50,
    "description": "Black Friday",
    "startingDate": "2025-11-28T16:55:50.273Z",
    "endingDate": "2025-11-30T16:55:50.273Z"
  },
  "code": "BLACK50",
  "usageLimit": 250,
  "autoApply": true,
  "minPrice": 500,
  "restrictions": [
    {
      "$type": "CouponCategoryRestriction",
      "categoryAllowedIds": ["1"],
      "productFromCategoryNotAllowedIds": []
    },
    {
      "$type": "CouponProductRestriction",
      "productAllowedIds": ["1"]
    }
  ]
}
```

### Response Format

- 201 CREATED: Coupon successfully created.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.

## Get Coupons

Retrieves a list of coupons based on the specified filters. Admin authentication is required.

```js
GET "/coupons"
```

### Headers

- `Authorization: Bearer {{token}}`

### Query Parameters

- `active` (optional, boolean): Filter the coupons by their activation status.
- `expiringAfter`(optional, dateTime): Return coupons expiring after the specified UTC date.
- `expiringBefore` (optional, dateTime): Return coupons expiring before the specified UTC date.
- `validForDate` (optional, dateTime): Return coupons that are valid on the specified UTC date.

### Response Format

- 200 OK: Coupons successfully retrieved.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.

Example Response:

```json
[
  {
    "id": "1",
    "code": "BLACK50",
    "discount": {
      "percentage": 50,
      "description": "Black Friday",
      "startingDate": "2025-11-28T16:55:50.273+00:00",
      "endingDate": "2025-11-30T16:55:50.273+00:00"
    },
    "usageLimit": 250,
    "autoApply": true,
    "minPrice": 500,
    "restrictions": [
      {
        "$type": "CouponCategoryRestriction",
        "categoryAllowedIds": ["1"],
        "productFromCategoryNotAllowedIds": []
      },
      {
        "$type": "CouponProductRestriction",
        "productAllowedIds": ["1"]
      }
    ]
  }
]
```

## Delete Coupon

Deletes an existent coupon. Admin authentication is required.

```js
DELETE "/coupons/{{id_coupon}}"
```

### Headers

- `Authorization: Bearer {{token}}``

### Response Format

- 204 NO_CONTENT: Coupon successfully deleted.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The coupon to be deleted does not exist.

### Toggle Coupon Activation

Toggles the active status of an existent coupon. Admin authentication is required.

```js
PATCH "/coupons/{{id_coupon}}"
```

### Headers

- `Authorization: Bearer {{token}}`

### Response Format

- 204 NO_CONTENT: Coupon active status successfully toggled.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The coupon to be updated does not exist.

## Update Coupon

Updates the details of an existent coupon. Admin authentication is required.

```js
PUT "/coupons/{{id_coupon}}"
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
- `code` – A unique string identifier for the coupon. Cannot be empty.
- `usageLimit` – The maximum number of times the coupon can be applied (≥ 1).
- `autoApply` – A boolean flag to indicate if the coupon should be applied automatically.
- `minPrice` – The minimum purchase price required for the coupon to be valid (≥ 0).
- `restrictions` (optional) – Array of restriction objects to control coupon applicability.

Example Request:

```json
{
  "discount": {
    "percentage": 50,
    "description": "Black Friday",
    "startingDate": "2025-11-28T16:55:50.273Z",
    "endingDate": "2025-11-30T16:55:50.273Z"
  },
  "code": "BLACK50",
  "usageLimit": 250,
  "autoApply": true,
  "minPrice": 500,
  "restrictions": [
    {
      "$type": "CouponCategoryRestriction",
      "categoryAllowedIds": ["1"],
      "productFromCategoryNotAllowedIds": []
    }
  ]
}
```

### Response Format

- 204 NO_CONTENT: Coupon successfully updated.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not an administrator.
- 404 NOT_FOUND: The coupon to be updated does not exist.
