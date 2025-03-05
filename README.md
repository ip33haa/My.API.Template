<h1>ASP.NET Core Web API with Clean Architecture, CQRS, Mediatr, FluentValidation, and JWT Authentication</h1>
<p><strong>Description</strong></p>
    <p>This is a robust ASP.NET Core Web API project implementing <strong>Clean Architecture</strong>, <strong>CQRS</strong> (Command Query Responsibility Segregation), <strong>Mediatr</strong>, <strong>FluentValidation</strong>, <strong>Entity Framework Core</strong>, <strong>Auto Migration</strong>, <strong>JWT Authentication</strong>, <strong>Authorization</strong>, and <strong>Refresh Tokens</strong>. The solution is structured to promote separation of concerns, maintainability, and scalability.</p>
 <h2>Features:</h2>
    <ul>
        <li><strong>Clean Architecture</strong>: Organized project structure with separation of concerns.</li>
        <li><strong>CQRS</strong>: Command and Query models for better handling of requests.</li>
        <li><strong>Mediatr</strong>: Simplified communication between components.</li>
        <li><strong>FluentValidation</strong>: For input validation on requests.</li>
        <li><strong>Entity Framework Core</strong>: ORM for database interaction.</li>
        <li><strong>Auto Migration</strong>: Automatic database migration on application startup.</li>
        <li><strong>JWT Authentication</strong>: Secure user authentication with token-based system.</li>
        <li><strong>Authorization</strong>: Role-based authorization for API access control.</li>
        <li><strong>Refresh Tokens</strong>: Implementation of refresh tokens for continuous authentication without re-login.</li>
    </ul>
 <h2>Technologies Used</h2>
    <ul>
        <li>ASP.NET Core 8 Web API</li>
        <li>Clean Architecture</li>
        <li>Mediatr</li>
        <li>FluentValidation</li>
        <li>Entity Framework Core</li>
        <li>JWT Authentication</li>
        <li>Refresh Tokens</li>
        <li>SQL Server (or any preferred database)</li>
    </ul>
<h2>Getting Started</h2>
<h3>Prerequisites</h3>
    <ul>
        <li><a href="https://dotnet.microsoft.com/download/dotnet">.NET SDK 8.0</a></li>
        <li><a href="https://www.microsoft.com/en-us/sql-server/sql-server-downloads">SQL Server</a> or any database system you plan to use.</li>
        <li><a href="https://www.postman.com/downloads/">Postman</a> (or any API testing tool)</li>
    </ul>
<h3>Installation</h3>
    <ol>
        <li>Clone the repository:
            <pre><code>[git clone https://github.com/ip33haa/API-Template.git]</code></pre>
        </li>
        <li>Navigate to the project directory:
            <pre><code>cd API-Template</code></pre>
        </li>
        <li>Create a <strong>appsettings.json</strong> file (if not included) in the root directory of the API and add your database connection string:
            <pre><code>
{
  "ConnectionStrings": {
    "DefaultConnection": "YourDatabaseConnectionString"
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKey",
    "Issuer": "YourIssuer",
    "Audience": "YourAudience",
    "ExpirationInMinutes": 30
  }
}
            </code></pre>
        </li>
        <li>Database migrations will automatically run:
        </li>
        <li>Start the application:
            <pre><code>dotnet run</code></pre>
        </li>
        <li>Your API will be available at <strong>https://localhost:5001</strong> (or <strong>http://localhost:5000</strong> for non-SSL).</li>
    </ol>
