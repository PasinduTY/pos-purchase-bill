# Purchase Bill — Full Stack Assignment

A full-stack application demonstrating login authentication (via an external POS API) and a Purchase Bill entry form, built with Angular and ASP.NET Core.

## Tech Stack

- **Frontend:** Angular (standalone components), TypeScript, Reactive Forms
- **Backend:** ASP.NET Core 8 Web API, C#, Entity Framework Core
- **Database:** SQL Server

## Project Structure

```
pos-purchase-bill/
├── backend/                    # ASP.NET Core Web API
├── frontend/
│   └── purchase-bill-frontend/ # Angular application
└── database/
    └── PurchaseBillDb.sql      # Database schema script
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18+) and npm
- [Angular CLI](https://angular.io/cli): `npm install -g @angular/cli`
- SQL Server (Express or higher) + SQL Server Management Studio (SSMS)
- Visual Studio 2022 (recommended for backend) or any C# IDE
- VS Code (recommended for frontend) or any editor

## 1. Database Setup

1. Open SQL Server Management Studio (SSMS) and connect to your local SQL Server instance.
2. Open and run `database/PurchaseBillDb.sql` — this creates the `PurchaseBillDb` database and the `Location_Details` table.

Alternatively, you can let the backend create the database via EF Core migrations (see Step 2.3 below) instead of running the script manually — either approach produces the same schema.

## 2. Backend Setup

```bash
cd backend/PurchaseBillApi
```

### 2.1 Configure application settings

Copy the example config file and fill in your local values:

```bash
cp appsettings.Example.json appsettings.Development.json
```

Edit `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PurchaseBillDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "SigningKey": "REPLACE_WITH_YOUR_OWN_LONG_RANDOM_SECRET_KEY"
  }
}
```

- Replace `YOUR_SERVER_NAME` with your SQL Server instance name (e.g. `localhost\SQLEXPRESS`).
- Replace the `SigningKey` with any random string, 32+ characters long (used to sign JWT tokens).

### 2.2 Restore packages

```bash
dotnet restore
```

### 2.3 (Optional) Apply EF Core migrations

If you didn't run the `.sql` script manually in Step 1, apply migrations instead:

```bash
dotnet ef database update
```

### 2.4 Run the backend

```bash
dotnet run
```

Or open the solution in Visual Studio and press F5.

The API will start at `https://localhost:7021` (confirm the exact port in `Properties/launchSettings.json`). Swagger UI is available at the root URL: `https://localhost:7021/`.

## 3. Frontend Setup

```bash
cd frontend/purchase-bill-frontend
npm install
ng serve
```

The application will be available at `http://localhost:4200`.

> **Note:** If your backend runs on a different port than `7021`, update the `apiUrl` in `src/environments/environment.development.ts` accordingly.

## 4. Using the Application

1. Navigate to `http://localhost:4200` — you'll be redirected to the login page.
2. Log in using valid POS system credentials (email and password).
3. On successful login, your account's locations are fetched from the external POS API and saved to the `Location_Details` table.
4. You'll be redirected to the Purchase Bill page, where you can:
   - Select an item (autocomplete) and batch (populated from your saved locations)
   - Enter cost/price/quantity/discount — totals calculate live
   - Click **Add** to add the item to the bill
   - Click **Submit Purchase Bill** to send the completed bill to the backend

## Architecture Notes

- **Authentication:** JWT-based. The backend acts as a proxy to the external POS API — the Angular app never calls the POS API directly.
- **Route protection:** An Angular route guard blocks access to `/purchase-bill` without a valid session; the backend independently enforces this via `[Authorize]` on protected endpoints (the real security boundary).
- **Validation:** Purchase Bill calculations are recomputed and validated server-side, not trusted from client input alone.

## Known Limitations / Scope Notes

- Purchase Bill submissions are validated server-side but not persisted to the database — the assignment specifies persistence only for `Location_Details`. The submit endpoint validates and acknowledges receipt.
- Token refresh is not implemented; JWTs expire after 60 minutes, after which the user must log in again.
