using AutoMapper;
using SmartCharging.Domain.DataTransfer.ChargeStation;
using SmartCharging.Domain.DataTransfer.Connector;
using SmartCharging.Domain.DataTransfer.Group;
using SmartCharging.Domain.Entities;

namespace SmartCharging.Infrastructure.MappingProfiles
{
	public class SmartChargingProfile : Profile
	{
		public SmartChargingProfile()
		{
			CreateMap<Group, CreateGroupDto>().ReverseMap();
			CreateMap<ChargeStation, CreateChargeStationDto>().ReverseMap();
			CreateMap<Connector, CreateConnectorDto>().ReverseMap();
		}
	}
}
