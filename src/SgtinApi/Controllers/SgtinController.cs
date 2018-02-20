using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SgtinAppCore;
using SgtinAppCore.Interfaces;

namespace SgtinApi.Controllers
{
    [Route("api/[controller]")]
    public class SgtinController : Controller
    {
        private readonly ISgtinFactoryService sgtinFactory;
        private readonly ISearchService search;
        private readonly ILogger<SgtinController> logger;

        public SgtinController(
            ISgtinFactoryService sgtinFactory, 
            ISearchService search, 
            ILogger<SgtinController> logger)
        {
            this.sgtinFactory = sgtinFactory;
            this.search = search;
            this.logger = logger;
        }

        private IActionResult TryCreateResult(Func<IActionResult> create)
        {
            try
            {                
                return create();
            }
            catch (SgtinException exception)
            {
                logger.LogError(exception, exception.Message);
                return new BadRequestObjectResult(new { message = exception.Message });
            }
        }

        // GET api/sgtin/3098D0A357783C0034E9DF74
        [HttpGet("{inputHex}")]
        public IActionResult Get(string inputHex)
        {
            return TryCreateResult(() =>
            {
                var model = sgtinFactory.CreateFromHex(inputHex);
                return new OkObjectResult(model);
            });
        }

        // GET api/sgtin/3098D0A357783C0034E9DF74/Milka Oreo
        [HttpGet("{inputHex}/{searchQuery}")]
        public IActionResult Get(string inputHex, string searchQuery)
        {
            return TryCreateResult(() =>
            {
                var result = search.Compare(inputHex, searchQuery);
                if (result)
                {
                    return new OkResult();
                }
                return new NotFoundResult();
            });
        }
    }
}
