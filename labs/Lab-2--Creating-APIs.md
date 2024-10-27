# Creating an API for data access and clientside use
In this HOL, you will use the generated razor pages and extract the API that can be used to access the database. By creating an API, we introduce the option to start using the webassembly version of the pages we created, makign the application less resource intensive on the server and hence making it easyer to scale the number of users of your application.

## Generating an API
For this we are again using the OpenAI playground that we used in the previosu labs. You will keep the paramters of the model the same, but we are going to change the system prompt. For the system prompt you are going to use the following instructions:

``` text
You are an AI programming assistant. Follow the user's requirements carefully and to the letter. First, think step-by-step and describe your plan for what to build in pseudocode, written out in great detail. Then, output the code in a single code block. Minimize any other prose. You specialize in extracting ASP.NET API controllers out of Blazor .NET 8.0 pages. The APIs you create are Restfull and abstract the dataaccess code you find in the blazor pages. Ensure the results from the queries are mapped to types before returned.

Each user prompt contains the blazor file from which you extract the API. You respond with no markdown and no explanation.
```
You can see that we now change the prompt to make it only focus on generating an API controller for each file we feed it. Since all current generated pages contain code to acccess the database, we ask it to take that code and make an API for it. By instructing it to create a `restfull` API we ensure the API makes use of HTTP directives like `Get`, `Put`, `Post` in stead of generating method names for each call. 

### Creating an API for AddRooms.razor
We copy all the code that is currently in the `AddRooms.razor` file. And after we hit enter we wait for the repsonse. Thsi response contains now an API controller that we want to host in the Server project.

Create a new folder in the server project with the name `Controllers`
Add a new file with the name `RoomController.cs` and paste generated results from the LArge Language Model in this file and save it.

### Enabling the Blazor Server to host an API
Now we have the controller code available in the project, but the server project also needs to understand the fact it is going to host a webAPI. We also want the API to contain documentation in the OpenAPI specification format, so tools can use this information to generate code to access the API. For this we need to make some changes to the project.
### Host the API controllers

in the `program.cs` file add the follwoing lines of code:
``` c#
builder.Services.AddControllers();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
```

### add OpenAPI documentation
To add the documentation we need to take the follwoing steps:

Add references in the project to the following two packages:
``` xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.10" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.9.0" />
```
In the `program.cs` file add the following statements:

``` c#
using Microsoft.AspNetCore.OpenApi;
....
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
...
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

```
### Testing the Controller
After making these changes, start the blazor application and then navigate to the api.
e.g. for our `RoomsController.cs` api, we can browse to the location: `https://localhost:7000/api/room` and this wll result in a list of rooms returned in a json document.

> **Fixing SQL Injection issues**
The code that we now have, has some serious sequrity vulnerabilities. This has to do with API methods that accept user input from the querystring and without checkign and validation concatenating it to the SQL statements that are used to access the database. We will adress this in later lab, but be aware this is the case and we need to fix this before we would deploy our application!

## Creating subsequent controllers
Now repeat this process for all razor pages that we have in the project, so we have API controllers for each resource that we can access in our application. 

# Rewiring the razor pages
Now we have API controllers, but our razor pages are still working with direct access to the database. In order to make the razor pages use the controllers that we just created we are going to use CoPIlot in our IDE, since Copilot has much more context about our application and can help us with creating these changes.

> Note: This part of the HOL expect that you have activated your Copilot voucher, so you can use copilot in the codespace environment.

### Prompting Copilot chat to make the required changes
Copilot chat has the option to reference files when you give it a prompt. We are going to make a very specific prompt for each razor page that we have. The prompt in the chat is as follows:

``` text
convert the file #<blazorpage> and change all the code refering to data access code to use the api that is created in  #<controllerclass>
```
In this prompt we refer to the blazor page that we want to convert using the `#` symbol. This gives Copilot extra contextand now knows it needs to make changes to only this specific file. We also pass it in a reference to one or more controllers that we generated in the previous excersise. With this copilot knows exactly what code is required to retrieve the same data from the API as the date it is now getting from the data access code that is in the razor pages.

## Change the AddRooms.razor to use the API
As descibed, open Copilot Chat and give it the follwoing prompt:
``` text
convert the file #AddRoom.razor and change all the code refering to data access code to use the api that is created in  #RoomController.cs
```

Next Copilot will suggest the changes needed and you can click the preview button to see the suggested changes.
Accept the changes and see how it converted the data access code to code that now will use HttpClient to access the API controller and then get the data from there.

In order to use this code, we also need to make a few changes, since the HttpClient is not yet available as an injected class. For this we are registering this in the `program.cs` file with the following statements:

``` c#
var baseAddress = builder.Configuration.GetValue<string>("BaseUrl");

builder.Services.AddScoped(http => new HttpClient
{
    BaseAddress = new Uri(baseAddress)
});
```
This code retrieves the base URI of the appliction from the configuration context. This configuration needs to be provided. For this we change the `appsettings.json` file to contain the extra line with the BaseUrl configuration. This looks as follows:
``` json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "BaseUrl": "https://localhost:7000"
}
```
Now compile the application and navigate tot the addroom blazor component as we did in the previous hands-on.
Now you will see the exact same page, but the data is now comming from the WebAPI that we are hosting.

## Finalizing API usage
For all the subsequent razor pages do exactly the same. Prompt Copilot to make the changes to use the API controllers. If there is a page that requires more then one controller, provide the additional references to the othe controllers where the data is to be retrieved from.

# Conclusion
We now used **OpenAI** to first generate our controller classes and then used **Copilot** to make the code changes we need to use the API. We now have our application using a multi tier approach where the UI is generated on the server, to retrieve data or modify data we use an API that is hosted on the server and we now have a web application that uses less resources then using a citrix server or Azure virtual Desktop to host our windows forms application. Giving us scalabillty and much cheaper hosting then we had in the past.