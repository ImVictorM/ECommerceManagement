# E-commerce Management API

## Products

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
- categories - must have at least one category
- images - must have at least one image representing the product
- initialDiscounts - can be empty

PS: Discount percentages must be between 1 and 100. Also, the discount starting date cannot be less than one day in the past, and the ending date must be at least one hour after the starting date. Lastly, products cannot apply more than 90% of aggregate discounts based on their base price.

```json
{
  "name": "Mens Cotton Jacket",
  "description": "Great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions.",
  "initialQuantity": 50,
  "basePrice": 200,
  "categories": ["fashion", "sports_outdoor"],
  "images": ["jacket.png"],
  "initialDiscounts": [
    {
      "percentage": 25,
      "description": "Black Friday discount",
      "startingDate": "2024-11-22T15:29:54.683Z",
      "endingDate": "2024-11-25T15:29:54.683Z"
    }
  ]
}
```

#### Response Format

204 NO_CONTENT

### Get Product By Id

Retrieve an active product by its identifier. No authentication is required.

```js
GET "/products/{{product_id}}"
```

#### Response Format

200 OK

```json
{
  "id": "1",
  "name": "Mens Cotton Jacket",
  "description": "Great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions.",
  "originalPrice": 200,
  "priceWithDiscount": 150,
  "quantityAvailable": 50,
  "discountsApplied": [
    {
      "percentage": 25,
      "description": "Black Friday discount",
      "startingDate": "2024-11-22T15:29:54.683Z",
      "endingDate": "2024-11-25T15:29:54.683Z"
    }
  ],
  "categories": ["fashion", "sports_outdoor"],
  "images": ["jacket.png"]
}
```

### Get Products

Retrieves all active products. It will retrieve the first 20 products if no limit is specified.
It is possible to filter the products by the specified categories appending &category={category_name} to the URL.
No authentication is required.

```js
GET "/products/?category=fashion&category=books_stationery&limit=2"
```

#### Response Format

200 OK

```json
{
  "products": [
    {
      "id": "1",
      "name": "Mens Cotton Jacket",
      "description": "Great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions.",
      "originalPrice": 200,
      "priceWithDiscount": 150,
      "quantityAvailable": 50,
      "discountsApplied": [
        {
          "percentage": 25,
          "description": "Black Friday discount",
          "startingDate": "2024-11-22T15:29:54.683Z",
          "endingDate": "2024-11-25T15:29:54.683Z"
        }
      ],
      "categories": ["fashion", "sports_outdoor"],
      "images": ["jacket.png"]
    }
  ]
}
```

### Get Product Categories

Retrieves all available product categories. No authentication is required.

```js
GET "/products/categories"
```

#### Response Format

200 OK

```json
{
  "categories": [
    "electronics",
    "home_appliances",
    "fashion",
    "footwear",
    "beauty",
    "health_wellness",
    "groceries",
    "furniture",
    "toys_games",
    "books_stationery",
    "sports_outdoor",
    "automotive",
    "pet_supplies",
    "jewelry_watches",
    "office_supplies",
    "home_improvement",
    "baby_products",
    "travel_luggage",
    "music_instruments"
  ]
}
```

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
- categories - must have at least one category
- images - must have at least one image representing the product

```json
{
  "name": "Mens Cotton Jacket - Black",
  "description": "Great outerwear jackets for Spring/Autumn/Winter.",
  "basePrice": 250,
  "images": ["jacket.png", "jacket-behind.png"],
  "categories": ["fashion"]
}
```

#### Response Format

204 NO_CONTENT

### Delete Product

Deactivates a product and set the inventory to 0 items. Admin authentication is required.

```js
DELETE "/products/{{product_id}}"
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

204 NO_CONTENT

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

204 NO_CONTENT

### Update Product Discounts

Changes the list of discounts related to an active product. If you need to remove a product's discounts, send an empty discounts list. Admin authentication is required.

```js
PUT "/products/{{product_id}}/discounts"
```

#### Headers

- `Authorization: Bearer {{token}}`
- `Content-Type: application/json`

#### Request Format

Field Rules:

- initialDiscounts - can be empty

PS: Discount percentages must be between 1 and 100. Also, the discount starting date cannot be less than one day in the past, and the ending date must be at least one hour after the starting date. Lastly, products cannot apply more than 90% of aggregate discounts based on their base price.

```json
{
  "discounts": [
    {
      "percentage": 5,
      "description": "Five percent discount.",
      "startingDate": "2024-11-25T15:55:48.930Z",
      "endingDate": "2024-11-27T15:55:48.930Z"
    }
  ]
}
```

#### Response Format

204 NO_CONTENT
