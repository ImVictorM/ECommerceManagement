# E-commerce Management API

## Payment Webhooks

The Payment Webhooks feature provides an endpoint that acts as a webhook for payment gateways. This endpoint receives notifications from the payment gateway and updates internal data accordingly. Secure communication is ensured through an HMAC-based signature validation.

## Handle Payment Status Changed

This endpoint allows payment gateways to send notifications regarding payment status changes. The provided signature is validated to ensure the authenticity of the request.

```js
POST "/webhooks/payments"
```

### Headers

- `Content-Type: application/json`
- `X-Provider-Signature: A hashed signature generated using an HMAC algorithm to ensure the request's authenticity.`

### Request Format

Field Specifications:

- `paymentId` – A valid identifier for the payment transaction. Cannot be empty.
- `paymentStatus` – A predefined status indicating the current state of the payment. Must match system-supported statuses.

Example Request:

```json
{
  "paymentId": "66b43390-c206-4ec9-8384-173029982ca8",
  "paymentStatus": "Authorized"
}
```

### Response Format

- 204 NO_CONTENT: The notification was successfully processed, and internal data was updated.
- 401 UNAUTHORIZED: The provided signature is invalid.
- 400 BAD_REQUEST: The request body is invalid or missing required fields.

> Notes: <br>
> Signature Validation: The X-Provider-Signature header must contain a valid signature generated using an HMAC algorithm. Both internal and the payment gateway algorithm must share the same secret key.<br/>
> Payment Statuses: Supported values for paymentStatus should align with the predefined statuses in the system.
