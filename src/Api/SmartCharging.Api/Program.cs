using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Application.Services.Concrete;
using SmartCharging.Application.Validators;
using SmartCharging.Domain.Interfaces;
using SmartCharging.Infrastructure.Contexts;
using SmartCharging.Infrastructure.MappingProfiles;
using SmartCharging.Infrastructure.Repositories;


namespace SmartCharging.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContext<SmartChargingContext>(options =>
			options.UseInMemoryDatabase("SmartChargingDB"));

			// Add services to the container.
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddScoped<IGroupRepository, GroupRepository>();
			builder.Services.AddScoped<IConnectorRepository, ConnectorRepository>();
			builder.Services.AddScoped<IChargeStationRepository, ChargeStationRepository>();
			builder.Services.AddScoped<IGroupService, GroupService>();
			builder.Services.AddScoped<IConnectorService, ConnectorService>();
			builder.Services.AddScoped<IChargeStationService, ChargeStationService>();

			builder.Services.AddAutoMapper(typeof(SmartChargingProfile));

			builder.Services.AddControllers();
			builder.Services.AddFluentValidationAutoValidation();
			builder.Services.AddFluentValidationClientsideAdapters(); // If needed for client-side validation
			builder.Services.AddValidatorsFromAssemblyContaining<CreateGroupValidator>();
			builder.Services.AddValidatorsFromAssemblyContaining<ConnectorValidator>();
			builder.Services.AddValidatorsFromAssemblyContaining<ChargeStationValidator>();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
