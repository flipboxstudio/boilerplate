# Boilerplate

This is a dead simple boilerplate to build Restful API using .NET Core.

## Features

- ORM using [Dapper](https://github.com/StackExchange/Dapper) over [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/).
    * Dapper SQL Builder.
- [BCrypt](https://en.wikipedia.org/wiki/Bcrypt) Password Hashing.
- [JWT](https://en.wikipedia.org/wiki/JSON_Web_Token) Authentication.
    * With blacklist method available via memory caching<sup>*</sup>, read more [here](https://auth0.com/blog/blacklist-json-web-token-api-keys/).
- JSON page error handler.
- Fully configurable via `appsetings.json`.
- Route versioning.
- Lots of helpers (check the `Extensions` folder).
- More to come :)

[<sup>*</sup>]: Restarting server may cause all token become blacklisted.

## Quick Start

- Clone this repo.
- Remove `.git` folder if needed.
- Create database:
  - `CREATE DATABASE app`.
  - Restore the SQL file inside `AppData` folder.
  - Download GeoNames file (per country is enough), restore it using command `LOAD DATA LOCAL INFILE '/path/to/file.txt' INTO TABLE app.GeoNames;`.
- Configure database connection in `appsettings.json`.
- Inside `app` folder, run `dotnet restore`.
- To run this app, there are two alternatives:
  - Via [Debugger](https://docs.microsoft.com/en-us/dotnet/articles/csharp/getting-started/with-visual-studio-code#debug) in [Visual Studio Code](https://code.visualstudio.com/) (recommended):
    - Open "Debug" tab [Ctrl + Shift + D]
    - Run code
  - Via dotnet:
    - Inside `app` folder, run `dotnet run`.

## Routes

Below is available route comes with this boilerplate:

```
GET   /                         => root [allow-anonymous]
GET   /v1                       => v1 root [allow-anonymous]
POST  /v1/auth/login            => authentication
GET   /v1/auth/user             => check authentication, return current authenticated user
GET   /v1/auth/admin            => same as above, but this route is for admin role only
POST  /v1/auth/logout           => logout
PATCH /v1/auth/refresh          => refresh the token
PATCH /v1/geonames?Query=@Query => example route that interacts with database
```

### Default Credentials

#### User

```js
var unirest = require("unirest");
var req = unirest("POST", "http://localhost:5000/v1/auth/login");

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
var req = unirest("POST", "http://localhost:5000/v1/auth/login");

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

## TODO

- [ ] Unit test, docs [here](https://docs.microsoft.com/en-us/dotnet/articles/core/testing/unit-testing-with-dotnet-test).
- [ ] Improve [documentation](https://msdn.microsoft.com/en-us/library/5ast78ax.aspx).
- [ ] Test on Linux and Mac.
- [ ] Change Memory Caching to something [distributed](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed) like [Redis](https://redis.io/).

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