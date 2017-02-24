using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ProjectOxford.Face;

namespace ERNI.UnlockIoT.Service.Controllers
{
    [Route("api/[controller]")]
    public class FacesController : Controller
    {
        private const string GroupId = "EPD2017ESK";

        private readonly FaceServiceClient faceServiceClient;

        public FacesController(FaceServiceClient faceClient)
        {
            faceServiceClient = faceClient;
        }

        [HttpPost]
        public async Task Post([FromBody]string name)
        {
            var groupId = await GetPersonGroupAsync();
            var person = await faceServiceClient.CreatePersonAsync(groupId, name);
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
                await faceServiceClient.CreatePersonGroupAsync(GroupId, "ESK EPD 2017");
                return GroupId;
            }
        }
    }
}
