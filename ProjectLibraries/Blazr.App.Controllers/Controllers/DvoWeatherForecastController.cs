﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Controllers;

[ApiController]
[Authorize(Roles = "VisitorRole, UserRole, AdminRole")]

public class DvoWeatherForecastController : AppControllerBase<DvoWeatherForecast>
{
    public DvoWeatherForecastController(ICQSDataBroker dataBroker)
        : base(dataBroker)
    { }

}
