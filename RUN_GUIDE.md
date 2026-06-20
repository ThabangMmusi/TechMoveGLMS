# TechMoveGLMS Execution Guide

This guide provides instructions on how to run the TechMoveGLMS applications (MVC Frontend and Web API Backend).

## Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQLite (included in the project)

## Running the Applications

### 1. Run the Web API (Backend)
The API handles data access and business logic.

```bash
cd TechMoveGLMS.API
dotnet run --urls "http://localhost:5001"
```
* **Swagger UI**: [http://localhost:5001/swagger](http://localhost:5001/swagger)
* **Base URL**: `http://localhost:5001/api`

### 2. Run the MVC Application (Frontend)
The MVC application provides the user interface.

```bash
cd TechMoveGLMS
dotnet run --urls "http://localhost:5000"
```
* **URL**: [http://localhost:5000](http://localhost:5000)

---

## API Endpoints Summary

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/contracts` | Get all contracts (supports `startDate`, `endDate`, `status` filters) |
| GET | `/api/contracts/{id}` | Get specific contract details |
| POST | `/api/contracts` | Create a new contract |
| PATCH | `/api/contracts/{id}/status` | Update contract status (Draft=0, Active=1, Expired=2, OnHold=3) |
| GET | `/api/clients` | Get all clients |
| POST | `/api/servicerequests` | Create service request (Blocked if contract is Expired/OnHold) |

## Troubleshooting
* **Database**: Ensure `TechMove.db` exists in the `TechMoveGLMS/` directory. The API is configured to look for it at `../TechMoveGLMS/TechMove.db`.
* **Ports**: If ports 5000 or 5001 are in use, you can change the `--urls` parameter in the run command.
