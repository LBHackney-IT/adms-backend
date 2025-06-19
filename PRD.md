# Product Requirements Document: Apprenticeship Data Management System (ADMS) Backend

## 1. Introduction/Overview

This document specifies the backend requirements for the Apprenticeship Data Management System (ADMS). The primary objective of this system is to replace the current inefficient, spreadsheet-based data management protocol with a robust, centralized backend, featuring a PostgreSQL database and a .NET API.

This change addresses the key business need to maintain an extended history of apprentice and financial data, overcoming the one-year export limitation of the current source. The backend will be responsible for intelligently processing data ingestion, handling data validation, managing data reconciliation (updates and insertions), and storing the information in a structured database.

The core goal is to create a single, authoritative source for apprentice data that reduces manual workload, improves data accuracy and consistency, and provides a comprehensive historical record.

## 2. Goals

*   **Data Centralization:** Establish a unified PostgreSQL database as the single source of truth for all apprentice and financial transaction data, eliminating data silos.
*   **Efficient Data Ingestion:** Implement a robust mechanism for users to submit apprentice and transaction data via CSV file uploads.
*   **Intelligent Data Processing:** Develop backend logic to parse, validate, and transform data from submitted CSV files, correctly identifying records for either creation (INSERT) or update (UPDATE) operations.
*   **Comprehensive Data Management:** Expose a secure and efficient .NET API that permits authorized users to perform Create, Read, Update, and Delete (CRUD) operations on all data.
*   **Improve Operational Efficiency:** Significantly reduce the manual effort and time required for data entry, reconciliation, and reporting tasks.
*   **Enhance Data Integrity:** Improve data accuracy and consistency through centralized governance, enforced validation rules, and structured storage.
*   **Extend Historical Data:** Overcome the one-year data export limitation by storing all uploaded data indefinitely, building a comprehensive historical archive.

## 3. User Stories

*   **US1:** As an apprenticeship team member, I want to retrieve specific apprentice information swiftly to respond to inquiries efficiently.
*   **US2:** As an apprenticeship team member, I want to extract a comprehensive list of all currently active apprentices so that I can generate requisite reports.
*   **US3:** As an apprenticeship team member, I want to upload a CSV file containing apprentice data, so that the system can automatically add new apprentice records and update existing ones.
*   **US4:** As an apprenticeship team member, I want to upload a CSV file containing financial transaction data, so that new transactions are systematically added and existing ones are updated as necessary.
*   **US5:** As an apprenticeship team member, I need the ability to view, add, edit, and delete individual apprentice records through a user interface to perform ad-hoc data corrections.
*   **US6:** As an apprenticeship team member, I want to be able to search, filter, view, add, edit, and delete individual financial transaction records through the UI, so that our financial data can be managed with precision.
*   **US7:** As an apprenticeship team member, I need access to activity logs for all data ingestion events so that I can monitor data flow and effectively troubleshoot any emergent issues.

## 4. Functional Requirements

1.  **FR1: Centralized Database:** The system shall utilize a PostgreSQL database. The schema must adhere to the specifications in the `Data Dictionary` and `dataTables` documents.
2.  **FR2: CSV Upload Endpoints:** The backend API shall provide secure, dedicated endpoints for the submission of `apprentice_data.csv` and `transaction_data.csv` files.
3.  **FR3: CSV Parsing:** The system must be able to accurately parse and interpret all data contained within the uploaded CSV files.
4.  **FR4: Data Validation & Error Handling:** All ingested data must be validated against the data schema (e.g., data types, mandatory fields, enumerated values). If a file contains invalid rows, the system shall log the error details for each invalid row and skip them, while continuing to process all valid rows in the file.
5.  **FR5: Data Transformation:** The system is required to transform the ingested data from its source CSV format into the relational format mandated by the database schema.
6.  **FR6: Apprentice Data Reconciliation (Upsert Logic):**
    *   The system must use the **ULN (Unique Learner Number)** as the unique identifier to check if an apprentice record exists.
    *   If the ULN does not exist in the database, a new record must be created.
    *   If the ULN exists, the existing record must be updated with the information provided in the CSV file. The old data will be overwritten.
7.  **FR7: Apprentice CRUD API:** The API must provide endpoints for Create, Read (for both individual records and lists), Update, and Delete operations for apprentice records.
8.  **FR8: Transaction CRUD API:** The API must provide endpoints for Create, Read (for both individual records and lists), Update, and Delete operations for financial transaction records.
9. **FR9: Data Querying and Filtering:** API endpoints that return lists of records must support server-side filtering.
    *   For **Apprentices**, filtering must be supported by "Status", "Directorate code", "Program", and "Start Date".
    *   For **Transactions**, filtering must be supported by "Transaction date" and "Description".
10. **FR10: Comprehensive Logging:** The system shall produce logs for each data ingestion process, detailing metrics such as the source filename, rows processed, rows inserted, rows updated, and any errors encountered.
11. **FR11: Security:** The API must be secured to ensure that all data modification operations (CRUD) and file uploads are restricted to authenticated users. For this initial implementation, all authenticated "apprenticeship team members" will have the same permissions.
12. **FR12: Transaction Data Reconciliation (Upsert Logic):**
*   The system will assume that all provided data are unique transactions and record them accordingly.

## 5. Non-Goals (Out of Scope)

*   Direct, automated API integration with the Department of Education or any other external data service.
*   The development of in-app graphical reporting functionalities or data visualization dashboards.
*   The creation of agentic workflows for automated report generation or intelligent data retrieval.
*   Automated data ingestion from Google Sheets.

## 6. Design Considerations

*   **Data Schema:** The database architecture must be modeled on the specifications within the provided `dataTables`, `Data Dictionary`, and visual data model documents. These will serve as the blueprint for all database tables and relationships.

## 7. Technical Considerations

*   **Backend Stack:** The API will be developed using .NET 8 and hosted on AWS.
*   **Database:** The persistence layer will be a PostgreSQL database, hosted on AWS RDS.
*   **Unique Identifiers:**
    *   **Apprentice Data:** The `ULN` (Unique Learner Number) field is the primary key for record reconciliation.
*   **Infrastructure as Code:** All AWS infrastructure components will be provisioned and managed using Terraform.
*   **Testing:** Unit tests are required for all critical business logic (CSV parsing, validation, reconciliation). Integration tests are required to verify the functionality of all API endpoints and their interaction with the database.

## 8. Success Metrics

*   Attainment of a 100% success rate in the processing of valid, correctly formatted CSV file submissions.
*   A quantifiable reduction in the time and manual effort expended by staff on data entry and reconciliation tasks.
*   Confirmation that authorized staff can successfully execute all CRUD operations on both apprentice and transaction data via the frontend application.
*   The generation of complete and verifiably accurate logs for all data ingestion and modification operations.
*   A significant and measurable decrease in data errors and inconsistencies when compared to the legacy spreadsheet-based system.

## 9. Open Questions

*   **Historical Data Management:** The decision has been made to overwrite existing data on update. This simplifies the logic but means no audit trail of changes is kept in the database. We should confirm this is an acceptable long-term trade-off.
*   **Performance Benchmarks:** Are there any performance requirements, such as the expected maximum size of the CSV files (e.g., number of rows) or the maximum acceptable processing time for a file upload?
