using GoldBusiness.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPAdminOrFullAccess")]
    public class FormaJuridicaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FormaJuridicaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> Get([FromQuery] string? lang = "es")
        {
            var l = (lang ?? "es").Split('-', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();

            var items = await _context.FormaJuridica
                .Include(f => f.Translations)
                .Where(f => !f.Cancelado)
                .Select(f => new
                {
                    id = f.Id,
                    descripcion = f.Translations
                        .Where(t => t.Language.ToLower() == l)
                        .Select(t => t.Descripcion)
                        .FirstOrDefault() ?? f.Descripcion
                })
                .OrderBy(f => f.descripcion)
                .ToListAsync();

            return Ok(items);
        }
    }
}