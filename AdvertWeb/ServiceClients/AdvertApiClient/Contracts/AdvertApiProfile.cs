using AutoMapper;

namespace AdvertWeb.ServiceClients.AdvertApiClient.Contracts;

public class AdvertApiProfile : Profile
{
    public AdvertApiProfile()
    {
        CreateMap<AdvertApi.Models.CreateAdvertRequest, CreateAdvertRequest>();
        CreateMap<AdvertApi.Models.CreateAdvertResponse, CreateAdvertResponse>();
        CreateMap<AdvertApi.Models.ConfirmAdvertRequest, ConfirmAdvertRequest>();
    }
}