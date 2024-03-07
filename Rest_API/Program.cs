
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Rest_API.Data;
using Rest_API.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Rest_API {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            // Add services to the container.
            builder.Services.AddCors(options => {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder => {
                        builder
                            .AllowAnyOrigin()
                            .WithMethods("GET")
                            .WithMethods("POST")
                            .WithMethods("PUT")
                            .WithMethods("DELETE")
                            .AllowAnyHeader();
                    });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme() {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            builder.Services.AddDbContext<BlogContext>(options => 
                                        options.UseSqlite(builder.Configuration.GetConnectionString("BlogContext")));

            builder.Services.AddAuthorization();
            builder.Services.AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<BlogContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapIdentityApi<User>();

            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
