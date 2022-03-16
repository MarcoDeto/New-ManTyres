using AutoMapper;
using ManTyres.COMMON.DTO;
using ManTyres.DAL.MongoDB.Models;
using ManTyres.DAL.SQLServer.Entities;

namespace ManTyres.BLL.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User, UserDTO>();
			CreateMap<UserDTO, User>();

			CreateMap<ApplicationRole, RoleDTO>();
			CreateMap<RoleDTO, ApplicationRole>();

			CreateMap<Clienti, ClientiDTO>()
				 .ForMember(x => x.ClienteId, _ => _.MapFrom(y => y.ClienteId));
			CreateMap<ClientiDTO, Clienti>()
				 .ForMember(x => x.ClienteId, _ => _.MapFrom(y => y.ClienteId));

			CreateMap<Depositi, DepositiDTO>();
			CreateMap<DepositiDTO, Depositi>();

			CreateMap<Inventario, InventarioDTO>();
			CreateMap<InventarioDTO, Inventario>();

			CreateMap<Pneumatici, PneumaticiDTO>()
				 .ForMember(x => x.Quantita, _ => _.MapFrom(y => y.Quantità));
			CreateMap<PneumaticiDTO, Pneumatici>()
				 .ForMember(x => x.Quantità, _ => _.MapFrom(y => y.Quantita));


			CreateMap<Stagioni, StagioniDTO>();
			CreateMap<StagioniDTO, Stagioni>();

			CreateMap<Sedi, SediDTO>();
			CreateMap<SediDTO, Sedi>();

			CreateMap<Veicoli, VeicoliDTO>()
				 .ForMember(x => x.VeicoloId, _ => _.MapFrom(y => y.VeicoloId));
			CreateMap<VeicoliDTO, Veicoli>()
				 .ForMember(x => x.VeicoloId, _ => _.MapFrom(y => y.VeicoloId));
		}
	}
}
