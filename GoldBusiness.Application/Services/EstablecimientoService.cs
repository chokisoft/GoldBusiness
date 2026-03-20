using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class EstablecimientoService : IEstablecimientoService
    {
        private readonly IEstablecimientoRepository _repo;
        private readonly IPaisRepository _paisRepo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public EstablecimientoService(
            IEstablecimientoRepository repo,
            IPaisRepository paisRepo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _paisRepo = paisRepo;
            _localizer = localizer;
        }

        public async Task<IEnumerable<EstablecimientoDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(e => MapToDTO(e, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<(IEnumerable<EstablecimientoDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? negocioId = null, string lang = "es")
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, termino, negocioId);
            var dtos = items.Select(s => MapToDTO(s, lang))
                            .Where(dto => dto is not null)
                            .Select(dto => dto!)
                            .ToList();
            return (dtos, total);
        }

        public async Task<IEnumerable<EstablecimientoDTO>> GetByNegocioIdAsync(int negocioId, string lang = "es")
            => (await _repo.GetByNegocioIdAsync(negocioId))
                .Select(l => MapToDTO(l, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<EstablecimientoDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<EstablecimientoDTO> CreateAsync(EstablecimientoDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";

            var (existe, estaCancelado, existingEntity) = await CodigoValidationHelper
                .ValidateCodigoForCreateAsync(_repo, dto.Codigo);

            if (existe)
            {
                if (estaCancelado && existingEntity != null)
                {
                    existingEntity.Reactivar(dto.Descripcion, creador);
                    existingEntity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
                    await _repo.UpdateAsync(existingEntity);

                    return MapToDTO(existingEntity, lang)!;
                }
                else
                {
                    var errorMessage = CodigoValidationHelper.GetDuplicateCodeErrorMessage(
                        _localizer, dto.Codigo, false);
                    throw new InvalidOperationException(errorMessage);
                }
            }

            // Obtener país si existe para validar teléfono
            Pais? pais = null;
            if (dto.PaisId.HasValue)
            {
                pais = await _paisRepo.GetByIdAsync(dto.PaisId.Value);
            }

            var entity = new Establecimiento(
                dto.Codigo,
                dto.Descripcion,
                dto.NegocioId,
                dto.Direccion,
                dto.Telefono,
                dto.PaisId,
                dto.ProvinciaId,
                dto.MunicipioId,
                dto.CodigoPostalId,
                creador);

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<EstablecimientoDTO> UpdateAsync(int id, EstablecimientoDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            // Normalizar el código entrante: trim y upper para evitar conflictos por espacios o case-sensitivity
            var incomingCodigo = dto.Codigo?.Trim() ?? string.Empty;
            var codigoUpper = incomingCodigo.ToUpperInvariant();

            // Solo validar si realmente se intenta cambiar el código (comparación case-insensitive)
            if (!string.Equals(entity.Codigo, incomingCodigo, StringComparison.OrdinalIgnoreCase))
            {
                // Usar helper centralizado que incluye registros cancelados
                var (existe, estaCancelado, existingId) = await CodigoValidationHelper
                    .ValidateCodigoForUpdateAsync(_repo, codigoUpper, id);

                if (existe)
                {
                    if (estaCancelado && existingId.HasValue)
                    {
                        var errorMessage = CodigoValidationHelper.GetDuplicateCodeErrorMessage(_localizer, dto.Codigo, true, existingId);
                        throw new InvalidOperationException(errorMessage);
                    }
                    else
                    {
                        var errorMessage = string.Format(_localizer["CodigoDuplicado"].Value, dto.Codigo);
                        throw new InvalidOperationException(errorMessage);
                    }
                }

                // Aplicar el nuevo código normalizado
                entity.SetCodigo(codigoUpper);
            }

            // Obtener país si existe para validar teléfono
            Pais? pais = null;
            if (dto.PaisId.HasValue)
            {
                pais = await _paisRepo.GetByIdAsync(dto.PaisId.Value);
            }

            entity.Actualizar(
                dto.Descripcion,
                dto.Direccion,
                dto.Telefono,
                dto.PaisId,
                dto.ProvinciaId,
                dto.MunicipioId,
                dto.CodigoPostalId,
                pais,
                user);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<EstablecimientoDTO?> SoftDeleteAsync(int id, string user)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            entity.SoftDelete(user);

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, "es");
        }

        public async Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user)
        {
            if (string.IsNullOrWhiteSpace(lang)) lang = "es";
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("Descripción requerida.", nameof(descripcion));

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.AddOrUpdateTranslation(lang, descripcion, user ?? "system");
            await _repo.UpdateAsync(entity);
        }

        private static EstablecimientoDTO? MapToDTO(Establecimiento? e, string lang)
        {
            if (e == null) return null;

            return new EstablecimientoDTO
            {
                Id = e.Id,
                Codigo = e.Codigo,
                NegocioId = e.NegocioId,
                NegocioDescripcion = e.Negocio?.GetNombreNegocio(lang) ?? string.Empty,
                Descripcion = e.GetDescripcion(lang),
                Direccion = e.Direccion,
                Telefono = e.Telefono,
                PaisId = e.PaisId,
                PaisDescripcion = e.Pais?.GetDescripcion(lang),
                ProvinciaId = e.ProvinciaId,
                ProvinciaDescripcion = e.Provincia?.GetDescripcion(lang),
                MunicipioId = e.MunicipioId,
                MunicipioDescripcion = e.Municipio?.GetDescripcion(lang),
                CodigoPostalId = e.CodigoPostalId,
                CodigoPostalCodigo = e.CodigoPostal?.Codigo,
                Activo = e.Activo,
                Cancelado = e.Cancelado,
                CreadoPor = e.CreadoPor,
                FechaHoraCreado = e.FechaHoraCreado,
                ModificadoPor = e.ModificadoPor,
                FechaHoraModificado = e.FechaHoraModificado
            };
        }
    }
}