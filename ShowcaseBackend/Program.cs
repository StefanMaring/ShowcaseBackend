using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rest_API.Data;
using Rest_API.Hubs;
using ShowcaseBackend.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder
                .WithOrigins("http://localhost:5173/")
                //.AllowAnyOrigin()
                //.WithMethods("GET")
                //.WithMethods("POST")
                //.WithMethods("PUT")
                //.WithMethods("DELETE")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((x) => true)
                .AllowCredentials();
        });
});

// For Entity Framework
builder.Services.AddDbContext<BlogContext>(options => options.UseSqlite(configuration.GetConnectionString("BlogContext")));

//For SignalR
builder.Services.AddSignalR();

builder.Services.AddTransient<BlogHub>();

// For Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<BlogContext>()
    .AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<BlogHub>("/bloghub");

app.Run();

