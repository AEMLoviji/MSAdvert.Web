using AdvertApi.Models;

namespace AdvertWeb.ServiceClients.AdvertApiClient.Contracts;

public class ConfirmAdvertRequestModel
{
    public string Id { get; set; }
    public string FilePath { get; set; }
    public AdvertStatus Status { get; set; }
}