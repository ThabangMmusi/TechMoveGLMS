***

# ✅ **GLOBAL LOGISTICS MANAGEMENT SYSTEM (GLMS) – FULL INSTRUCTIONS**

***

# **Background**

**TechMove Logistics** is a global shipping coordinator managing:

* International freight contracts
* Driver schedules
* Service invoices

Current system:

* Disjointed spreadsheets
* Emails
* Manual phone calls

### Problems:

* Data fragmentation
* Lost invoices
* Compliance failures (expired contracts)
* Workflow bottlenecks

### Project:

Build an **Enterprise-Grade Global Logistics Management System (GLMS)** that:

* Handles complex relationships (clients, contracts, service requests)
* Is scalable and highly available

***

# **Objective**

Develop a **scalable, distributed enterprise web platform**:

* Use industry-standard architecture
* Build a working prototype
* Implement automated testing
* Deploy using **Docker (containerised environment)**

***

# ✅ **PART 2 — CORE PROTOTYPE & UNIT TESTING (Marks: 100)**

## **Learning Units**

* LU3: Enterprise Software System Development
* LU4: Optimising Performance (Async/Await)
* QA: Test-Driven Development (Unit Testing)

***

## **Task (Monolithic System)**

Build a prototype using **ASP.NET Core MVC** as a **Monolith**:

* Single unified system
* UI + Database + Business logic tightly coupled
* Focus: core functionality

***

## **1. Database & Complex Models**

### Use:

* SQL Server
* Entity Framework Core

### Entities Required:

1. **Client**
   * Name
   * Contact Details
   * Region

2. **Contract**
   * Linked to Client
   * StartDate
   * EndDate
   * Status (Draft / Active / Expired / On Hold)
   * ServiceLevel

3. **ServiceRequest**
   * Linked to Contract
   * Description
   * Cost
   * Status

***

### **File Handling**

* Upload PDF **“Signed Agreement”** per contract
* Store on server (simulate file server)
* Must be downloadable via UI

***

## **2. Workflow Logic**

* Cannot create ServiceRequest if Contract is:
  * Expired
  * On Hold

* Implement search/filter (LINQ):
  * Filter contracts by Date Range
  * Filter by Status

***

## **3. External API Integration**

* TechMove works in USD/EUR but reports in ZAR

### Requirement:

* Use a **currency exchange API** (e.g., ExchangeRate API)

### Implementation:

* Use **HttpClient**
* Convert USD → ZAR
* Auto-calculate cost when user enters USD

***

## **4. Unit Testing (Quality Assurance)**

### Requirements:

* Create **separate Test Project**
* Use:
  * xUnit OR NUnit

### What to Test:

* Currency calculation correctness
* File validation (.exe rejected, only .pdf allowed)
* Other business logic

### Evidence:

* Screenshots of passing tests in **Test Explorer**

***

## **Key Features of Platform**

1. **Contract Management**
   * Central storage (agreements, SLAs)
   * Status tracking

2. **Service Requests**
   * Linked to contracts
   * Only valid contracts allowed

3. **Financial Integration**
   * Currency conversion via API

4. **Architecture**
   * Must later evolve to microservices

***

# ✅ **PART 3 — MODERNISATION, DOCKER & AUTOMATED TESTING (Marks: 100)**

## **Learning Units**

* All Learning Units
* QA: Automated Integration Testing

***

## **Task**

Convert the monolithic system into:

* **Service-Oriented Architecture (SOA)**
* **Containerised (Cloud-native)**
* Add automated integration tests

***

## **1. Refactoring: Web API (Backend)**

### Steps:

* Move business logic out of MVC Controllers
* Create new **ASP.NET Core Web API project**
* API handles database access ONLY

### API Endpoints:

* `GET /api/contracts` (with filtering)
* `POST /api/contracts` (create)
* `PATCH /api/contracts/{id}/status` (approve/decline)
* Add other endpoints as needed

### Other Requirements:

* Enable Swagger / OpenAPI
* Return JSON responses
* Reuse business logic code

***

## **2. Refactoring: MVC Client (Frontend)**

### Changes:

* MVC must NOT connect directly to database
* Use **HttpClient** to call API

### Purpose:

* Separate:
  * Presentation Layer
  * Service Layer

### Requirement:

* Implement authentication:
  * JWT / Firebase / similar

***

## **3. Automated API Integration Testing**

### Task:

* Expand Test Project to include integration tests

### Example Test:

* Call API endpoint
* Check:
  * Status code = 200
  * Response JSON not null

### Purpose:

* Run automatically in DevOps pipeline
* Prevent breaking changes

***

## **4. Full Containerization (Docker Compose)**

### Create:

`docker-compose.yml`

### Containers:

1. **sql-server-db**
   * Database

2. **glms-backend-api**
   * Web API (connects to DB)

3. **glms-frontend-web**
   * MVC App (connects to API)

### Networking:

* Containers must communicate via Docker network

***

## **5. Technical Reflection Report**

### Topics:

1. **DevOps & Testing**
   * Why automated testing is critical
   * How it prevents bugs reaching production

2. **Containerization**
   * How Docker ensures consistency across environments
   * Solves “works on my machine” problem

***

# ✅ **Submission Guidelines**

You must submit:

* ✅ Full Source Code (MVC + API in one solution on GitHub)
* ✅ Dockerfile (API + Web App)
* ✅ docker-compose.yml
* ❌ No GitHub = **–5% penalty**
* ✅ Screenshots of system running in Docker
* ✅ PDF of Technical Reflection
* ✅ Full video demo:
  * Show system running
  * Explain application flow

***
