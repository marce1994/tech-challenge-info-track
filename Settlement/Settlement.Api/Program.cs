using FluentValidation;
using FluentValidation.AspNetCore;
using Settlement.Application.Config;
using Settlement.Api.ViewModels;
using Settlement.Api.Validators;
using Settlement.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDateOnlyTimeOnlyStringConverters();
builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<BookingViewModel>, BookingViewModelValidator>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<BookingServiceOptions>(builder.Configuration.GetSection(BookingServiceOptions.BookingService));

// Add services
builder.Services
    .AddApplication();

var app = builder.Build();

// Set up endpoints
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.Run();