using core.Services;

try
{
    // PDNService pDNService = new();
    // pDNService.WriteReturnValuesToFile($@"D:\TMEIC SVN\data\OneDrive_1_3-11-2024\70e3_240304(Trace Save and TMdN Data Included)\Database\A5CA03A_TM-70e3 - Copy.pdn", $@"D:\TMEIC SVN\data\OneDrive_1_3-11-2024\70e3_240304(Trace Save and TMdN Data Included)\Database\A5CA03A_TM-70e3.txt");
    
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container
    builder.Services.AddControllers();

    // Add custom services and interfaces here
    // builder.Services.AddScoped<IConverter, YourConverter>();
    // builder.Services.AddScoped<IPDNManager, PDNManager>();

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine($"Unhandled exception occurred: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    throw;
}

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
