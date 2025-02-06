global using Madiff.Api.Models;
global using Microsoft.OpenApi.Models;
global using MadiffApi.Services;
global using MadiffApi.Middlewares;
global using MadiffApi.Repositories;
global using Microsoft.AspNetCore.Mvc;
global using MadiffApi.Requests;
global using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Madiff API", Version = "v1" });
});
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ICardActionService, CardActionService>();
builder.Services.AddScoped<IAvailableActionsProvider, AvailableActionsProvider>();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Madiff API V1"));
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
