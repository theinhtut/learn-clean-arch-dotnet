using BuberDinner.Application.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    builder.Services.AddControllers();
}
var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}