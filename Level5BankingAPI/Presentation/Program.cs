using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Clients;
using Application.Interfaces;
using Infrastructure.Repositories;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Presentation.Formatters;
using Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
   options.OutputFormatters.Add(new CsvOutputFormatter());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    
    setupAction.IncludeXmlComments(xmlCommentsFullPath);
    
    setupAction.AddSecurityDefinition("BankingApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Input a valid token to access this API"
    });

    setupAction.AddSecurityRequirement(document => new OpenApiSecurityRequirement()
    {
        [new OpenApiSecuritySchemeReference("BankingApiBearerAuth", document)] = []
    });
});

builder.Services.AddDbContext<AccountContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("AccountContext")));

builder.Services.AddHttpClient<ICurrencyClient, CurrencyClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiWebAddress"]!);
});

builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
builder.Services.AddScoped<AccountsService>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwtOptions =>
{
    jwtOptions.Audience = builder.Configuration["Authentication:Audience"];
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]!))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{ 
    app.UseMiddleware<ExceptionHandlingMiddleware>();   
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();