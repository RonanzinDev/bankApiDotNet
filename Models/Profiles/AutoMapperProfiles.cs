using AutoMapper;

namespace bankApi.Models.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterNewAccountModel, Account>();
            CreateMap<UpdateAccountModel, Account>();
            CreateMap<Account, GetAccountModel>();

        }
    }
}