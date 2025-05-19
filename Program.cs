using Microsoft.EntityFrameworkCore;
using OrganizationForm.Models.cs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
    policy =>
    {
        policy.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<OrgFormDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiCon")));

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
var services = scope.ServiceProvider;
var dbContext = services.GetRequiredService<OrgFormDbContext>();

try
{
dbContext.Database.Migrate();
}
catch (Exception ex)
{
Console.WriteLine($"Migration error: {ex.Message}");
}
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

builder.Services.AddControllers();
app.MapControllers();

app.Run();
