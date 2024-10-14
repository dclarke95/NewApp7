using API.Extensions;
using API.Middleware;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);


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
app.UseMiddleware<ExceptionMiddleware>();
// Enable HTTPS redirection
app.UseHttpsRedirection();

// Configure CORS to allow requests from Angular frontend
app.UseCors(policy => 
    policy.WithOrigins("https://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod()
);


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
