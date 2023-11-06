using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Models.DTO;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    //Now valid for all methods, You cannot access this controller without being logged in
    //can place also on methods
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : Controller
    {
        loginUserSessionDto _usr = null;

        IPersonsService _PersonService = null;
        ILoginService _loginService = null;
        ILogger<AdminController> _logger;

        //GET: api/admin/seed?count={count}
        [HttpGet()]
        [ActionName("Seed")]
        [ProducesResponseType(200, Type = typeof(adminInfoDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Seed(string count)
          {
            if (!int.TryParse(count, out int _count))
            {
                return BadRequest("count format error");
            }

            adminInfoDbDto _info = await _PersonService.SeedAsync(_usr, _count);
            return Ok(_info);
        }


        //GET: api/admin/removeseed
        [HttpGet()]
        [ActionName("RemoveSeed")]
        [ProducesResponseType(200, Type = typeof(adminInfoDbDto))]
        public async Task<IActionResult> RemoveSeed(string seeded = "true")
        {
            if (!bool.TryParse(seeded, out bool _seeded))
            {
                return BadRequest("count format error");
            }

            adminInfoDbDto _info = await _PersonService.RemoveSeedAsync(_usr, _seeded);
            return Ok(_info);
        }

        //GET: api/admin/seeduser?users={count}&superusers={count}
        [HttpGet()]
        [ActionName("SeedUsers")]
        [ProducesResponseType(200, Type = typeof(usrInfoDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> SeedUsers(string countUsr = "32", string countSupUsr = "2")
           => BadRequest("Not implemented");

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        #region constructors
        
        public AdminController(IPersonsService PersonService /* , ILogger<AdminController> logger*/)
        {
            _PersonService = PersonService;
           // _logger = logger;
        }
        //public AdminController(IPersonsService PersonService, ILoginService loginService, ILogger<AdminController> logger)
        //{
        //    _PersonService = PersonService;
        //    _loginService = loginService;

        //    _logger = logger;
        //}
        
        #endregion
    }
}

