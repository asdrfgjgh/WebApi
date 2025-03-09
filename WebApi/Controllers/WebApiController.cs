
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.AspNetCore.Authentication;
using WebApi.Repositories;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("WebApi")]
    public class WebApiController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<WebApiController> _logger;
        private readonly IAuthenticationService _authenticationService;
        private static List<WebApi> _webApi = new List<WebApi>();
        

        public WebApiController(IRepository repository, ILogger<WebApiController> logger, IAuthenticationService authenticationService)
        {
            _repository = repository;
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpGet(Name = "GetWebApi")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WebApi>>> Get()
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            _logger.LogInformation("Authenticated user Id: {UserId}", userId);

                var webApi = await _repository.GetAllAsync();
                return Ok(webApi);
        }
 


        [HttpGet("{WebApiId}", Name = "GetWebApiId")]
        [Authorize]
        public async Task<ActionResult<WebApi>> Get(Guid WebApiId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            _logger.LogInformation("Authenticated user ID: {UserId}", userId);

            var WebApi = await _repository.GetByIdAsync(WebApiId);
            _webApi.Add(WebApi);
            if (WebApi == null)
            {
                return NotFound();
            }
            return Ok(WebApi);
        }

        [HttpPost(Name = "PostWebApi")]
        [Authorize]
        public async Task<IActionResult> Post(WebApi webApi)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            _logger.LogInformation("Authenticated user ID: {UserId}", userId);

            webApi.id = Guid.NewGuid();
            await _repository.AddAsync(webApi);
            return CreatedAtRoute("GetWebApi", new { id = webApi.id }, webApi);
        }

        [HttpPut("{webApiId}", Name = "PutWebApi")]
        [Authorize]
        public async Task<ActionResult> Put(Guid webApiId, WebApi newWebApi)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            _logger.LogInformation("Authenticated user ID: {UserId}", userId);

            var existingWebApi = await _repository.GetByIdAsync(webApiId);
            if (existingWebApi == null)
            {
                return NotFound();
            }
            newWebApi.id = webApiId;
            await _repository.UpdateAsync(newWebApi);
            return CreatedAtRoute("GetWebApi", new { id = newWebApi.id }, newWebApi);
        }

        [HttpDelete("{webApiId}", Name = "DeleteWebApi")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid webApiId)
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            _logger.LogInformation("Authenticated user ID: {UserId}", userId);

            var existingWebApi = await _repository.GetByIdAsync(webApiId);
            if (existingWebApi == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(webApiId);
            return Ok(webApiId);
        }
        [HttpGet("GetUserId", Name = "GetUserId")]
        public ActionResult<string> GetUserId()
        {
            var userId = _authenticationService.GetCurrentAuthenticatedUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            return Ok(userId);
        }
    }
}
