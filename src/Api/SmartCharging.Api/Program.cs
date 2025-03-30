using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Application.Services.Concrete;
using SmartCharging.Application.Validators.ChargeStation;
using SmartCharging.Application.Validators.Connector;
using SmartCharging.Application.Validators.Group;
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

			builder.Services.AddFluentValidationAutoValidation();
			builder.Services.AddValidatorsFromAssemblyContaining<CreateGroupValidator>();
			builder.Services.AddValidatorsFromAssemblyContaining<UpdateGroupValidator>();
			builder.Services.AddValidatorsFromAssemblyContaining<CreateConnectorValidator>();
			builder.Services.AddValidatorsFromAssemblyContaining<UpdateConnectorValidator>();
			builder.Services.AddValidatorsFromAssemblyContaining<CreateChargeStationValidator>();
			builder.Services.AddValidatorsFromAssemblyContaining<UpdateChargeStationValidator>();

			builder.Services.AddControllers();

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
