using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestAPIService.Infrastructure;
using RestAPIService.Application;
using RestAPIService.Infrastructure.Data;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//--------------------------------- Add JWT --------------------------------------------------//
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
           .AddJwtBearer(options =>
           {
               IConfiguration config = builder.Configuration; // Correct way to access the configuration
               var secretKey = config["JWTSection:SecretKey"];

               // Check if secretKey is null or empty
               if (string.IsNullOrWhiteSpace(secretKey))
               {
                   throw new InvalidOperationException("JWT secret key is missing in the configuration.");
               }

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,

                   ValidIssuer = config["JWTSection:Issuer"],
                   ValidAudience = config["JWTSection:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
               };
               options.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       var exception = context.Exception;
                       Console.WriteLine("Token validation failed: " + exception.Message);
                       return Task.CompletedTask;
                   }
               };
           });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("1"));
    options.AddPolicy("ManagerPolicy", policy =>
        policy.RequireRole("1", "2"));

    options.AddPolicy("ViewPolicy", policy =>
        policy.RequireRole("1", "2", "3"));

});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. " +
                      "\n\nEnter your token in the text input below. " +
                      "\n\nExample: '12345abcde'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});
//------------------------------------- DbContext -------------------------------------//
builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//---------------------------------- Add Dependency Injection ---------------------------------//
builder.Services.AddRepositories();
builder.Services.Services();



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
