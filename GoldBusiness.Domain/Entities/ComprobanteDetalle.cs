using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class ComprobanteDetalle : BaseEntity
    {
        private readonly HashSet<ComprobanteDetalleTranslation> _translations = new();

        public int Id { get; private set; }
        public int ComprobanteId { get; private set; }
        public int CuentaId { get; private set; }
        public string Departamento { get; private set; } = string.Empty;
        public decimal Debito { get; private set; }
        public decimal Credito { get; private set; }
        public decimal Parcial { get; private set; }
        public string Nota { get; private set; } = string.Empty;
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Comprobante Comprobante { get; private set; } = null!;
        public Cuenta Cuenta { get; private set; } = null!;

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<ComprobanteDetalleTranslation> Translations => _translations;

        // Constructor protegido para EF Core
        protected ComprobanteDetalle() { }

        // Constructor con validaciones
        public ComprobanteDetalle(
            int comprobanteId,
            int cuentaId,
            decimal debito,
            decimal credito,
            string creadoPor)
        {
            ComprobanteId = comprobanteId;
            CuentaId = cuentaId;
            SetDebito(debito);
            SetCredito(credito);
            EstablecerCreador(creadoPor);
            Cancelado = false;
            Parcial = 0;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetDebito(decimal debito)
        {
            if (debito < 0)
                throw new DomainException("El débito no puede ser negativo.");

            if (debito > 0 && Credito > 0)
                throw new DomainException("No se puede tener débito y crédito simultáneamente.");

            Debito = debito;
        }

        public void SetCredito(decimal credito)
        {
            if (credito < 0)
                throw new DomainException("El crédito no puede ser negativo.");

            if (credito > 0 && Debito > 0)
                throw new DomainException("No se puede tener crédito y débito simultáneamente.");

            Credito = credito;
        }

        public void SetDepartamento(string departamento)
        {
            if (!string.IsNullOrWhiteSpace(departamento) && departamento.Length > 50)
                throw new DomainException("El departamento no puede exceder 50 caracteres.");

            Departamento = departamento?.Trim() ?? string.Empty;
        }

        public void SetNota(string nota)
        {
            if (!string.IsNullOrWhiteSpace(nota) && nota.Length > 512)
                throw new DomainException("La nota no puede exceder 512 caracteres.");

            Nota = nota?.Trim() ?? string.Empty;
        }

        public void SetParcial(decimal parcial)
        {
            Parcial = parcial;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🌍 MÉTODOS DE TRADUCCIÓN
        // ═══════════════════════════════════════════════════════════════

        public void AddOrUpdateTranslation(string language, string nota, string usuario)
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var existing = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.SetNota(nota, usuario);
            }
            else
            {
                _translations.Add(new ComprobanteDetalleTranslation(Id, lang, nota, usuario));
            }
        }

        public string GetNota(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Nota))
                return match.Nota;

            if (!string.IsNullOrWhiteSpace(Nota))
                return Nota;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Nota;

            return string.Empty;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Update(
            int cuentaId,
            decimal debito,
            decimal credito,
            string departamento,
            string nota,
            decimal parcial,
            string modificadoPor)
        {
            CuentaId = cuentaId;
            SetDebito(debito);
            SetCredito(credito);
            SetDepartamento(departamento);
            SetNota(nota);
            SetParcial(parcial);
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("El detalle ya está cancelado.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string modificadoPor)
        {
            if (!Cancelado)
                throw new DomainException("El detalle no está cancelado.");

            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA
        // ═══════════════════════════════════════════════════════════════

        public decimal GetMonto()
        {
            return Debito > 0 ? Debito : Credito;
        }

        public bool EsDebito()
        {
            return Debito > 0;
        }

        public bool EsCredito()
        {
            return Credito > 0;
        }
    }
}