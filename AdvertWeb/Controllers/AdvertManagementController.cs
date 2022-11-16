using AdvertWeb.Models.AdvertManagement;
using AdvertWeb.ServiceClients.AdvertApiClient;
using AdvertWeb.ServiceClients.AdvertApiClient.Contracts;
using AdvertWeb.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdvertWeb.Controllers;

public class AdvertManagementController : Controller
{
    private readonly IFileUploader _fileUploader;
    private readonly IAdvertApiClient _advertApiClient;
    private readonly IMapper _mapper;

    public AdvertManagementController(IFileUploader fileUploader,
        IAdvertApiClient advertApiClient,
        IMapper mapper)
    {
        _fileUploader = fileUploader;
        _advertApiClient = advertApiClient;
        _mapper = mapper;
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateAdvertViewModel input, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            var createAdvertRequest = _mapper.Map<CreateAdvertRequest>(input);

            var createdAdvert = await _advertApiClient.CreateAsync(createAdvertRequest);

            var createdAdvertId = createdAdvert.Id;
            if (imageFile != null)
            {
                var fileName = string.IsNullOrEmpty(imageFile.FileName)
                    ? createdAdvertId : Path.GetFileName(imageFile.FileName);

                string? filePath = $"{createdAdvertId}/{fileName}";
                try
                {
                    using var readStream = imageFile.OpenReadStream();

                    var isFileUploadOk = await _fileUploader.UploadFileAsync(filePath, readStream);
                    if (!isFileUploadOk)
                    {
                        throw new Exception("Could not upload the image to file repository.");
                    }

                    ConfirmAdvertRequest confirmAdvertRequest = new()
                    {
                        Id = createdAdvertId,
                        FilePath = filePath,
                        Status = AdvertApi.Models.AdvertStatus.Active
                    };

                    var confirmed = await _advertApiClient.ConfirmAsync(confirmAdvertRequest);
                    if (!confirmed)
                    {
                        throw new Exception($"Cannot confirm advert of id = {createdAdvertId}");
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    ConfirmAdvertRequest confirmAdvertRequest = new()
                    {
                        Id = createdAdvertId,
                        FilePath = filePath,
                        Status = AdvertApi.Models.AdvertStatus.Pending
                    };

                    await _advertApiClient.ConfirmAsync(confirmAdvertRequest);
                }
            }
        }

        return View(input);
    }

}
