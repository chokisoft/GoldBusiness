using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class UnidadMedidaService : IUnidadMedidaService
    {
        private readonly IUnidadMedidaRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public UnidadMedidaService(
            IUnidadMedidaRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _localizer = localizer;
        }

        public async Task<IEnumerable<UnidadMedidaDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(um => MapToDTO(um, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<UnidadMedidaDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<UnidadMedidaDTO> CreateAsync(UnidadMedidaDTO dto, string user, string lang = "es")
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

            var entity = new UnidadMedida(dto.Codigo, dto.Descripcion, creador);

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<UnidadMedidaDTO> UpdateAsync(int id, UnidadMedidaDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            if (entity.Codigo != dto.Codigo)
            {
                var existingWithNewCode = await _repo.GetByCodigoAsync(dto.Codigo, includeCanceled: true);

                if (existingWithNewCode != null && existingWithNewCode.Id != id)
                {
                    if (existingWithNewCode.Cancelado)
                    {
                        var errorMessage = $"Ya existe un registro cancelado con el código '{dto.Codigo}'. " +
                                         $"Considere reactivar el registro existente (ID: {existingWithNewCode.Id}).";
                        throw new InvalidOperationException(errorMessage);
                    }
                    else
                    {
                        var errorMessage = string.Format(_localizer["CodigoDuplicado"].Value, dto.Codigo);
                        throw new InvalidOperationException(errorMessage);
                    }
                }

                entity.SetCodigo(dto.Codigo);
            }

            entity.Update(dto.Descripcion, user);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<UnidadMedidaDTO?> SoftDeleteAsync(int id, string user)
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

        private static UnidadMedidaDTO? MapToDTO(UnidadMedida? um, string lang)
        {
            if (um == null) return null;

            return new UnidadMedidaDTO
            {
                Id = um.Id,
                Codigo = um.Codigo,
                Descripcion = um.GetDescripcion(lang),
                Cancelado = um.Cancelado,
                CreadoPor = um.CreadoPor,
                FechaHoraCreado = um.FechaHoraCreado,
                ModificadoPor = um.ModificadoPor,
                FechaHoraModificado = um.FechaHoraModificado
            };
        }
    }
}