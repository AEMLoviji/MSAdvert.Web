using AdvertApi.Models;
using AdvertWeb.ServiceClients.AdvertApiClient.Contracts;
using AutoMapper;
using System.Net;

namespace AdvertWeb.ServiceClients.AdvertApiClient;

public class AdvertApiClient : IAdvertApiClient
{
    private readonly HttpClient _client;
    private readonly string _baseAddress;
    private readonly IMapper _mapper;

    public AdvertApiClient(IConfiguration configuration, HttpClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;

        _baseAddress = configuration.GetSection("AdvertApi").GetValue<string>("BaseUrl");
    }

    public async Task<CreateAdvertResponseModel> CreateAsync(CreateAdvertRequestModel model)
    {
        var advertApiRequest = _mapper.Map<CreateAdvertRequest>(model);

        var response = await _client.PostAsJsonAsync(
            new Uri($"{_baseAddress}/create"),
            advertApiRequest);

        var createdAdvert = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

        return _mapper.Map<CreateAdvertResponseModel>(createdAdvert);
    }

    public async Task<bool> ConfirmAsync(ConfirmAdvertRequestModel request)
    {
        var advertApiRequest = _mapper.Map<ConfirmAdvertRequest>(request);

        var response = await _client.PutAsJsonAsync(
            new Uri($"{_baseAddress}/confirm"),
            advertApiRequest);

        return response.StatusCode == HttpStatusCode.OK;
    }
}
