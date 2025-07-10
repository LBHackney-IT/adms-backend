# Development Tasks for: Apprenticeship Data Management System (ADMS) Backend

### Notes
*   Unit tests should be created for all business logic, particularly for CQRS handlers, and placed alongside the code files they are testing.
*   All new and modified code must have a minimum of **20% unit test coverage**.
*   The architecture follows Clean Architecture principles (`Domain`, `Application`, `Infrastructure`, `API`).
*   The CQRS pattern is used to separate read (Queries) and write (Commands) operations.
*   Dependency Injection is used to provide services and repositories to the controllers, configured in `Api/Program.cs`.
*   Remember to use `async` and `await` for all database and I/O operations.

## Tasks

- [X] **1.0 Project Setup & Configuration**
  - [X] 1.1 Create a new .NET 8 solution named `adms-backend`.
  - [X] 1.2 Create the following projects: `API`, `Application`, `Domain`, and `Infrastructure`.
  - [X] 1.3 Add project references: `API` → `Application`, `Infrastructure` → `Application` → `Domain`.
  - [X] 1.4 Install necessary NuGet packages (e.g., `Microsoft.EntityFrameworkCore.PostgreSQL`) in the appropriate projects.

- [X] **2.0 Domain and Infrastructure Layer Setup**
  - [X] 2.1 In the `Domain` project, create the entity classes: `Apprentice`, `Transaction`, and `AuditLog`, based on the specifications in the Data Dictionary.
  - [X] 2.2 In the `Infrastructure` project, create `ApplicationDbContext` inheriting from `DbContext`.
  - [X] 2.3 Add `DbSet<>` properties for `Apprentices`, `Transactions`, and `AuditLogs` to the `ApplicationDbContext`.
  - [X] 2.4 In `ApplicationDbContext`, configure entity constraints (e.g., primary keys, required fields, unique constraints for `ULN`) as specified in the Data Dictionary.
  - [X] 2.5 Set up the database connection string for PostgreSQL in `API/appsettings.json`.
  - [X] 2.6 Create the initial database migration and apply it to the database.

- [X] **3.0 Implement Core CQRS Pattern & Generic Repositories**
  - [X] 3.1 In the `Domain` project, define generic `IReadRepository` and `IWriteRepository` interfaces.
  - [X] 3.2 In the `Infrastructure` project, implement the generic repository classes.
  - [X] 3.3 Configure the repository services in `API/Program.cs` for dependency injection.

- [X] **4.0 Develop Apprentice CRUD API Endpoints (US1, US5, FR8, FR10)**
  - [X] 4.1 Implement `GetAllApprenticesQuery` to fetch all apprentices, including server-side filtering by "Status", "Directorate code", "Program", and "Start Date" (FR10).
  - [X] 4.2 Implement `GetApprenticeByUlnQuery` to retrieve a single apprentice by their Unique Learner Number.
  - [X] 4.3 Implement `UpdateApprenticeCommand` and `DeleteApprenticeCommand` for modifying and removing apprentice records.
  - [X] 4.4 Add corresponding `GET`, `PUT`, `DELETE` endpoints to the `ApprenticesController`.
  - [X] 4.5 Write unit tests for all new queries and command handlers.

- [X] **5.0 Develop Transaction CRUD API Endpoints (US6, FR9, FR10)**
  - [X] 5.1 Implement `GetAllTransactionsQuery` with server-side filtering by "Transaction date" and "Description" (FR10).
  - [X] 5.2 Implement `GetTransactionByIdQuery` to retrieve a single transaction.
  - [X] 5.3 Implement `UpdateTransactionCommand` and `DeleteTransactionCommand`.
  - [X] 5.4 Add corresponding `GET`, `PUT`, `DELETE` endpoints to the `TransactionsController`.
  - [X] 5.5 Write unit tests for the new transaction-related queries and command handlers.

- [ ] **6.0 Implement Comprehensive Audit Logging (US7, FR11)**
  - [ ] 6.1 Create an `AuditService` to handle the creation of log entries in the `AuditLogs` table.
  - [ ] 6.2 Integrate the `AuditService` into the create, update, and delete command handlers for Apprentices and Transactions.
  - [ ] 6.3 Log all data modification events (creations, updates, deletions), including the entity changed, the user performing the action, and the timestamp.
  - [ ] 6.4 Write unit tests for the `AuditService` to ensure logs are created correctly.

- [ ] **7.0 Secure API Endpoints (FR12)**
  - [ ] 7.1 Implement authentication to ensure all API endpoints are protected.
  - [ ] 7.2 Add authorization policies to restrict all data modification operations (CRUD) and file uploads to authenticated "apprenticeship team members".
  - [ ] 7.3 Write integration tests to verify that unauthorized requests are rejected.

- [ ] **8.0 Infrastructure as Code (IaC) and CI/CD Pipeline Setup**
  - [ ] 8.1 In a `terraform` directory, write Terraform scripts to provision AWS resources (e.g., RDS for PostgreSQL, Lambda/ECS for hosting, API Gateway).
  - [ ] 8.2 Create a `.circleci/config.yml` file for the CI/CD pipeline.
  - [ ] 8.3 Define CircleCI jobs for building the .NET solution, running tests, and deploying the application to AWS. *(Note: This may require collaboration with a senior engineer.)*

### Implementation Notes
*   Focus on the "how." The PRD defines "what" and "why"; this task list defines "how" it gets built.
*   The `ULN` (Unique Learner Number) is the key for apprentice data reconciliation. Ensure all database operations and business logic use it correctly.