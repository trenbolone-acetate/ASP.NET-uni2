using ASPNET2;

Console.WriteLine("Select configuration file (1- json, 2 - xml, 3 - ini, 4 - info about me): ");
var choice = Console.ReadLine();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(@$"{builder.Environment.ContentRootPath}\Properties");

switch (choice)
{
    case "1":
        builder.Configuration.AddJsonFile("launchSettings.json", optional: false, reloadOnChange: true);
        break;
    case "2":
        builder.Configuration.AddXmlFile("launchSettingsXML.xml", optional: false, reloadOnChange: true);
        break;
    case "3":
        builder.Configuration.AddIniFile("launchSettingsINI.ini", optional: false, reloadOnChange: true);
        break;
    case "4":
        builder.Configuration.AddJsonFile("configFile.json", optional: false, reloadOnChange: true);
        break;
    default:
        Console.WriteLine("Invalid choice! Falling back to default.json.");
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        break;
}

var app  = builder.Build();

switch (choice)
{
    case "1":
        app.MapGet("/", (IConfiguration config) => $"Company: {config["name"]}, " +
                                               $"number of employees: {config["employees"]}");
        break;
    case "2":
        var apple = new Company(); 
        app.Configuration.Bind(apple);
        app.MapGet("/", async context =>
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            string name = $"<p>Company name: {apple.Name}</p>";
            string employees = $"<p>Number of employees: {apple.Employees}</p>";

            await context.Response.WriteAsync($"{name}\t{employees}");
        });
        break;
    case "3":
        app.MapGet("/", (IConfiguration config) => $"Company: {config.GetValue<string>("company:name")}, " +
                                               $"number of employees: {config.GetValue<string>("company:employees")}");
        break;
    case "4":
        app.MapGet("/", (IConfiguration config) => $"Name: {config["firstName"] + ' ' + config["lastName"]}\n" +
                                                   $"Age: {config["age"]}\nHeight: {config["height"]}\nWeight: {config["weight"]}");
        break;
    default:
        app.MapGet("/", (IConfiguration config) => $"Default Case: {config["DefaultAppSetting"]}");
        break;
}

app.Run();