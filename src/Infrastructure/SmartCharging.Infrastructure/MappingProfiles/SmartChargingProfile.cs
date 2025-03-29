using AutoMapper;
using SmartCharging.Domain.DataTransfer;
using SmartCharging.Domain.Entities;

namespace SmartCharging.Infrastructure.MappingProfiles
{
	public class SmartChargingProfile : Profile
	{
		public SmartChargingProfile()
		{
			CreateMap<Group, CreateGroupDto>().ReverseMap();
			CreateMap<ChargeStation, ChargeStationDto>().ReverseMap();
			CreateMap<Connector, ConnectorDto>().ReverseMap();
		}
	}
}
