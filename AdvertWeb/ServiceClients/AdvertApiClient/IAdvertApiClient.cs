using AdvertWeb.ServiceClients.AdvertApiClient.Contracts;

namespace AdvertWeb.ServiceClients.AdvertApiClient;

public interface IAdvertApiClient
{
    Task<CreateAdvertResponseModel> CreateAsync(CreateAdvertRequestModel request);

    Task<bool> ConfirmAsync(ConfirmAdvertRequestModel request);
}
