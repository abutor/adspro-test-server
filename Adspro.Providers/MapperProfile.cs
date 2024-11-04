using Adspro.Contract.Models;
using Adspro.Data.Models;
using AutoMapper;

namespace Adspro.Providers
{
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserModel>().ReverseMap();
        }
    }
}
