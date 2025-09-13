using MediatR;
using MyShop.API.Middleware;
using MyShop.Application.Common.Extensions;
using MyShop.Application.Common.Interfaces;
using MyShop.Application.Common.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(PaginationExtensions).Assembly);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add repositories
builder.Services.AddScoped<ICustomerRepository, MockCustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Add exception handling middleware
app.UseExceptionHandling();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Seed test data
using (var scope = app.Services.CreateScope())
{
    var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
    if (customerRepository is MockCustomerRepository mockRepo)
    {
        mockRepo.SeedTestData();
    }
}

app.Run();
