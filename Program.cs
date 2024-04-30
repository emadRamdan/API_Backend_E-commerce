
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApplicationProject.DAL;
using WebApplicationProject.DAL.context;
using WebApplicationProject.DAL.models;
using WebApplicationProject.DBL;

namespace WebApplicationProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            var connectionString = builder.Configuration.GetConnectionString("MyConnectionString");
            builder.Services.AddDbContext<OnlineStoreContext>(option =>
            {
                option.UseSqlServer(connectionString);
            });

            //register all DAL services 
            builder.Services.RegisterDALServices();

            //register all DBL services 
            builder.Services.RegisterAllBLServices();

            //add identity user and roles 
            builder.Services.AddIdentity<AppUser,IdentityRole >(option =>
            {
                option.Password.RequiredLength = 10;
            }).AddEntityFrameworkStores<OnlineStoreContext>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //add auth middleware 
            object value = builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "MyDefault";
                option.DefaultChallengeScheme = "MyDefault";
            }) .AddJwtBearer("MyDefault" ,option => {

                // generta the secret key for the token
                var userSKey = builder.Configuration.GetValue<string>("TokenSecret");
                var KeyByites = Encoding.ASCII.GetBytes(userSKey);
                var Key = new SymmetricSecurityKey(KeyByites);

                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = Key
                };
            });


            //add AddAuthorization middleware 

            builder.Services.AddAuthorization(option =>
            {
                option.AddPolicy("EmployeeOnly", b =>
                {
                    b
                    .RequireClaim(ClaimTypes.NameIdentifier)
                    .RequireRole("Employee");

                });
                
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //configer serve static files 
            //first create the folder for the files 

            var folderPath = Path.Combine(builder.Environment.ContentRootPath, "Uploades");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var UploadeFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploades");


            //server this folder 
            app.UseStaticFiles( new StaticFileOptions
            {
                FileProvider= new PhysicalFileProvider(UploadeFolderPath) ,
                RequestPath = "/Uploades"
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
