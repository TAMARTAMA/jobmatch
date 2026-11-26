using Microsoft.EntityFrameworkCore;
using JobMatch.Api.Data;
using JobMatch.Api.AI;
using JobMatch.Api.Agents;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<JobMatchDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("JobMatch")
    )
);
builder.Services.AddCors(options =>
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()));

builder.Services.AddSingleton<IChatClient>(sp =>
    ChatClientFactory.Create(sp.GetRequiredService<IConfiguration>()));
builder.Services.AddSingleton<JobAnalyzerAgent>();

var app = builder.Build();

app.UseCors("Angular");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
