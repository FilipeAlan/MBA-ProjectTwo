using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCF.Core.Context;
using PCF.Core.Interface;
using PCF.Core.Repository;

var builder = WebApplication.CreateBuilder(args);

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin() // Permite qualquer origem (ajuste conforme necessário)
               .AllowAnyMethod() // Permite todos os métodos (GET, POST, etc.)
               .AllowAnyHeader(); // Permite todos os cabeçalhos
    });
});


// Adiciona a Connection String do SQLite
var connectionString = builder.Configuration.GetConnectionString("SQLiteConnection");
builder.Services.AddDbContext<PCFDBContext>(options =>
    options.UseSqlite(connectionString));

// Configura serviços do Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<PCFDBContext>()
    .AddDefaultTokenProviders();

// Adiciona serviços da API
builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Configura Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<PCFDBContext>();
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao aplicar migrations: {ex.Message}");
        }
    }

    // Configurações do Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PCF API v1");
        c.RoutePrefix = string.Empty; // Faz com que o Swagger seja a página inicial
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();
