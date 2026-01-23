# Student Course Enrollment API (.NET 8 + EF Core + SQLite)

[![CI](https://github.com/ashenneth/StudentCourseEnrollmentApi/actions/workflows/ci.yml/badge.svg)](https://github.com/ashenneth/StudentCourseEnrollmentApi/actions/workflows/ci.yml)

A clean, beginner-friendly **CRUD REST API** built with **ASP.NET Core Web API (.NET 8)** and **EF Core**.  
Manages **Students**, **Courses**, and **Enrollments** with validation, correct HTTP status codes, pagination/filtering, global exception handling, unit tests, and CI.

---

## Why this project?

This project demonstrates that I can build a complete REST API end-to-end with:
- proper REST patterns (DTOs, status codes, validation)
- EF Core + migrations (SQLite)
- a simple layered architecture (Controllers → Services → Repositories)
- engineering discipline (tests + GitHub Actions CI)
- production-friendly basics (global exception handling middleware)

---

## Features

- CRUD for Students and Courses
- Enroll / list / unenroll students in courses
- Business rules:
  - Student **Email** is unique
  - Course **Code** is unique, normalized to **uppercase**, and 4–10 chars
  - Credits must be 1–6
  - A student cannot enroll in the same course twice
- Correct HTTP status codes: **200, 201, 204, 400, 404, 409**
- DTO-based API (EF entities are not exposed)
- Pagination + filtering (Students list): `page`, `pageSize`, `search`, `isActive`
- Swagger UI enabled
- Global exception handling middleware (consistent error responses + safe failures)
- Unit tests (xUnit) for key business rules
- GitHub Actions CI (restore → build → test on push/PR)

---

## Tech Stack

- **.NET 8** — ASP.NET Core Web API  
- **EF Core** — ORM  
- **SQLite** — default database  
- **Swagger / OpenAPI** — API docs and testing  
- **xUnit** — tests  
- **GitHub Actions** — CI pipeline  

---

## Architecture

Simple layered structure:

```
Controllers → Services → Repositories → EF Core (DbContext)
```

- Controllers handle HTTP and return correct status codes.
- Services contain business rules (uniqueness checks, validations, pagination logic).
- Repositories encapsulate database access using EF Core.
- A global exception middleware catches unexpected exceptions and returns consistent JSON error responses.

---

## Project Structure

```
.github/workflows/
  ci.yml

src/StudentCourseEnrollmentApi/
  Common/
  Controllers/
  DTOs/
  Data/
  Domain/Entities/
  Middleware/
  Migrations/
  Repositories/
  Services/
  Program.cs
  StudentCourseEnrollmentApi.csproj
  appsettings.json
  appsettings.Development.json

tests/StudentCourseEnrollmentApi.Tests/
  TestDbFactory.cs
  StudentServiceTests.cs
  CourseServiceTests.cs
  EnrollmentServiceTests.cs

StudentCourseEnrollmentApi.sln
.gitignore
README.md
```

---

## Domain Models (Simplified)

### Student
- `Id` (int)
- `FullName` (string)
- `Email` (string, unique)
- `DateOfBirth` (DateOnly)
- `IsActive` (bool)

### Course
- `Id` (int)
- `Title` (string)
- `Code` (string, unique, uppercase, length 4–10)
- `Credits` (int, 1–6)
- `IsActive` (bool)

### Enrollment
- `Id` (int)
- `StudentId` (int)
- `CourseId` (int)
- `EnrolledOn` (DateOnly)
- Unique rule: cannot enroll same student into same course twice

---

## Validation Rules

- Student `Email` must be valid and **unique**
- Course `Code` must be **unique**, **uppercase**, and **4–10 chars**
- Course `Credits` must be **1–6**
- Enrollment `(StudentId, CourseId)` must be **unique**

---

## HTTP Status Codes Used

- `200 OK` — successful fetch
- `201 Created` — created resource
- `204 No Content` — successful update/delete (no body)
- `400 Bad Request` — invalid input / validation failures
- `404 Not Found` — resource not found
- `409 Conflict` — uniqueness violations / duplicate enrollment

---

## Run Locally

### Prerequisites
- .NET 8 SDK

### Restore
```bash
dotnet restore
```

### Database (SQLite + EF Core migrations)

Install EF CLI if needed:
```bash
dotnet tool install --global dotnet-ef
```

Apply migrations (from repo root):
```bash
dotnet ef database update --project src/StudentCourseEnrollmentApi --startup-project src/StudentCourseEnrollmentApi
```

Notes:
- Migrations are committed so the schema can be reproduced reliably.
- Do not commit the SQLite `.db` file (ignored via `.gitignore`).

### Run the API
```bash
dotnet run --project src/StudentCourseEnrollmentApi
```

Swagger UI:
- `https://localhost:<PORT>/swagger`

> Tip: Use the **http** URL in Postman if https certificate causes issues.

---

## Date Format (DateOnly)

All `DateOnly` fields use `yyyy-MM-dd`.

Example:
```json
"dateOfBirth": "2001-04-15"
```

---

## API Endpoints

### Students

- `GET /api/students` (paged + filters)
- `GET /api/students/{id}`
- `POST /api/students`
- `PUT /api/students/{id}`
- `DELETE /api/students/{id}`

#### GET /api/students (Pagination + Filtering)

Query params:
- `page` (default 1)
- `pageSize` (default 10)
- `search` (optional: matches `FullName` or `Email`)
- `isActive` (optional: `true`/`false`)

Example:
```
GET /api/students?page=1&pageSize=5&search=gmail&isActive=true
```

Response example:
```json
{
  "items": [
    {
      "id": 1,
      "fullName": "John Daniel",
      "email": "John@example.com",
      "dateOfBirth": "2001-04-15",
      "isActive": true
    }
  ],
  "page": 1,
  "pageSize": 5,
  "totalCount": 12
}
```

#### POST /api/students
```json
{
  "fullName": "John Daniel",
  "email": "John@example.com",
  "dateOfBirth": "2001-04-15",
  "isActive": true
}
```

---

### Courses

- `GET /api/courses`
- `GET /api/courses/{id}`
- `POST /api/courses`
- `PUT /api/courses/{id}`
- `DELETE /api/courses/{id}`

#### POST /api/courses
```json
{
  "title": "Introduction to Programming",
  "code": "cs101",
  "credits": 3,
  "isActive": true
}
```

> Note: `code` is normalized to uppercase (e.g., `CS101`).

---

### Enrollments

- `POST /api/enrollments` (enroll student in course)
- `GET /api/enrollments` (optional filters: `studentId`, `courseId`)
- `DELETE /api/enrollments/{id}` (unenroll)

#### POST /api/enrollments
```json
{
  "studentId": 1,
  "courseId": 1,
  "enrolledOn": "2025-01-10"
}
```

#### GET /api/enrollments (filter examples)
```
GET /api/enrollments?studentId=1
GET /api/enrollments?courseId=2
```

---

## Error Responses

The API returns consistent JSON errors and uses correct status codes.

Example:
```json
{
  "code": "conflict",
  "message": "Email already exists."
}
```

Common cases:
- Duplicate student email → `409 Conflict`
- Duplicate course code → `409 Conflict`
- Duplicate enrollment → `409 Conflict`
- Invalid input → `400 Bad Request`
- Not found → `404 Not Found`

---

## Global Exception Handling Middleware

A global exception middleware is included to:
- handle unexpected exceptions safely
- return a consistent JSON error response
- map common failures to appropriate HTTP status codes

This prevents raw stack traces from leaking and keeps responses predictable for clients.

---

## Tests

Run:
```bash
dotnet test
```

Examples covered:
- Duplicate Student Email → `409 Conflict`
- Duplicate Course Code (after normalization) → `409 Conflict`
- Duplicate Enrollment (same student + course) → `409 Conflict`

---

## CI (GitHub Actions)

A miJohn CI workflow runs on:
- every push to `main`
- every pull request to `main`

Steps:
- `dotnet restore`
- `dotnet build -c Release`
- `dotnet test -c Release`

---

## What I Learned / Skills Demonstrated

- Building REST APIs with correct HTTP behavior and status codes
- Layered architecture: Controllers → Services → Repositories
- EF Core modeling + migrations + SQLite
- DTO-based design + validation
- Business rule enforcement (uniqueness + duplicates)
- Pagination and filtering for list endpoints
- Global exception handling middleware
- Automated testing with xUnit
- CI pipeline setup with GitHub Actions

---

## Future Improvements (Optional)

- Pagination/filtering for Courses and Enrollments
- Sorting support (`sortBy`, `sortOrder`)
- Dockerfile for containerized run
- JWT Authentication & Authorization
- Soft delete support

---

## License

- This project is provided for learning and portfolio purposes. No license specified.

