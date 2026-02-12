using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers;

/// <summary>
/// ⚙️ CONFIGURACIÓN - Configuración general del sistema
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "v2")]
public class ConfiguracionController : ControllerBase
{
    private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ConfiguracionResources> _localizer;

    public ConfiguracionController(IStringLocalizer<GoldBusiness.Domain.Resources.ConfiguracionResources> localizer)
    {
        _localizer = localizer;
    }

    /// <summary>
    /// Obtener información sobre las configuraciones del sistema
    /// </summary>
    /// <remarks>
    /// Esta sección agrupa todas las configuraciones administrativas del ERP:
    /// - Configuración general del sistema
    /// - Parámetros de la empresa
    /// - Configuraciones operativas
    /// </remarks>
    /// <response code="200">Información devuelta correctamente</response>
    [HttpGet("info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            section = _localizer["Section"].Value,
            description = _localizer["Description"].Value,
            remarks = _localizer["InfoRemarks"].Value,
            version = "2.0",
            culture = System.Globalization.CultureInfo.CurrentUICulture.Name,
            modules = new[]
            {
                new { code = "SystemConfiguration", name = _localizer["SystemConfiguration"].Value, route = "/api/SystemConfiguration" }
            },
            totalModules = 1,
            timestamp = DateTime.UtcNow
        });
    }
}