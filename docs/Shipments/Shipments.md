# E-commerce Management API

## Shipments

The Shipments feature manages the lifecycle of shipments within the e-commerce system. It allows carriers to update shipment statuses as they progress through different stages of the delivery process. Secure authentication and role-based permissions ensure that only authorized carriers can advance shipment statuses.

### Advance Shipment Status

Advances a shipment status to the next state. Carrier authentication is required.<br>

> Note: A shipment with a status of "Pending" or "Canceled" cannot be advanced manually. The system automatically updates the status from "Pending" to "Preparing" when the order payment is authorized.

```js
PATCH "/shipments/{{id_shipment}}/status"
```

#### Headers

- `Authorization: Bearer {{token}}`

#### Response Format

- 204 NO_CONTENT: The shipment status was successfully updated.
- 400 BAD_REQUEST: The shipment is in a state that cannot be advanced.
- 401 UNAUTHORIZED: The current user is not authenticated.
- 403 FORBIDDEN: The current user is not authorized to update this shipment.
- 404 NOT_FOUND: The specified shipment does not exist.
