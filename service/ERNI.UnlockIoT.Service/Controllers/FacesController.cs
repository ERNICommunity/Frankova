using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.ProjectOxford.Face;

namespace ERNI.UnlockIoT.Service.Controllers
{
    [Route("api/[controller]")]
    public class FacesController : Controller
    {
        private const string GroupId = "epd";

        private readonly FaceServiceClient faceServiceClient;

        public FacesController(FaceServiceClient faceClient)
        {
            faceServiceClient = faceClient;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPerson()
        {
            var form = await Request.ReadFormAsync();
            var name = form["name"];

            var groupId = await GetPersonGroupAsync();
            var person = await faceServiceClient.CreatePersonAsync(groupId, name);

            using (var imageStream = form.Files[0].OpenReadStream())
            {
                await faceServiceClient.AddPersonFaceAsync(groupId, person.PersonId, imageStream);
            }

            await faceServiceClient.TrainPersonGroupAsync(groupId);

            return Ok($"Person added: {person.PersonId}");
        }

        private async Task<string> GetPersonGroupAsync()
        {
            try
            {
                var group = await faceServiceClient.GetPersonGroupAsync(GroupId);
                return group.PersonGroupId;
            }
            catch
            {
                await faceServiceClient.CreatePersonGroupAsync(GroupId, "EPD2017ESK");
                return GroupId;
            }
        }
    }
}
