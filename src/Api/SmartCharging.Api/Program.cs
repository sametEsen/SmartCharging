using Microsoft.EntityFrameworkCore;
using SmartCharging.Application.Services.Abstract;
using SmartCharging.Application.Services.Concrete;
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
			builder.Services.AddScoped<IGroupService, GroupService>();
			builder.Services.AddScoped<IConnectorService, ConnectorsService>();
			builder.Services.AddScoped<IChargeStationRepository, ChargeStationRepository>();

			builder.Services.AddAutoMapper(typeof(SmartChargingProfile));

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
