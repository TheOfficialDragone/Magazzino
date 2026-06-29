# Magazzino

Warehouse management system built with **C# .NET Framework**, using a **WCF client-server architecture** and **MySQL** database. Developed by Rocco Carpi & Riccardo Versetti (October 2022).

## Architecture

```
┌─────────────────────┐        WCF / TCP        ┌─────────────────┐
│   Magazzino Client  │ ◄──────────────────────► │     Server      │
│  (Console App)      │                           │  (WCF Service)  │
│                     │                           │        │        │
│  - Admin            │                           │     MySQL DB    │
│  - Magazziniere     │                           └─────────────────┘
└─────────────────────┘
```

Two client roles communicate with the WCF server which handles all business logic and database operations.

## Features

### Admin
- View, add, edit, delete products
- Manage product categories
- View list of warehouse workers

### Magazziniere (Warehouse Worker)
- View product list
- Increase / decrease stock quantities

### Shared
- Role-based login with BCrypt password hashing
- User registration
- Password change

## Database Schema

| Table | Columns |
|-------|---------|
| `account` | ID, nome, cognome, email, password (BCrypt), indirizzo, data_nascita, telefono, TipoAccount (1=Admin, 2=Magazziniere) |
| `prodotto` | ID, nome, descrizione, prezzo, quantita, fk_categoria |
| `categoria` | ID, nome |

## Tech Stack

- **Language:** C# (.NET Framework)
- **Communication:** WCF (Windows Communication Foundation) over TCP
- **Database:** MySQL 8 via `MySql.Data`
- **Password hashing:** BCrypt.Net-Next
- **IDE:** Visual Studio

## Setup

### 1. Database

```bash
mysql -u root -p < init-db.sql
```

Default test accounts (see `Useres & Password/`):

| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@test.local` | `1` |
| Magazziniere | `magazziniere@test.local` | `test` |

### 2. Server

Open `Magazzino.sln` in Visual Studio, configure the connection string in `Server/App.config`, then run the **Server** project.

### 3. Client

With the server running, run the **Magazzino** project.

## Authors

- Rocco Carpi
- Riccardo Versetti
