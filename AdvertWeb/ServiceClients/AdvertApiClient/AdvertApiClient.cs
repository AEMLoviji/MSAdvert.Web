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

    public async Task<CreateAdvertResponse> CreateAsync(CreateAdvertRequest model)
    {
        var advertApiRequest = _mapper.Map<AdvertApi.Models.CreateAdvertRequest>(model);

        var response = await _client.PostAsJsonAsync(
            new Uri($"{_baseAddress}/create"),
            advertApiRequest);

        var createdAdvert = await response.Content.ReadFromJsonAsync<AdvertApi.Models.CreateAdvertResponse>();

        return _mapper.Map<CreateAdvertResponse>(createdAdvert);
    }

    public async Task<bool> ConfirmAsync(ConfirmAdvertRequest request)
    {
        var advertApiRequest = _mapper.Map<AdvertApi.Models.ConfirmAdvertRequest>(request);

        var response = await _client.PutAsJsonAsync(
            new Uri($"{_baseAddress}/confirm"),
            advertApiRequest);

        return response.StatusCode == HttpStatusCode.OK;
    }
}
