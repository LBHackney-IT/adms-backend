# Product Requirements Document: Apprenticeship Data Management System (ADMS) Backend

## 1. Introduction and Overview
*   **Problem Statement**: The Apprenticeship Department currently relies on a manual, spreadsheet-based system for managing apprentice and financial data. This process is inefficient, prone to errors, leads to data inconsistencies, and is limited by a one-year data export cap. This creates a significant administrative burden and prevents effective long-term data analysis.
*   **Proposed Solution**: This project will build a robust, centralized backend system to replace the current spreadsheet protocol. The system will feature a PostgreSQL database to serve as a single source of truth and a .NET 8 API to handle all data operations. The core goal is to automate data ingestion from a JSON-based format, ensure data integrity through validation and reconciliation, and provide a secure, comprehensive historical record of all apprentice data.

## 2. Stakeholders and Target Audience
*   **Stakeholders**: Product Manager, Engineering Lead, Apprenticeship Team Members.
*   **Target User**: The primary users are **Apprenticeship Team Members**. They require a reliable and efficient system to manage apprentice records, track financial transactions, and respond to inquiries without the manual effort of cross-referencing multiple spreadsheets.

## 3. Goals and Success Metrics
*   **Business/Product Goals**:
    *   Establish a unified PostgreSQL database as the single source of truth for all apprentice and financial data.
    *   Significantly reduce the manual effort and time required for data entry, reconciliation, and reporting.
    *   Improve data accuracy and consistency through centralized governance and enforced validation rules.
    *   Overcome the one-year data export limitation by storing all uploaded data indefinitely.
*   **Success Metrics (KPIs)**:
    *   Attainment of a 100% success rate in the processing of valid, correctly formatted JSON data submissions.
    *   A quantifiable reduction in the time and manual effort expended by staff on data entry and reconciliation tasks.
    *   A significant and measurable decrease in data errors compared to the legacy system.
    *   Generation of complete and verifiable logs for all data ingestion and modification operations.

## 4. User Stories
*   **US1:** As an apprenticeship team member, I want to retrieve specific apprentice information swiftly to respond to inquiries efficiently.
*   **US2:** As an apprenticeship team member, I want to extract a comprehensive list of all currently active apprentices so that I can generate requisite reports.
*   **US3:** As an apprenticeship team member, I want to bulk-upload apprentice data, so that the system can automatically add new apprentice records and update existing ones.
*   **US4:** As an apprenticeship team member, I want to bulk-upload financial transaction data, so that new transactions are systematically added.
*   **US5:** As an apprenticeship team member, I need the ability to view, add, edit, and delete individual apprentice records through a user interface to perform ad-hoc data corrections.
*   **US6:** As an apprenticeship team member, I want to be able to search, filter, view, add, edit, and delete individual financial transaction records through the UI, so that our financial data can be managed with precision.
*   **US7:** As an apprenticeship team member, I need access to activity logs for all data ingestion events so that I can monitor data flow and effectively troubleshoot any emergent issues.

## 5. Prioritization
*   **Must-Have**:
    *   Core database schema implementation (Apprentices, Transactions, AuditLogs).
    *   Secure JSON ingestion endpoints for apprentice and transaction data (US3, US4).
    *   Backend logic to parse, validate, and transform JSON data.
    *   Apprentice data reconciliation logic (Upsert using ULN).
    *   CRUD APIs for both Apprentice and Transaction records (US1, US5, US6).
    *   Comprehensive logging for all data ingestion activities (US7).
*   **Should-Have**:
    *   Advanced server-side filtering capabilities for all list endpoints (e.g., by Status, Date, Program).
*   **Could-Have**:
    *   Performance optimization for very large JSON payload processing.
*   **Won't-Have (in this release)**:
    *   Direct, automated API integration with the Department of Education.
    *   Automated data ingestion from Google Sheets.
    *   In-app graphical reporting or data visualization dashboards.

## 6. Functional Requirements
1.  **FR1: Centralized Database**: The system shall utilize a PostgreSQL database. The schema must adhere to the specifications in the provided Data Dictionary and data model documents.
2.  **FR2: JSON Ingestion Endpoints**: The backend API shall provide secure, dedicated endpoints for submitting JSON arrays of `apprentice` and `transaction` data.
3.  **FR3: Data Validation & Error Handling**: All ingested JSON data must be validated against the database schema (data types, mandatory fields, enums). If the submitted JSON array contains invalid objects, the system shall log the error details for each invalid object and skip them, while continuing to process all valid objects.
4.  **FR4: Data Transformation**: The system is required to transform the ingested data from the JSON format into the relational format of the database schema.
5.  **FR5: Apprentice Data Reconciliation (Upsert Logic)**: The system must use the **ULN (Unique Learner Number)** as the unique identifier. If the ULN does not exist in the database, a new apprentice record must be created. If the ULN exists, the existing record must be updated with the new information.
6.  **FR6: Transaction Data Ingestion**: The system will assume that all provided transaction data are unique and record them accordingly without updates.
7.  **FR7: Apprentice CRUD API**: The API must provide endpoints for Create, Read (for individual records and lists), Update, and Delete operations for apprentice records.
8.  **FR8: Transaction CRUD API**: The API must provide endpoints for Create, Read (for individual records and lists), Update, and Delete operations for financial records.
9.  **FR9: Data Querying and Filtering**: API endpoints that return lists must support server-side filtering.
    *   For **Apprentices**, filtering must be supported by "Status", "Directorate code", "Program", and "Start Date".
    *   For **Transactions**, filtering must be supported by "Transaction date" and "Description".
