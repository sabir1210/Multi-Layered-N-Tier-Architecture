using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NTier.Presentation.Controllers
{

    [ApiController]
    [EnableCors]
    public class BaseController : ControllerBase
    {
        protected string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "";
    }
}
