
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using WebApi.Repositories;
namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebApiController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<WebApiController> _logger;
        private static List<WebApi> _webApi = new List<WebApi>();

        public WebApiController(IRepository repository, ILogger<WebApiController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet(Name = "GetWebApi")]
        public async Task<ActionResult<IEnumerable<WebApi>>> Get()
        {
            var webApi = await _repository.GetAllAsync();
            return Ok(webApi);
        }


        [HttpGet("{id}", Name = "GetWebApiId")]
        public async Task<ActionResult<WebApi>> Get(Guid WebApiId)
        {
            var WebApi = await _repository.GetByIdAsync(WebApiId);
            _webApi.Add(WebApi);
            if (WebApi == null)
            {
                return NotFound();
            }
            return Ok(WebApi);
        }

        [HttpPost(Name = "PostWebApi")]
        public async Task<IActionResult> Post(WebApi webApi)
        {
            webApi.id = Guid.NewGuid();
            await _repository.AddAsync(webApi);
            return CreatedAtRoute("GetWebApi", new { id = webApi.id }, webApi);
        }

        [HttpPut("{webApiId}", Name = "PutWebApi")]
        public async Task<ActionResult> Put(Guid webApiId, WebApi newWebApi)
        {
            var existingWebApi = await _repository.GetByIdAsync(webApiId);
            if (existingWebApi == null)
            {
                return NotFound();
            }
            newWebApi.id = webApiId;
            await _repository.UpdateAsync(newWebApi);
            return CreatedAtRoute("GetWereldBouwer", new { id = newWebApi.id }, newWebApi);
        }

        [HttpDelete("{webApiId}", Name = "DeleteWebApi")]
        public async Task<IActionResult> Delete(Guid webApiId)
        {
            var existingWebApi = await _repository.GetByIdAsync(webApiId);
            if (existingWebApi == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(webApiId);
            return Ok(webApiId);
        }
    }
}
