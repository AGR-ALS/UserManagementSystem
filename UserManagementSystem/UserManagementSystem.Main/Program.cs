using Microsoft.EntityFrameworkCore;
using UserManagementSystem.DataAccess.Context;
using UserManagementSystem.DataAccess.Mapping;
using UserManagementSystem.Main;
using UserManagementSystem.Main.Extensions.Authentication;
using UserManagementSystem.Main.Extensions.Environment;
using UserManagementSystem.Main.Extensions.MessageBrokers;
using UserManagementSystem.Main.Extensions.Services;
using UserManagementSystem.Main.Middleware.Activity;
using UserManagementSystem.Main.Middleware.Authentication;
using UserManagementSystem.Main.Middleware.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserManagementSystemContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddDiServices();
builder.Services.AddAuthenticationWithJwtScheme(builder.Configuration);
builder.Services.AddRabbitMqViaMassTransit(builder.Configuration);
builder.Services.ConfigureOptions(builder.Configuration);

builder.Services.AddAutoMapper(cfg => { }, typeof(UserProfile));

var app = builder.Build();

if (app.Environment.IsDockerEnvironment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<UserManagementSystemContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<UnauthorizedRedirectMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<RefreshTokenAuthenticationMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UpdateUserLastActivityMiddleware>();

app.MapStaticAssets();
app.MapControllers();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}")
    .WithStaticAssets();

app.Run();