# Boilerplate

This is a bare metal boilerplate to build Restful API using [ASP.NET Core](https://www.microsoft.com/net/).

---

## Library Stack

- [ASP.NET Core 2.0](https://www.microsoft.com/net/) (Framework)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) (ORM)
- [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity) (User Authentication)
- [JWT](https://jwt.io/) (HTTP Authentication)

---

## Quick Start

### Running

```sh
git clone https://github.com/flipboxstudio/boilerplate.git
cd boilerplate/App
dotnet restore
dotnet ef database update
dotnet run
```

### Testing

```sh
git clone https://github.com/flipboxstudio/boilerplate.git
cd boilerplate/App.Tests
dotnet restore
dotnet test
```

---

## Configuration

### `AppSettings.Database.Driver`

The database driver you want to use for your Application. Valid settings are:
- `Sqlite`
- `Mysql`
- `SqlServer`

### `AppSettings.Jwt.Key`

A secret to encode your JWT.

### `AppSettings.Jwt.Issuer`

The issuer of the JWT. Remember, the application would validate the valid issuer of incoming JWT, so any misconfiguration this config would resulting your token become always invalid. See [here](https://tools.ietf.org/html/rfc7519#section-4.1.1).

### `AppSettings.Jwt.Audience`

The valid audience of the JWT. Remember, the application would validate the valid audience of incoming JWT, so any misconfiguration this config would resulting your token become always invalid. See [here](https://tools.ietf.org/html/rfc7519#section-4.1.3).


### `AppSettings.Jwt.Authority`

The valid authority of the JWT. Remember, the application would validate the valid authority of incoming JWT, so any misconfiguration this config would resulting your token become always invalid.

### `AppSettings.Jwt.Expiry`

The timespan of JWT in minutes. After `n` minutes of issuing the JWT, that token would be blacklisted. Default value is `60`, means after one hour the token would become expired. See [here](https://tools.ietf.org/html/rfc7519#section-4.1.4).

### `ConnectionStrings.{Driver}Connection`

If you configure your database using `Sqlite` driver, the used connection string would be `ConnectionStrings.SqliteConnection`. If you configure your database using `Mysql` driver, the used connection string would be `ConnectionStrings.MysqlConnection`. And if you configure your database using `SqlServer` driver, the used connection string would be `ConnectionStrings.SqlServerConnection`.

---

## Available API

### User Registration

#### Example Request

```
POST /api/v1/auth/register HTTP/1.1
Host: localhost:1822
Content-Type: application/json

{
  "email":"user@example.com",
  "password":"P4ssw0rd!"
}
```

#### Example Response

```json
{
    "message": "Registration success.",
    "status": 806,
    "data": {
        "token": "JWT_TOKEN",
        "user": {
            "id": "bde2fada-39fe-42ab-a0ee-29ce040bc8e9",
            "user_name": "user@example.com",
            "normalized_user_name": "USER@EXAMPLE.COM",
            "email": "user@example.com",
            "normalized_email": "USER@EXAMPLE.COM",
            "email_confirmed": false,
            "security_stamp": "29901b62-2a31-4718-a35e-a8cea3d3e93a",
            "concurrency_stamp": "0d027542-a523-4076-b939-e52ccc6f1221",
            "phone_number_confirmed": false,
            "two_factor_enabled": false,
            "lockout_enabled": true,
            "access_failed_count": 0
        }
    }
}
```

### User Authentication

#### Example Request

```
POST /api/v1/auth/login HTTP/1.1
Host: localhost:1822
Content-Type: application/json

{
  "email":"user@example.com",
  "password":"P4ssw0rd!"
}
```

#### Example Response

```json
{
    "message": "Authentication success.",
    "status": 801,
    "data": {
        "token": "JWT_TOKEN"
    }
}
```

### Check Current User

#### Example Request

```
GET /api/v1/auth/authorize HTTP/1.1
Host: localhost:1822
Authorization: Bearer JWT_TOKEN

```

#### Example Response

```json
{
    "message": "Authorized.",
    "status": 808,
    "data": {
        "user": {
            "id": "bde2fada-39fe-42ab-a0ee-29ce040bc8e9",
            "user_name": "user@example.com",
            "normalized_user_name": "USER@EXAMPLE.COM",
            "email": "user@example.com",
            "normalized_email": "USER@EXAMPLE.COM",
            "email_confirmed": false,
            "security_stamp": "29901b62-2a31-4718-a35e-a8cea3d3e93a",
            "concurrency_stamp": "0d027542-a523-4076-b939-e52ccc6f1221",
            "phone_number_confirmed": false,
            "two_factor_enabled": false,
            "lockout_enabled": true,
            "access_failed_count": 0
        }
    }
}
```

---

## LICENSE

Copyright (c) 2017 Flipbox Studio

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Written with :heart: by Flipbox at Jakarta.