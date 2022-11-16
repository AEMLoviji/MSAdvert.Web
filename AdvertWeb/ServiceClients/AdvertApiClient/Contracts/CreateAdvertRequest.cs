namespace AdvertWeb.ServiceClients.AdvertApiClient.Contracts;

public class CreateAdvertRequest
{
    public string Title { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }
}
