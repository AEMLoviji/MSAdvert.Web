using AdvertWeb.Models.AdvertManagement;
using AdvertWeb.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace AdvertWeb.Controllers;

public class AdvertManagementController : Controller
{
    private readonly IFileUploader _fileUploader;

    public AdvertManagementController(IFileUploader fileUploader)
    {
        _fileUploader = fileUploader;
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateAdvertViewModel input, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            // TODO: get ID from AdvertAPI call.
            var id = new Random().Next().ToString();

            var filePath = string.Empty;
            if (imageFile != null)
            {
                var fileName = string.IsNullOrEmpty(imageFile.FileName)
                    ? id
                    : Path.GetFileName(imageFile.FileName);

                filePath = $"{id}/{fileName}";

                try
                {
                    using var readStream = imageFile.OpenReadStream();

                    var isFileUploadOk = await _fileUploader.UploadFileAsync(filePath, readStream);
                    if (!isFileUploadOk)
                    {
                        throw new Exception("Could not upload the image to file repository.");
                    }
                }
                catch (Exception e)
                {

                }
            }

            return RedirectToAction("Index", "Home");
        }

        return View(input);
    }

}
