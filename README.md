# Boilerplate

This is a dead simple boilerplate to build Restful API using .NET Core.

## Features

- ORM using [Dapper](https://github.com/StackExchange/Dapper) over [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/).
    * Dapper MicroCRUD.
- [BCrypt](https://en.wikipedia.org/wiki/Bcrypt) Password Hashing.
- [JWT](https://en.wikipedia.org/wiki/JSON_Web_Token) Authentication.
- JSON page error handler.
- Fully configurable via `appsetings.json`.
- Route versioning.
- Lots of helpers (check the `Extensions` folder).
- Static assets provider.
- Mailer.
- Need HTML template engine? Don't worry. We have HandleBars!
- Protobuff 3 for Input and Output.
- More to come :)

## Quick Start

- Clone this repo.
- Remove `.git` folder if needed.
- Restore database inside `AppData` folder.
- Configure database connection in `appsettings.json`.
- Configure email provider in `appsettings.json`.
- Inside `app` folder, run `dotnet restore`.
- To run this app, there are two alternatives:
  - Via [Debugger](https://docs.microsoft.com/en-us/dotnet/articles/csharp/getting-started/with-visual-studio-code#debug) in [Visual Studio Code](https://code.visualstudio.com/) (recommended):
    - Open "Debug" tab [Ctrl + Shift + D]
    - Run code
  - Via dotnet:
    - Inside `app` folder, run `dotnet run`.

> **ATTENTION** When running inside production machine, make sure you have generated JWT Signing Key.

## Routes

Below is available route comes with this boilerplate:

```
GET   /                         => root [allow-anonymous]
GET   /v1                       => v1 root [allow-anonymous]
POST  /v1/auth/register         => register a new user [allow-anonymous] [sending email]
POST  /v1/auth/login            => authentication
POST  /v1/auth/forgot           => request new password via email, in case you forgot your password [allow-anonymous] [sending email]
GET   /v1/auth/user             => check authentication, return current authenticated user
PUT   /v1/user/profile          => change basic user profile
PUT   /v1/user/password         => change user password
PUT   /v1/user/avatar           => change user avatar
```

## Example Request

I made a simple Postman Collection [here](https://www.getpostman.com/collections/bfc5c63ad66543463321).

## TODO

- [ ] Unit test, docs [here](https://docs.microsoft.com/en-us/dotnet/articles/core/testing/unit-testing-with-dotnet-test).
- [ ] Write a blog post about this boilerplate.

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

Written with :heart: by Krisan Alfa Timur at Jakarta.