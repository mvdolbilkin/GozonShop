using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/orders/swagger.json", "Orders API");
        c.SwaggerEndpoint("/swagger/payments/swagger.json", "Payments API");
    });
}

app.UseCors("AllowFrontend");


app.UseAuthorization();

app.MapControllers();

await app.UseOcelot();

app.Run();
