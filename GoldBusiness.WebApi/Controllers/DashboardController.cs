using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para el Dashboard del sistema
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPAdminOrFullAccess")]
    public class DashboardController(
        IDashboardService service,
        ILogger<DashboardController> logger,
        IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : BaseEntityController(localizer)
    {
        private readonly IDashboardService _service = service;
        private readonly ILogger<DashboardController> _logger = logger;

        /// <summary>
        /// Obtener todos los datos del dashboard.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<DashboardDataDTO>> Get()
        {
            try
            {
                var lang = GetCurrentLanguage();
                _logger.LogInformation($"📊 GET /api/dashboard - Idioma: {lang}");

                var data = await _service.GetDashboardDataAsync(lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener datos del dashboard");
                return StatusCode(500, new { message = "Error al obtener datos del dashboard", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtener solo las estadísticas
        /// </summary>
        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDTO>> GetStats()
        {
            try
            {
                _logger.LogInformation("📊 GET /api/dashboard/stats");

                var stats = await _service.GetStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener estadísticas");
                return StatusCode(500, new { message = "Error al obtener estadísticas", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtener solo las actividades recientes
        /// </summary>
        [HttpGet("recent-activities")]
        public async Task<ActionResult<List<RecentActivityDTO>>> GetRecentActivities()
        {
            try
            {
                var lang = GetCurrentLanguage();
                _logger.LogInformation($"📊 GET /api/dashboard/recent-activities - Idioma: {lang}");

                var activities = await _service.GetRecentActivitiesAsync(lang);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al obtener actividades recientes");
                return StatusCode(500, new { message = "Error al obtener actividades recientes", error = ex.Message });
            }
        }
    }
}