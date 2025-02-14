﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Controllers;

[ApiController]
[Authorize(Roles = "VisitorRole, UserRole, AdminRole")]
public class DboWeatherForecastController : AppControllerBase<DboWeatherForecast>
{
    public DboWeatherForecastController(ICQSDataBroker dataBroker)
        : base(dataBroker)
    { }
}
