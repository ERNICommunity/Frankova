using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

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

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync()
        {
            var file = Request.Form.Files[0];
            using (var image = file.OpenReadStream())
            {
                var faces = await faceServiceClient.DetectAsync(image);
                var faceIds = faces.Select(f => f.FaceId).ToArray();
                var result = await faceServiceClient.IdentifyAsync(GroupId, faceIds);

                Candidate candidate = null;
                foreach (var c in result.SelectMany(i => i.Candidates))
                {
                    if (candidate == null || candidate.Confidence < c.Confidence)
                    {
                        candidate = c;
                    }
                }

                if (candidate != null)
                {
                    var person = await faceServiceClient.GetPersonAsync(GroupId, candidate.PersonId);
                    return Ok(person.Name);
                }
            }

            return NotFound();
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPersonAsync()
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