10. **FR10: Comprehensive Logging**: The system shall produce logs for each data ingestion process in the `AuditLogs` table, detailing metrics such as records processed, records inserted/updated, and any errors encountered.
11. **FR11: Security**: The API must be secured to ensure that all data modification operations (CRUD) and data ingestion are restricted to authenticated "apprenticeship team members".

## 7. Non-Functional Requirements
*   **Performance**: The API should respond to all requests within a reasonable time frame, even under moderate load. JSON payload processing time should be acceptable for the expected number of records per request.
*   **Security**: All API endpoints must be protected and require user authentication. Sensitive data should be handled according to security best practices.
*   **Reliability**: The system should be highly available and include robust error handling to prevent data loss or corruption during data ingestion.

## 8. Acceptance Criteria
*Example for Requirement FR5 (Apprentice Data Reconciliation):*

*   **GIVEN** the system receives a POST request with a JSON array of apprentice data
*   **AND** the array contains a record for an apprentice with a ULN that does *not* exist in the database
*   **WHEN** the system processes the request
*   **THEN** a new apprentice record is created in the `Apprentices` table with the provided data.

---
*   **GIVEN** the system receives a POST request with a JSON array of apprentice data
*   **AND** the array contains a record for an apprentice with a ULN that *already exists* in the database
*   **WHEN** the system processes the request
*   **THEN** the existing apprentice record in the `Apprentices` table is updated with the new information from the JSON record.

## 9. Non-Goals (Out of Scope)
*   Direct, automated API integration with the Department of Education or any other external data service.
*   The development of in-app graphical reporting functionalities or data visualization dashboards.
*   The creation of agentic workflows for automated report generation.
*   Automated data ingestion from Google Sheets.

## 10. Design and UI/UX
*   This document covers the backend API and data architecture. No UI/UX is being designed or implemented as part of this backend-specific PRD.
*   The database architecture must be modeled on the specifications within the provided `dataTables`, `Data Dictionary`, and visual data model documents.

## 11. Technical Considerations
*   **Backend Stack**: The API will be developed using **.NET 8**.
*   **Database**: The persistence layer will be a **PostgreSQL** database, hosted on **AWS RDS**.
*   **Hosting**: The entire backend system will be hosted on **AWS**.
*   **Infrastructure as Code**: All AWS infrastructure components will be provisioned and managed using **Terraform**.
*   **Unique Identifiers**: The `ULN`(Unique Learner Number) field is the key for apprentice record reconciliation. Primary keys for all tables will be UUIDs as defined in the Data Dictionary.

## 12. Assumptions, Constraints, and Dependencies
*   **Assumptions**:
    *   Users will be authenticated before they can access any API endpoints.
    *   The frontend application will handle the conversion and initial sanitization of any uploaded files (e.g., CSV) into a valid JSON array structure.
*   **Constraints**:
    *   The solution must be built using the specified technology stack (.NET 8, PostgreSQL, AWS).

## 13. Risk Assessment
*   **Technical Risks**:
    *   *Risk*: The data reconciliation (upsert) logic could have bugs, leading to data corruption or duplication.
    *   *Mitigation*: Implement comprehensive unit and integration tests specifically covering all cases for the upsert logic (create, update).
*   **Dependency Risks**:
    *   *Risk*: The frontend application may be delayed, preventing end-to-end testing and user feedback.
    *   *Mitigation*: Use tools like Postman or Swagger to test the API endpoints independently of any frontend client.
    *   *Risk*: The frontend application may send malformed, unsanitized, or structurally incorrect JSON, leading to ingestion failures.
    *   *Mitigation*: Implement robust validation on the backend for all incoming JSON data. Establish and share a clear data contract (e.g., using OpenAPI/Swagger) between the frontend and backend.

## 14. Testing Strategy
*   **Unit Testing**: Unit tests are required for all critical business logic, especially data validation and the apprentice reconciliation (upsert) logic.
*   **Integration Testing**: Integration tests are required to verify the functionality of all API endpoints, including JSON ingestion and CRUD operations, ensuring proper interaction with the database.

## 15. Glossary
*   **ADMS**: Apprenticeship Data Management System.
*   **API**: Application Programming Interface.
*   **CRUD**: Create, Read, Update, Delete.
*   **PRD**: Product Requirements Document.
*   **ULN**: Unique Learner Number. A unique identifier for each apprentice.
*   **Upsert**: An operation that will UPDATE an existing record if a specified value exists, and INSERT a new record if it does not.
*   **JSON**: JavaScript Object Notation. A lightweight data-interchange format.

## 16. Open Questions
1.  **Historical Data Management**: The current decision is to overwrite existing apprentice data on update. This simplifies the logic but means no audit trail of changes is kept in the database. We should confirm if this is an acceptable long-term trade-off or if a full history of changes needs to be preserved.
2.  **Performance Benchmarks**: Are there any specific performance requirements, such as the expected maximum number of records in a single JSON payload or the maximum acceptable processing time for an ingestion request? This will inform decisions on asynchronous processing and scalability.