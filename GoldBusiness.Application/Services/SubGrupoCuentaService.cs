using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;

namespace GoldBusiness.Application.Services
{
    public class SubGrupoCuentaService : ISubGrupoCuentaService
    {
        private readonly ISubGrupoCuentaRepository _repo;

        public SubGrupoCuentaService(ISubGrupoCuentaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SubGrupoCuentaDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(s => MapToDTO(s, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<SubGrupoCuentaDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<SubGrupoCuentaDTO> CreateAsync(SubGrupoCuentaDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";
            var entity = new SubGrupoCuenta(dto.Codigo, dto.GrupoCuenta, dto.Descripcion, dto.Deudora, creador);

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<SubGrupoCuentaDTO> UpdateAsync(int id, SubGrupoCuentaDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.Update(dto.Descripcion, dto.GrupoCuenta, dto.Deudora, user);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<SubGrupoCuentaDTO?> SoftDeleteAsync(int id, string user)
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
            if (string.IsNullOrWhiteSpace(descripcion)) throw new ArgumentException("Descripción requerida.", nameof(descripcion));

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.AddOrUpdateTranslation(lang, descripcion, user ?? "system");
            await _repo.UpdateAsync(entity);
        }

        private static SubGrupoCuentaDTO? MapToDTO(SubGrupoCuenta? s, string lang)
        {
            if (s == null) return null;

            return new SubGrupoCuentaDTO
            {
                Id = s.Id,
                Codigo = s.Codigo,
                GrupoCuenta = s.GrupoCuentaId,
                GrupoCuentaDescripcion = s.GrupoCuenta?.GetDescripcion(lang),
                Descripcion = s.GetDescripcion(lang),
                Deudora = s.Deudora,
                Cancelado = s.Cancelado,
                CreadoPor = s.CreadoPor,
                FechaHoraCreado = s.FechaHoraCreado,
                ModificadoPor = s.ModificadoPor,
                FechaHoraModificado = s.FechaHoraModificado
            };
        }
    }
}