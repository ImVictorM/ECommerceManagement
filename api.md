# Ecommerce Management API

## Authentication

### Register
```js
POST {{host}}/auth/register
```
#### Request format
```json
{
	"name": "Victor Figueiredo Mendes",
	"email": "victor7@email.com",
	"password: "victor123!",
}
````
#### Response format
201 Created
```json
{
	"id": 1,
	"name": "Victor Figueiredo Mendes",
	"email": "victor7@email.com",
	"token": "eyJhbGciOiJIUzI1N...adQssw5c"
}
```

### Login
```js
POST {{host}}/auth/login
```
#### Request format
```json
{
	"email": "victor7@email.com",
	"password: "victor123!"
}
````
#### Response format
200 Ok
```json
{
	"id": 1,
	"name": "Victor Figueiredo Mendes",
	"email": "victor7@email.com",
	"token": "eyJhbGciOiJIUzI1N...adQssw5c"
}
```
