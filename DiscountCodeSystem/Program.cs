using DiscountCodeSystem.Data;
using DiscountCodeSystem.Services;
using DiscountCodeSystem.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=discounts.db"));
builder.Services.AddSingleton<DiscountCodeGenerator>();
builder.Services.AddSingleton<WebSocketHandler>();

var app = builder.Build();
app.UseCors("AllowAll");
app.UseWebSockets();
app.Map("/ws/notifications", async context =>
{
    var handler = context.RequestServices.GetRequiredService<WebSocketHandler>();
    await handler.HandleAsync(context);
});
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
