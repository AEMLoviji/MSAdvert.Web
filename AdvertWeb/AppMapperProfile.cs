using AdvertWeb.Models.AdvertManagement;
using AdvertWeb.ServiceClients.AdvertApiClient.Contracts;
using AutoMapper;

namespace AdvertWeb;

public class AppMapperProfile : Profile
{
    public AppMapperProfile()
    {
        CreateMap<CreateAdvertRequest, CreateAdvertViewModel>().ReverseMap();
    }
}