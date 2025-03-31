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
			CreateMap<Group, GroupDto>().ReverseMap();
			CreateMap<Group, CreateGroupDto>().ReverseMap();
			CreateMap<Group, UpdateGroupDto>().ReverseMap();

			CreateMap<ChargeStation, ChargeStationDto>().ReverseMap();
			CreateMap<ChargeStation, CreateChargeStationDto>().ReverseMap();
			CreateMap<ChargeStation, UpdateChargeStationDto>().ReverseMap();

			CreateMap<Connector, ConnectorDto>().ReverseMap();
			CreateMap<Connector, CreateConnectorDto>().ReverseMap();
			CreateMap<Connector, UpdateConnectorDto>().ReverseMap();
		}
	}
}
