using AdvertApi.Models;
using AutoMapper;

namespace AdvertWeb.ServiceClients.AdvertApiClient.Contracts;

public class AdvertApiProfile : Profile
{
    public AdvertApiProfile()
    {
        CreateMap<CreateAdvertRequest, CreateAdvertRequestModel>().ReverseMap();
        CreateMap<CreateAdvertResponse, CreateAdvertResponseModel>().ReverseMap();
        CreateMap<ConfirmAdvertRequest, ConfirmAdvertRequestModel>().ReverseMap();
    }
}