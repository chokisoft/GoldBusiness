using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Helpers;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class CodigoPostalService : ICodigoPostalService
    {
        private readonly ICodigoPostalRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public CodigoPostalService(
            ICodigoPostalRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _localizer = localizer;
        }

        public async Task<IEnumerable<CodigoPostalDTO>> GetAllAsync()
            => (await _repo.GetAllAsync())
                .Select(g => MapToDTO(g, "es"))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<IEnumerable<CodigoPostalDTO>> GetByMunicipioIdAsync(int municipioId, string lang = "es")
            => (await _repo.GetByMunicipioIdAsync(municipioId))
            .Select(m => MapToDTO(m, lang))
            .ToList();

        public async Task<CodigoPostalDTO?> GetByIdAsync(int id, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity != null ? MapToDTO(entity, lang) : null;
        }

        public async Task<CodigoPostalDTO> CreateAsync(CodigoPostalDTO dto, string lang, string creadoPor)
        {
            var existing = await _repo.GetByCodigoAsync(dto.Codigo, includeCanceled: true);

            if (existing != null)
            {
                if (existing.Cancelado)
                {
                    existing.Activar(creadoPor);
                    existing.Actualizar(dto.Codigo, dto.MunicipioId, creadoPor);
                    await _repo.UpdateAsync(existing);
                    return MapToDTO(existing, lang);
                }
                throw new InvalidOperationException(
                    string.Format(_localizer["CodigoDuplicado"].Value, dto.Codigo));
            }

            var entity = new CodigoPostal(dto.Codigo, dto.MunicipioId, creadoPor);

            await _repo.AddAsync(entity);
            return MapToDTO(entity, lang);
        }

        public async Task<CodigoPostalDTO> UpdateAsync(int id, CodigoPostalDTO dto, string lang, string creadoPor)
        {
            var entity = await _repo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"CodigoPostal con Id={id} no encontrado.");

            if (entity.Codigo != dto.Codigo)
            {
                var conflict = await _repo.GetByCodigoAsync(dto.Codigo, includeCanceled: true);
                if (conflict != null && conflict.Id != id)
                    throw new InvalidOperationException(
                        string.Format(_localizer["CodigoDuplicado"].Value, dto.Codigo));
            }

            entity.Actualizar(dto.Codigo, dto.MunicipioId, creadoPor);

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang);
        }

        public async Task<CodigoPostalDTO?> SoftDeleteAsync(int id, string user)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Cancelar(user);
            await _repo.UpdateAsync(entity);
            return MapToDTO(entity);
        }

        private static CodigoPostalDTO MapToDTO(CodigoPostal cp, string lang = "es") => new()
        {
            Id = cp.Id,
            Codigo = cp.Codigo,
            MunicipioId = cp.MunicipioId,
            MunicipioCodigo = cp.Municipio?.Codigo ?? string.Empty,
            MunicipioDescripcion = cp.Municipio?.Descripcion ?? string.Empty,
            Cancelado = cp.Cancelado,
            CreadoPor = cp.CreadoPor,
            FechaHoraCreado = cp.FechaHoraCreado,
            ModificadoPor = cp.ModificadoPor,
            FechaHoraModificado = cp.FechaHoraModificado
        };
    }
}