using BuberDinner.Api.Errors;
using BuberDinner.Api.Filters;
using BuberDinner.Application;
using BuberDinner.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
    //builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
    builder.Services.AddControllers();
    builder.Services.AddSingleton<ProblemDetailsFactory, BurberDinnerProblemDetailsFactory>();
}
var app = builder.Build();
{
    //app.UseMiddleware<ErrorHandlingMiddleware>(); // Using ErrorHandlingMiddleware
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}