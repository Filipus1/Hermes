using Hermes.API.Cookies;
using Hermes.Application.Abstraction;
using Hermes.Application.Entities;
using Hermes.Application.Services;
using Hermes.Infrastructure.Config;
using Hermes.Infrastructure.Context.Extension;
using Hermes.Infrastructure.CronJobs.Extension;
using Hermes.Infrastructure.Email;
using Hermes.Infrastructure.Factory;
using Hermes.Infrastructure.FormatSerializer;
using Hermes.Infrastructure.Mapper;
using Hermes.Infrastructure.Repositories;
using Hermes.Infrastructure.Seed;
using Hermes.Infrastructure.TokenGenerator;
using Microsoft.AspNetCore.Identity;
using MimeKit;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load("../.env");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpClient();

builder.Services.AddSingleton<AppContextFactory>();
builder.Services.AddSingleton<IEmailConfig, EmailConfig>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<ITokenGenerator, TokenGenerator>();
builder.Services.AddSingleton<IFormatSerializer, BanSerializer>();
builder.Services.AddSingleton<CookieManager>();

builder.Services.AddScoped<MimeMessage>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailSender>(provider =>
{
    var message = provider.GetRequiredService<MimeMessage>();
    var config = provider.GetRequiredService<IEmailConfig>();

    return new EmailSender(message, config);
});

builder.Services.AddScoped<ITokenRepository>(provider =>
{
    var factory = provider.GetRequiredService<AppContextFactory>();
    var context = factory.CreateDbContext(args);
    var tokenGenerator = provider.GetRequiredService<ITokenGenerator>();

    return new TokenRepository(context, tokenGenerator);
});

builder.Services.AddScoped<IUserRepository>(provider =>
{
    var factory = provider.GetRequiredService<AppContextFactory>();
    var context = factory.CreateDbContext(args);

    var hasher = provider.GetRequiredService<IPasswordHasher<User>>();

    return new UserRepository(context, hasher);
});

builder.Services.AddAuthentication("auth-scheme")
    .AddCookie("auth-scheme", options =>
    {
        options.Cookie.Name = "auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    })
    .AddCookie("active-scheme", options =>
    {
        options.Cookie.Name = "active";
        options.Cookie.HttpOnly = false;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors-policy", (pb) =>
    {
        pb.WithOrigins(Environment.GetEnvironmentVariable("REACT_CLIENT_URL")!)
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddCronJobs();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("cors-policy");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.ApplyMigrations(args);
await app.Services.SeedAdminUser();

app.Run();