using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();

// Configure Kestrel to use the backend.pfx certificate
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(listenOptions =>
    {
        // Path to the PFX file
        var certPath = "/home/danclarke/Documents/CProjects/DatingAppNew/client/ssl/backend.pfx";
        
        // If you set an export password when generating the PFX, use it here
        var certPassword = "ron!XD7557"; // Replace with your password if set

        // Load the certificate
        listenOptions.ServerCertificate = new X509Certificate2(certPath, certPassword);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Configure CORS to allow requests from Angular frontend
app.UseCors(policy => 
    policy.WithOrigins("https://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod()
);

//app.UseAuthorization();

app.MapControllers();

app.Run();
