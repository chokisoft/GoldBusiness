using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers;

/// <summary>
/// 🗂️ NOMENCLADORES - Catálogos y clasificadores del sistema
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "v2")]
public class NomencladoresController : ControllerBase
{
    private readonly IStringLocalizer<GoldBusiness.Domain.Resources.NomencladoresResources> _localizer;

    public NomencladoresController(IStringLocalizer<GoldBusiness.Domain.Resources.NomencladoresResources> localizer)
    {
        _localizer = localizer;
    }

    /// <summary>
    /// Obtener información sobre los nomencladores del sistema
    /// </summary>
    /// <remarks>
    /// Esta sección agrupa todos los catálogos y clasificadores del ERP:
    /// - Grupos de Cuentas, SubGrupos y Cuentas
    /// - Monedas y Conceptos de Ajuste
    /// - Transacciones
    /// - Establecimientos y Localidades
    /// - Clientes y Proveedores
    /// - Líneas, SubLíneas y Unidades de Medida
    /// - Productos y Fichas de Producto (BOM)
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
                new { code = "GrupoCuenta", name = _localizer["GrupoCuenta"].Value, route = "/api/GrupoCuenta" },
                new { code = "SubGrupoCuenta", name = _localizer["SubGrupoCuenta"].Value, route = "/api/SubGrupoCuenta" },
                new { code = "Cuenta", name = _localizer["Cuenta"].Value, route = "/api/Cuenta" },
                new { code = "Moneda", name = _localizer["Moneda"].Value, route = "/api/Moneda" },
                new { code = "ConceptoAjuste", name = _localizer["ConceptoAjuste"].Value, route = "/api/ConceptoAjuste" },
                new { code = "Transaccion", name = _localizer["Transaccion"].Value, route = "/api/Transaccion" },
                new { code = "Establecimiento", name = _localizer["Establecimiento"].Value, route = "/api/Establecimiento" },
                new { code = "Localidad", name = _localizer["Localidad"].Value, route = "/api/Localidad" },
                new { code = "Cliente", name = _localizer["Cliente"].Value, route = "/api/Cliente" },
                new { code = "Proveedor", name = _localizer["Proveedor"].Value, route = "/api/Proveedor" },
                new { code = "Linea", name = _localizer["Linea"].Value, route = "/api/Linea" },
                new { code = "SubLinea", name = _localizer["SubLinea"].Value, route = "/api/SubLinea" },
                new { code = "UnidadMedida", name = _localizer["UnidadMedida"].Value, route = "/api/UnidadMedida" },
                new { code = "Producto", name = _localizer["Producto"].Value, route = "/api/Producto" },
                new { code = "FichaProducto", name = _localizer["FichaProducto"].Value, route = "/api/FichaProducto" }
            },
            totalModules = 15,
            timestamp = DateTime.UtcNow
        });
    }
}