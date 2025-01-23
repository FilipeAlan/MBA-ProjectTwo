using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PCF.API.Controllers.Base
{
    [ApiController]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
