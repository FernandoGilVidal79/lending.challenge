using LendingService.Core.Services;
using LendingService.Infrastructure;
using LendingService.Infrastructure.Context;
using LendingService.Infrastructure.Tests;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddPersistence();




var app = builder.Build();


using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
    context.Offers.Add(Seed.GetOffer());
    context.Loans.AddRange(Seed.GetMockLoans());
    context.SaveChanges();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();