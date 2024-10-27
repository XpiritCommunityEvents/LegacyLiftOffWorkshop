# Lab XX: Create the Blazor application

First, scaffold a new Blazor application. Later in the labs, we want to leverage the automatic interactivity mode that allows us to seamlessly switch between Server or Client side rendering. Therefore, we use the `blazor` template with the `auto` interactivity mode.

- In the Terminal in your Codespace, type:

```bash
dotnet new blazor -int auto -o code/start/HmsBlazor
```

Now, we will start the migration. In this first iteration, we will reuse the existing `DataAccess` logic from the Forms application.

- Copy the `DataAccess.cs` file from the `code/start/legacy/Project_HMS/Project_HMS` folder into your new Blazor app (in `code/start/HmsBlazor/HmsBlazor`). If you like, you can change the namespace the class is in, or leave it as is. For this lab, we assume that the fully qualified class name will remain `Project_HMS.DataAccess`.
- For interacting with SQL Server, the old code uses the `System.Data.SqlClient` library. This has been replaced by Microsoft by a more modern implementation called `Microsoft.Data.SqlClient` which comes as a Nuget package. We need to install it:

```bash
dotnet add code/start/HmsBlazor/HmsBlazor/HmsBlazor.csproj package Microsoft.Data.SqlClient
```

- Now we just need to change 1 `using` statement at the top of the `DataAccess.cs` file that you copied:

~~`using System.Data.SqlClient`~~

Now becomes:

`using Microsoft.Data.SqlClient;`

Now you can compile your Blazor application again.

In Blazor, we can use Dependency Injection for using the `DataAccess` class in our pages. Therefore, we need to register it in the DI Container.

- Open `Program.cs` in your `HmsBlazor` project
- Find the following line:

```csharp
var app = builder.Build();
```

- Just _before_ this line, add:
  
```csharp
builder.Services.AddScoped<Project_HMS.DataAccess>();
```

Since we're using a development container for SQL Server, we need to tweak the connection string in `DataAccess.cs` so that we don't run into connection errors.

- On `line 43` in `DataAccess.cs`, at the end of the connection string, add:

`;TrustServerCertificate=True`

This will make the `SqlConnection` accept the development certificate that the SQL Docker container returns.

