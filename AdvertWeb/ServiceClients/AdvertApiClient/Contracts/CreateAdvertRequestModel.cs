namespace AdvertWeb.ServiceClients.AdvertApiClient.Contracts;

public class CreateAdvertRequestModel
{
    public string Title { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }
}
