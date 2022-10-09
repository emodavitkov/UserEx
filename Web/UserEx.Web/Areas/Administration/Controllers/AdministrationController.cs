namespace UserEx.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using UserEx.Common;
    using UserEx.Web.Controllers;

    using static UserEx.Common.GlobalConstants;

    [Authorize(Roles = AdministratorRoleName)]
    [Area(AreaName)]
    public class AdministrationController : BaseController
    {
    }
}
