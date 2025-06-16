using ItemManagement.Middlewares;
using ItemManagement.Configuration;
using ItemManagement.MappingProfile;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

builder.Services.AddCustomAuthentication();

builder.Services.AddCustomAuthorization();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.ConnectDatabase();

builder.Services.RegisterServices();

builder.Services.RegisterRepository();

builder.Services.RegisterValidators();

builder.Services.RegisterMiddleWares();

builder.Services.AddCorsPolicy();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.RegisterAPIs();

app.UseCors("AllowFrontEnd");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();