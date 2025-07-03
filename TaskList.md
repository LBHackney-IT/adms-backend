
### Notes

- Unit tests should be created for all business logic, particularly for CQRS handlers.
- Remember to use `async` and `await` for all database and I/O operations.
- Dependency Injection is used to provide services and repositories to the controllers, configured in `Api/Program.cs`.
- The architecture follows Clean Architecture principles with a clear separation of concerns between `Domain`, `Application`, `Infrastructure`, and `API` layers.
- The CQRS pattern is implemented using separate models and logic for read (Queries) and write (Commands) operations.

---

## Tasks

- [X] **1.0 Project Setup & Configuration**
  - [X] 1.1 Create a new .NET 8 solution named `adms-backend`.
  - [X] 1.2 Create `API`, `Application`, `Domain`, and `Infrastructure` projects.
  - [X] 1.3 Add project references: `API` -> `Application`, `Application` -> `Domain`, `Infrastructure` -> `Application`.
  - [X] 1.4 Install necessary NuGet packages in the appropriate projects.
- [X] **2.0 Domain and Infrastructure Layer Setup**
  - [X] 2.1 In `Domain`, create entity classes `Apprentice`, `Transaction`, and `AuditLog`.
  - [X] 2.2 In `Infrastructure`, create `ApplicationDbContext` inheriting from `DbContext`.
  - [X] 2.3 Add `DbSet` properties to `ApplicationDbContext`.
  - [X] 2.4 Configure entity constraints in `ApplicationDbContext`.
  - [X] 2.5 Set up the database connection string in `appsettings.json`.
  - [X] 2.6 Create and apply the initial database migration.
- [X] **3.0 Implement Generics & CQRS**
  - [X] 3.1 In `Domain`, define generic `IReadRepository` and `IWriteRepository` interfaces.
  - [X] 3.2 In `Infrastructure`, implement separation of concerns.
- [X] **4.0 Implement Apprentice Data Ingestion (CQRS Command)**
  - [X] 4.1 The handler should process each record, checking if an apprentice exists by `ULN`.
  - [X] 4.2 Implement logic to either create a new `Apprentice` or update an existing one.
  - [X] 4.3 In `ApprenticesController`, create an endpoint `POST /api/apprentices/upload` that sends the command.
- [X] **5.0 Implement Transaction Data Ingestion (CQRS Command)**
  - [X] 5.1 Implement logic to create new `Transaction` entities for each record in the input.
  - [X] 5.2 In `TransactionsController`, create an endpoint `POST /api/transactions/upload` that sends the command.
- [X] **6.0 Develop Apprentice CRUD API Endpoints (CQRS)**
  - [X] 6.1 Implement `GetAllApprenticesQuery` to fetch all apprentices.
  - [X] 6.2 Implement `GetApprenticeByIdQuery` to retrieve a single apprentice.
  - [X] 6.3 Implement filtering logic within the `GetAllApprenticesQuery` handler for status, directorate, etc.
  - [X] 6.4 Implement `UpdateApprenticeCommand` for modifying an apprentice record.
  - [X] 6.5 Implement `DeleteApprenticeCommand` for removing an apprentice.
  - [X] 6.6 Create corresponding endpoints in `ApprenticesController` for the queries and commands.
- [X] **7.0 Develop Transaction CRUD API Endpoints (CQRS)**
  - [X] 7.1 Implement `GetAllTransactionsQuery` and `GetTransactionByIdQuery`.
  - [X] 7.2 Implement `UpdateTransaction` and `DeleteTransaction`.
  - [X] 7.3 Create corresponding endpoints in `TransactionsController`.
- [ ] **8.0 Implement Comprehensive Audit Logging**
  - [ ] 8.1 Create an `AuditService` to handle the creation of log entries.
  - [ ] 8.2 Log the start, completion, and errors of data ingestion processes.
- [ ] **9.0 Infrastructure as Code (IaC) and CI/CD Pipeline Setup**
  - [ ] 9.1 In the `terraform` directory, write Terraform scripts to provision AWS resources (RDS, Lambda, API Gateway).
  - [ ] 9.2 Create a `.circleci/config.yml` file for the CI/CD pipeline.
  - [ ] 9.3 Define CircleCI jobs for building, testing, and deploying the application.