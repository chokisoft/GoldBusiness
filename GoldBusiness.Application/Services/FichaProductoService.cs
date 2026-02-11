using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class FichaProductoService : IFichaProductoService
    {
        private readonly IFichaProductoRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public FichaProductoService(
            IFichaProductoRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _localizer = localizer;
        }

        public async Task<IEnumerable<FichaProductoDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(f => MapToDTO(f, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<IEnumerable<FichaProductoDTO>> GetByProductoIdAsync(int productoId, string lang = "es")
            => (await _repo.GetByProductoIdAsync(productoId))
                .Select(f => MapToDTO(f, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<IEnumerable<FichaProductoDTO>> GetByLocalidadIdAsync(int localidadId, string lang = "es")
            => (await _repo.GetByLocalidadIdAsync(localidadId))
                .Select(f => MapToDTO(f, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<FichaProductoDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<FichaProductoDTO> CreateAsync(FichaProductoDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";

            // Verificar si ya existe la composición
            var exists = await _repo.ExistsComposicionAsync(dto.ProductoId, dto.ComponenteId, dto.LocalidadId);
            if (exists)
            {
                throw new InvalidOperationException(
                    $"Ya existe una ficha con esta composición (Producto: {dto.ProductoId}, Componente: {dto.ComponenteId}, Localidad: {dto.LocalidadId}).");
            }

            var entity = new FichaProducto(
                dto.ProductoId,
                dto.LocalidadId,
                dto.ComponenteId,
                dto.Cantidad,
                creador);

            await _repo.AddAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<FichaProductoDTO> UpdateAsync(int id, FichaProductoDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            // Verificar si cambió la composición y si ya existe
            if (entity.ProductoId != dto.ProductoId ||
                entity.ComponenteId != dto.ComponenteId ||
                entity.LocalidadId != dto.LocalidadId)
            {
                var exists = await _repo.ExistsComposicionAsync(
                    dto.ProductoId, dto.ComponenteId, dto.LocalidadId, id);

                if (exists)
                {
                    throw new InvalidOperationException(
                        $"Ya existe una ficha con esta composición (Producto: {dto.ProductoId}, Componente: {dto.ComponenteId}, Localidad: {dto.LocalidadId}).");
                }
            }

            entity.Update(dto.Cantidad, dto.LocalidadId, user);

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<FichaProductoDTO?> SoftDeleteAsync(int id, string user)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            entity.SoftDelete(user);

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, "es");
        }

        private static FichaProductoDTO? MapToDTO(FichaProducto? f, string lang)
        {
            if (f == null) return null;

            return new FichaProductoDTO
            {
                Id = f.Id,
                ProductoId = f.ProductoId,
                ProductoCodigo = f.Producto?.Codigo ?? string.Empty,
                ProductoDescripcion = f.Producto?.GetDescripcion(lang) ?? string.Empty,
                LocalidadId = f.LocalidadId,
                LocalidadCodigo = f.Localidad?.Codigo ?? string.Empty,
                LocalidadDescripcion = f.Localidad?.GetDescripcion(lang) ?? string.Empty,
                ComponenteId = f.ComponenteId,
                ComponenteCodigo = f.Componente?.Codigo ?? string.Empty,
                ComponenteDescripcion = f.Componente?.GetDescripcion(lang) ?? string.Empty,
                Cantidad = f.Cantidad,
                Cancelado = f.Cancelado,
                CreadoPor = f.CreadoPor,
                FechaHoraCreado = f.FechaHoraCreado,
                ModificadoPor = f.ModificadoPor,
                FechaHoraModificado = f.FechaHoraModificado
            };
        }
    }
}