# Boilerplate

This is a dead simple boilerplate to build Restful API using .NET Core.

## Features

- ORM using [Dapper](https://github.com/StackExchange/Dapper) over [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/).
- [BCrypt](https://en.wikipedia.org/wiki/Bcrypt) Password Hashing.
- [JWT](https://en.wikipedia.org/wiki/JSON_Web_Token) Authentication.
    * With blacklist method available via memory caching<sup>*</sup>, read more [here](https://auth0.com/blog/blacklist-json-web-token-api-keys/).
- JSON page error handler.
- Fully configurable via `appsetings.json`.
- Route versioning.
- Lots of helpers (check the `Extensions` folder).
- More to come :)

<sup>*</sup>: Restarting server may cause all token become blacklisted.

## Routes

Below is available route comes with this boilerplate:

```
GET   /                  => root [allow-anonymous]
GET   /v1                => v1 root [allow-anonymous]
POST  /v1/auth/login     => authentication
GET   /v1/auth/user      => check authentication, return current authenticated user
GET   /v1/auth/admin     => same as above, but this route is for admin role only
POST  /v1/auth/logout    => logout
PATCH /v1/auth/refresh   => refresh the token
```

### Default Credentials

#### User

```js
var unirest = require("unirest");
var req = unirest("POST", "http://localhost:13848/v1/auth/login");

req.headers({
  "cache-control": "no-cache",
  "content-type": "application/json"
});

req.type("json");
req.send({
  "Identity": "user",
  "Password": "user"
});

req.end(function (res) {
  if (res.error) throw new Error(res.error);

  console.log(res.body);
});
```

#### Admin

```js
var unirest = require("unirest");
var req = unirest("POST", "http://localhost:13848/v1/auth/login");

req.headers({
  "cache-control": "no-cache",
  "content-type": "application/json"
});

req.type("json");
req.send({
  "Identity": "admin",
  "Password": "admin"
});

req.end(function (res) {
  if (res.error) throw new Error(res.error);

  console.log(res.body);
});
```

## Getting Started (Need Improvement)

> **NOTE**: This boilerplate is not tested yet on Linux and Mac, the result of quickstart may vary.

1. Clone this repo.
2. Restore tha package via `dotnet restore`.
3. Create the database and import `Users` table from `Boilerplate.sql`.
4. Adjust your database connection in `appsettings.json`.
5. Run the application via `dotnet run`.

## TODO

I'm **NOT** planning to make this boilerplate works with any other RDMS but the SQL Server one.
The problem is, current boilerplate not using [SQLBuilder](https://github.com/StackExchange/Dapper/tree/master/Dapper.SqlBuilder).
If you want it be, just sent me a pull request.

- [] Unit test, docs [here](https://docs.microsoft.com/en-us/dotnet/articles/core/testing/unit-testing-with-dotnet-test).
- [] Improve [documentation](https://msdn.microsoft.com/en-us/library/5ast78ax.aspx).
- [] Test on Linux and Mac.
- [] Change Memory Caching to something [distributed](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed) like [Redis](https://redis.io/).

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