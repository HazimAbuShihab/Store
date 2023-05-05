using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Store_Core7.Model;
using Store_Core7.Repository;
using Store_Core7.Utils;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//* Set the port number for the application
builder.WebHost.ConfigureKestrel(
    options =>
    {
        options.ListenAnyIP(builder.Configuration.GetValue<int>("Port"), listenOptions =>
        {

        });
    });


#region DB Type & Connection
var dbType = Environment.GetEnvironmentVariable("DB_TYPE");
if (string.IsNullOrEmpty(dbType))
{
    dbType = builder.Configuration.GetSection("DBType").Value;
}
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString))
{
    if ((DBType)Enum.Parse(typeof(DBType), dbType) == DBType.MSSQL)
    {
        connectionString = builder.Configuration.GetConnectionString("MSSqlConnection");
    }
    else if ((DBType)Enum.Parse(typeof(DBType), dbType) == DBType.MySQL)
    {
        connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    }
}
#endregion

// Add services to the container.
builder.Services.AddIdentity<UserModel, IdentityRole>().AddEntityFrameworkStores<AppDBContext>();
builder.Services.AddDbContext<AppDBContext>(options =>
{
    if ((DBType)Enum.Parse(typeof(DBType), dbType) == DBType.MSSQL)
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("MSSqlConnection"));
    }
    else
    {
        options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
            new MySqlServerVersion(new Version(8, 0, 23)), mySqlOptions => mySqlOptions.EnableRetryOnFailure());
    }
});
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Store APIs", Version = "v1" });
});


var app = builder.Build();
var serviceProvider = app.Services;
using (var scope = serviceProvider.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();

    DbInitializer.Initialize(dbContext);

}

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store APIs v1"));
app.UseSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
