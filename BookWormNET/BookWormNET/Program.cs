
using BookWormNET.Data;
using BookWormNET.dto;
using BookWormNET.Services;
using BookWormNET.Services.Implementation;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookWormNET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddControllers();

             // add this at the top

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        ReferenceHandler.IgnoreCycles;

                    
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter()
                    );
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<BookwormDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("BookwormDB");

                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                );
            });

            builder.Services.Configure<EmailSettings>(
                 builder.Configuration.GetSection("EmailSettings")
            );


            //JWT Authentication Setup
            builder.Services
                .AddAuthentication(options =>
                {
                    // ?? REQUIRED for Google OAuth
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    // JWT still used for APIs
                    options.DefaultAuthenticateScheme = "Bearer";
                    options.DefaultChallengeScheme = "Bearer";
                })

                // ? Cookie MUST be default (no custom name)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)

                // ? Google OAuth
                .AddGoogle("Google", options =>
                {
                    options.ClientId = builder.Configuration["GoogleAuth:ClientId"];
                    options.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];
                    options.CallbackPath = "/api/auth/google/callback";

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");

                    options.SaveTokens = true;

                    // ?? FORCE GOOGLE UI
                    //options.Prompt = "select_account";
                    //options.AdditionalQueryParameters.Add("prompt", "select_account");
                    options.Events.OnRedirectToAuthorizationEndpoint = context =>
                    {
                         var uri = context.RedirectUri;

                         if (!uri.Contains("prompt="))
                         {
                             uri += uri.Contains("?") ? "&prompt=select_account" : "?prompt=select_account";
                         }

                         context.Response.Redirect(uri);
                         return Task.CompletedTask;
                    };
                })

                // ? JWT Bearer
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])
                    )
                    };
                });



            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("UserPolicy",
                    policy => policy.RequireAuthenticatedUser());

                options.AddPolicy("AdminOnly",
                    policy => policy.RequireRole("ADMIN"));
            });

            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });


            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IShelfService, ShelfService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<ICheckoutService, CheckoutService>();
            builder.Services.AddScoped<ILibraryPackageService, LibraryPackageService>();
            builder.Services.AddScoped<ILibraryCheckoutService, LibraryCheckoutService>();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<TransactionPdfService>();
            builder.Services.AddScoped<IOrderHistoryService, OrderHistoryService>();
            builder.Services.AddScoped<IGenereService, GenereService>();
            builder.Services.AddScoped<ILanguageService, LanguageService>();
            builder.Services.AddScoped<IProductBeneficiaryService, ProductBeneficiariesService>();
            builder.Services.AddScoped<IBeneficiaryService, BeneficiaryService>();
            builder.Services.AddScoped<IPdfBookService, PdfBookService>();



            var app = builder.Build();
            app.UseMiddleware<BookWormNET.Middlewares.GlobalExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
