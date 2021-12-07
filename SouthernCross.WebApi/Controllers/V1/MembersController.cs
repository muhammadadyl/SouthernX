using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SouthernCross.Core.Entities;
using SouthernCross.Core.Services;

namespace SouthernCross.WebApi.Controllers.V1
{

    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/members")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [Route("search/{policyNumber:regex(\\d{{10}})}")]
        [HttpGet]
        [ProducesResponseType(typeof(List<Member>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(string policyNumber, string memberCardNumber) 
        {
            if (!string.IsNullOrWhiteSpace(policyNumber))
            {
                var members = await _memberService.GetAsync(policyNumber, memberCardNumber);
                return Ok(members);
            }

            return Ok(new List<Member>());
        }

        [Route("search/{dateOfBirth:regex(^(?:(?:31(\\-)(?:0?[[13578]]|1[[02]]))\\1|(?:(?:29|30)(\\-)(?:0?[[1,3-9]]|1[[0-2]])\\2))(?:(?:1[[6-9]]|[[2-9]]\\d)?\\d{{2}})$|^(?:29(\\-)0?2\\3(?:(?:(?:1[[6-9]]|[[2-9]]\\d)?(?:0[[48]]|[[2468]][[048]]|[[13579]][[26]])|(?:(?:16|[[2468]][[048]]|[[3579]][[26]])00))))$|^(?:0?[[1-9]]|1\\d|2[[0-8]])(\\-)(?:(?:0?[[1-9]])|(?:1[[0-2]]))\\4(?:(?:1[[6-9]]|[[2-9]]\\d)\\d{{2}})$)}")]
        [HttpGet]
        [ProducesResponseType(typeof(List<Member>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchByDateOfBirth(string dateOfBirth)
        {
            var dob = DateTime.ParseExact(dateOfBirth, "dd-MM-yyyy", null);
            var members = await _memberService.GetAsync(dob);

            return Ok(members);
        }

    }
}