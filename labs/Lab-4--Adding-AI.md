# Adding AI capabilities in your application
In this lab, you are going to add a very usefull capability to the application that is going to use a LLM to help fill in a form for makign reservations.

By now you have seen how we can use LLM's to help us converting existing software into new modern software that can even run in the cloud. But with the same models we can also augement our applications and make them more "smart"

## Adding Clipboard Support
Since we want to paste the text that is on the clipboard we need to add clipboard support to the application.
To make a button that can pick up the text from the clipboard take the following steps:

1. In your Blazor app, add the `CurrieTechnologies.Razor.Clipboard` [NuGet package](https://www.nuget.org/packages/CurrieTechnologies.Razor.Clipboard/)

   ```sh
   Install-Package CurrieTechnologies.Razor.Clipboard
   ```

2. In your Blazor app's `Startup.cs`, register the 'ClipboardService'.

   ```cs
   public static void Main(string[] args)
   {
       ...
       builder.Services.AddClipboard();
       ...
   }
   ```

3. Add this script tag in your root razor file App.razor, right under the framework script tag. (i.e `<script src="_framework/blazor.server.js"></script>` for Blazor Server Apps or `<script src="_framework/blazor.webassembly.js"></script>` for Blazor WebAssembly Apps).
We need to register this javascript, because that is the only way we can interact with the browser and get access to the clipboard.

   ```html
   <script src="_content/CurrieTechnologies.Razor.Clipboard/clipboard.min.js"></script>
   ```

4. You can now inject the ClipboardService into any Blazor page. We will do this for the Reservation.razor page, where we want to create our magic paste button.:

   ```razor
   @using CurrieTechnologies.Razor.Clipboard
   @inject ClipboardService clipboard

   <button @onclick="btnPasteOnclick">Paste From Clipboard</button>

    ....
    @code{
    private string clipboardText;

        private async Task btnPasteOnclick()
        {
            clipboardText = await clipboard.ReadTextAsync();
        }
    }
   ```

## Adding AI Capabilities by hand
We are going to make a change to the AddResevration screen and we are going to add a smart paste capability to the form. This smart paste capability makes it possible to copy an email from a customer and simply paste the email in the form, resulting in all fields to be fileld with the correct values, so we can very simply input the data. This is helpefull for our users, since they don't have to parse the email themselves translatign it into a real booking, in the booking system we have.

The way we do this, is by using a C# library called Semantic Kernel, that makes it very easy to incorporate the fetures we used during our prevous labes into your application. 

With Semantic Kernel we are going to parse an email text, provide a system prompt with instructions to get data taht we can bind to our form.

## Adding semantci kernel to our application

## Providing the prompt
We are going to use the following system prompt:
``` c#
string prompt = """
You are an AI assitant that takes customer emails and converts them to a json structure that contains the datapoints you find in the email. The json structure looks as follows:
{
    "checkInDate": "Check in Date in dd-MM-yyyy format",
    "checkOutDate": "Check out date in dd-MM-yyyy format",
    "FoodPackage": "Requested Food Package Id",
    "RoomType": Requested Room Type,
    "Name": "Customer full name",
    "Address": "Customer address",
    "PhoneNumber": "Customer phone number, prefered cell phone if available",
    "Total": "total cost of the booking in USD",
    "AdvancePayment": "Advanced payment made at time of booking in USD",
    "Remaining": "Remaining payment to be done at checkin in USD"
}
"""
```
The user prompt will contain an email conversation that we can get from the clipboard, when we click a button that we call smart paste. 


## Creating plugins to solve parts of the data
In order to select the correct food package, we need to first query the database what food packages are available and then have the LLM decide which food package fits best for this customer request. To create such a function you can create a C# function that will be called by Semantic kernel automaticaly.

```c#
  private class FoodPackagePlugin
  {
      [KernelFunction,
       Description("returns the food package ID, based on the food preferences of the customer")]
      [return:Description("The food package ID")]
      public static int SelectFoodPreference([Description("The food preference of the customer")] string preferedFood)
      {
         string fdsql = "select FId, FName from FoodPackage;";
            DataSet ds2 = Da.ExecuteQuery(fdsql);
            string availablePackages;
            ds2.Tables[0].AsEnumerable().Select(dataRow => 
                {
                    availablePackages += $"Fid: {dataRow.Field<string>("Fid")} , Food Package: { dataRow.Field<string>("FName")}\n"; 
                });

        //now we have available food packages in the form:
        //Fid:11, Food Package:Rice+Chicken
        //Fid:12, Food Package:Rice+Beef
        //Fid:13, Food Package:Rice+Shrimp
        // we use this to let the LLM decide which package matches best for this customer, given their food preference.
       string systemPrompt =
        """
        You are tasked with selecting a food package based on the customers food preference. You must make a choice and you only return the Fid number as an integer that matches the package you selected. The food packages available are:
        ${availablePackages}
        The next user prompt will contain the customers food preference.
        """
        var history =  = new ChatHistory
                      {
                          new(AuthorRole.System,systemPrompt)
                          new(AuthorRole.User,preferedFood)
                      };
                      
                var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(settings.DeploymentName,
                                                         settings.EndPoint,
                                                         settings.ApiKey)
                           .Build();

               chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
               var settings = new OpenAIPromptExecutionSettings {ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions};
               var result = await _chatCompletionService.GetChatMessageContentAsync(history, settings, kernel);

               int parsedResult = JsonSerializer.Deserialize<int>(result.ToString());
            return parsedResult;
        }
  }
```

We can do the same for selecting the corret room type, based on what we know from the customer. For this we can create a plugin that takes the details and from there determines what the best hotel room would be for them.

This function works more or less the same, it retrieves room type information from the database and it takes the input with details about the customer. Based on this it returns the best match room ID that is available.

```c#
  private class RoomTypePlugin
  {
      [KernelFunction,
       Description("returns the room  ID, of an available room based on the details we know about the customer")]
      [return:Description("The room ID")]
      public static int SelectFoodPreference([Description("The details about what we know about the customer that would influence the selection of a room type")] string customerdetails)
      {
         string roomsql = "select rID, Category from Room where IsBooked='No'";
            DataSet ds2 = Da.ExecuteQuery(roomsql);
            string availableRooms;
            ds2.Tables[0].AsEnumerable().Select(dataRow => 
                {
                    availableRooms += $"Room ID: {dataRow.Field<string>("rID")} , Room type: { dataRow.Field<string>("Category")}\n"; 
                });

        //now we have available food packages in the form:
        //Room ID:11, Room Type:Single
        //Room ID:14, Room Type:Double King
        //Room ID:16, Room Type:Double
        // we use this to let the LLM decide which room matches best for this customer, given the details we know.
       string systemPrompt =
        """
        You are tasked with selecting a room based on the information we have about the customer. You must make a choice and you only return the Room ID number as an integer that matches the room you selected. The rooms  available are:
        ${availableRooms}
        The next user prompt will contain the details about the customer.
        """
        var history =  = new ChatHistory
                      {
                          new(AuthorRole.System,systemPrompt)
                          new(AuthorRole.User,customerdetails)
                      };
                      
                var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(settings.DeploymentName,
                                                         settings.EndPoint,
                                                         settings.ApiKey)
                           .Build();

               chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
               var settings = new OpenAIPromptExecutionSettings {ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions};
               var result = await _chatCompletionService.GetChatMessageContentAsync(history, settings, kernel);

               int parsedResult = JsonSerializer.Deserialize<int>(result.ToString());
            return parsedResult;
        }
  }

```

## Calling semeantic kernel to provide results
As response to the click on a button to paste the clipboard email conversation we need to create an event handler that will then call semantic kernel and prompt it to do its work.
the button handler for this looks as follows:

``` C#
private void btnSmartPaste_Clicked()
{
    string prompt = 
    """
    You are an AI assitant that takes customer emails and converts them to a json structure that contains the datapoints you find in the email. The json structure looks as follows:
    {
        "checkInDate": "Check in Date in dd-MM-yyyy format",
        "checkOutDate": "Check out date in dd-MM-yyyy format",
        "FoodPackage": "Requested Food Package Id",
        "RoomType": Requested Room Type,
        "Name": "Customer full name",
        "Address": "Customer address",
        "PhoneNumber": "Customer phone number, prefered cell phone if available",
        "Total": "total cost of the booking in USD",
        "AdvancePayment": "Advanced payment made at time of booking in USD",
        "Remaining": "Remaining payment to be done at checkin in USD"
    }
    """;

    var kernel = Kernel.CreateBuilder()
                           .AddAzureOpenAIChatCompletion(settings.DeploymentName,
                                                         settings.EndPoint,
                                                         settings.ApiKey)
                           .Plugins.AddFromType<FoodPackagePlugin>()
                           .Plugins.AddFromType<RoomTypePlugin>()
                           .Build();
    
    chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
    var settings = new OpenAIPromptExecutionSettings {ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions};
    var result = await chatCompletionService.GetChatMessageAsync(prompt, settings, _kernel);
    @@TODO, Map the json to the fields
}
```
## Test the implementation
Now we want to test if we can get a booking inserted with a simple smart paste option. 
For this use the follwoing email conversation to see if it works:

``` text
From: Marcel de Vries <vriesmarcel@hotmail.com> 
Sent: 22 October 2024 21:34
To: Reservations <reservations@thedixon.co.uk>
Subject: Re: I like to make a reservation

Sufia, 
Absolutely, my wife is vegitarian, I am good with anything.
One note, I am quite tall so it would be great if you can accomodate us a room with a larger bed.

We land at Heathrow arround noon and then. Will travel to the hotel. If the rooms are not ready we will drop our bags and check I later. Of course it would be awesome if at least one room would be available so we can fresh up

Thanks 
Marcel de Vries
+31635115491 

Sent from Nine
________________________________________
From: Reservations <reservations@thedixon.co.uk>
Sent: Monday, October 21, 2024 4:15 PM
To: VRIESMARCEL@HOTMAIL.COM
Subject: Re: I like to make a reservation

Dear Mr Devries,
 
Greetings from the Dixon Tower Bridge, Autograph Collection.
 
Thank you for inquiries to stay in our  hotel.

May I request for your approximate arrival time in the hotel, and I can note it in the booking to ensure a smooth arrival for you.

Also can you provide some information about your food preferences, since our rooms include a meal every night.
 
Below, I have provided some information about our hotel, which I hope you will find useful prior to your stay with us.
 
•	Check in time is at 03:00 pm
•	Checkout time is at 11:00 am
•	Early check in is subject to availability on the day of arrival however we always try our very best to accommodate an earlier check if possible. We do recommend if you would like to confirm an early check in is to pre book from the night before if we have availability.
•	To arrange a transfer, please advise us 72 hours in advance and my colleagues from the reception team will be more than happy to assist with these arrangements for you. They will require your complete flight details so they can quote the appropriate costs to you.
 
Please do let us know if there is anything else we can further assist prior to your arrival.
 
We are looking forward to Welcome you soon at The Dixon!

Warm Regards
Sufia
T +44 (0) 203 959 2900
reservations@thedixon.co.uk  |  thedixon.co.uk
The Dixon, 211 Tooley St, London SE1 2JX
 
  
Disclaimer
This e-mail and any attachments hereto are strictly confidential and intended solely for the addressee. It may contain information which is privileged. If you are not the intended addressee, you must not disclose, forward, copy or take any action in relation to this e-mail or attachments. If you have received this e-mail in error, please delete it and notify postmaster@dominvsgroup.com
________________________________________
From: Marcel de Vries <vriesmarcel@hotmail.com> 
Sent: 22 October 2024 21:34
To: Reservations <reservations@thedixon.co.uk>
Subject: I like to make a reservation

L.s., 
Next week saturday we are comming to London and we would like to stay at your hotel. I am traveling with my wife.
The plan is that we stay for two days. 

We land at Heathrow arround noon and then. Will travel to the hotel. If the rooms are not ready we will drop our bags and check I later. Of course it would be awesome if at least one room would be available so we can fresh up

Thanks 
Marcel 

Sent from Nine
```

Copy this text to the clipboard and then click the button Smart Paste that you created and see if the fields are filled out in the form according to your expectations.

You can expect that a room of type King is selected, since it has a larger bed, that the food option for a vegitarion is selected. That the check in date is Saturday 26th and check out date 28th. The telephone number should contain the cell phone number that is in the signature of Marcel.

## Using smart components
Microsoft has released a package called Smart components that takes care of the similar approach then what we just created ourselves. This is specificly for blazor and it is rather simple to use. 

To use smart components you start with adding two packages to the project. 
``` c#
 <PackageReference Include="SmartComponents.AspNetCore" Version="0.1.0-preview10148" />
 <PackageReference Include="SmartComponents.Inference.OpenAI" Version="0.1.0-preview10148" />
```

Next you can add the smart paste button that is part of the smart components package.
``` html
<smart-paste-button>Smart paste</smart-paste-button>
```
now for the input types, you can provide smart paste context, so the LLM that is used can intepret the input and from there infer the required values.

So for the checkIn and CheckOut dates you can add the following additional context using the `data-smartpaste-description` attribute:
``` html
data-smartpaste-description="A date in the format of YYYY-MM-dd if you can't find the year assume current year 2024"
``` 

Finaly you make a change to the `program.cs` file and add the followinf to activate the smart components AI backend:

``` C#
builder.Services.AddSmartComponents()
       .WithInferenceBackend<OpenAIInferenceBackend>();
```

Now try this and check if you can get the same results with this implementation. You will see that it is not so advanced to get you the room and food package, since it does not know how to get to that data.

@@TODO, Check if you can add functions like with semantic kernel.
