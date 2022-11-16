using AdvertWeb.ServiceClients.AdvertApiClient.Contracts;

namespace AdvertWeb.ServiceClients.AdvertApiClient;

public interface IAdvertApiClient
{
    Task<CreateAdvertResponse> CreateAsync(CreateAdvertRequest request);

    Task<bool> ConfirmAsync(ConfirmAdvertRequest request);
}
