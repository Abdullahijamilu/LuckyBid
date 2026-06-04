# Lucky Bid

Lucky Bid is an online auction platform built with a modern tech stack, featuring a clean architecture backend and a responsive frontend.


## 🛠️ Technology Stack
- **Backend:** .NET 8 Web API (Clean Architecture: Api, Application, Domain, Infrastructure)
- **Frontend:** Angular 17 + Tailwind CSS

## 💻 Getting Started (Local Development)

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [Angular CLI](https://angular.io/cli) (v17+)
- SQL Server (or update connection string for another database)

### Running the Backend (API)
1. Navigate to the API folder:
   ```bash
   cd LuckyBid.Api
   ```
2. Update `appsettings.json` with your database connection string if necessary.
3. Apply database migrations:
   ```bash
   dotnet ef database update --project ../LuckyBid.Infrastructure --startup-project .
   ```
4. Run the API:
   ```bash
   dotnet run
   ```
   The API will typically be available at `https://localhost:5001` or `http://localhost:5000` (check terminal output for exact URL).

### Running the Frontend
1. Navigate to the Frontend folder:
   ```bash
   cd LuckyBid.Frontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   npm start
   ```
4. Open your browser and navigate to: [http://localhost:4200](http://localhost:4200)

## 📁 Project Structure
- `LuckyBid.Api`: The entry point, controllers, and API configurations.
- `LuckyBid.Application`: Business logic, interfaces, and CQRS handlers.
- `LuckyBid.Domain`: Core entities, domain models, and exceptions.
- `LuckyBid.Infrastructure`: Database context, repositories, and external services integrations.
- `LuckyBid.Frontend`: The Angular application.
