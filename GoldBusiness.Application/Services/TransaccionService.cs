using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class TransaccionService : ITransaccionService
    {
        private readonly ITransaccionRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public TransaccionService(
            ITransaccionRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _localizer = localizer;
        }

        public async Task<IEnumerable<TransaccionDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(t => MapToDTO(t, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<TransaccionDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<TransaccionDTO> CreateAsync(TransaccionDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";

            // Verificar si ya existe
            var existing = await _repo.GetByCodigoAsync(dto.Codigo);
            if (existing != null)
            {
                var errorMessage = string.Format(_localizer["CodigoDuplicado"].Value, dto.Codigo);
                throw new InvalidOperationException(errorMessage);
            }

            var entity = new Transaccion(dto.Codigo, dto.Descripcion, creador);

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<TransaccionDTO> UpdateAsync(int id, TransaccionDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            if (entity.Codigo != dto.Codigo)
            {
                var existingWithNewCode = await _repo.GetByCodigoAsync(dto.Codigo);

                if (existingWithNewCode != null && existingWithNewCode.Id != id)
                {
                    var errorMessage = string.Format(_localizer["CodigoDuplicado"].Value, dto.Codigo);
                    throw new InvalidOperationException(errorMessage);
                }

                entity.SetCodigo(dto.Codigo);
            }

            entity.Update(dto.Descripcion, user);
            entity.AddOrUpdateTranslation(lang, dto.Descripcion, user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
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

        private static TransaccionDTO? MapToDTO(Transaccion? t, string lang)
        {
            if (t == null) return null;

            return new TransaccionDTO
            {
                Id = t.Id,
                Codigo = t.Codigo,
                Descripcion = t.GetDescripcion(lang),
                CreadoPor = t.CreadoPor,
                FechaHoraCreado = t.FechaHoraCreado,
                ModificadoPor = t.ModificadoPor,
                FechaHoraModificado = t.FechaHoraModificado
            };
        }
    }
}