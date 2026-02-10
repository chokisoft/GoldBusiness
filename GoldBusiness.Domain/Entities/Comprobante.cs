using GoldBusiness.Domain.Exceptions;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Domain.Helpers;

namespace GoldBusiness.Domain.Entities
{
    public class Comprobante : BaseEntity
    {
        private readonly HashSet<ComprobanteTranslation> _translations = new();
        private readonly HashSet<ComprobanteDetalle> _detalles = new();

        public int Id { get; private set; }
        public int EstablecimientoId { get; private set; }
        public string NoComprobante { get; private set; } = string.Empty;
        public DateTime Fecha { get; private set; }
        public string Observaciones { get; private set; } = string.Empty;
        public bool Automatico { get; private set; }
        public bool Posteado { get; private set; }
        public bool Cancelado { get; private set; }

        // Propiedades de navegación
        public Establecimiento Establecimiento { get; private set; } = null!;

        // Colecciones de navegación (read-only)
        public IReadOnlyCollection<ComprobanteTranslation> Translations => _translations;
        public IReadOnlyCollection<ComprobanteDetalle> Detalles => _detalles;

        // Constructor protegido para EF Core
        protected Comprobante() { }

        // Constructor con validaciones
        public Comprobante(
            int establecimientoId,
            string noComprobante,
            DateTime fecha,
            bool automatico,
            string creadoPor)
        {
            EstablecimientoId = establecimientoId;
            SetNoComprobante(noComprobante);
            SetFecha(fecha);
            Automatico = automatico;
            EstablecerCreador(creadoPor);
            Posteado = false;
            Cancelado = false;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetNoComprobante(string noComprobante)
        {
            if (string.IsNullOrWhiteSpace(noComprobante))
                throw new DomainException("El número de comprobante es obligatorio.");

            if (noComprobante.Length > 50)
                throw new DomainException("El número de comprobante no puede exceder 50 caracteres.");

            NoComprobante = noComprobante.Trim();
        }

        public void SetFecha(DateTime fecha)
        {
            if (fecha == default)
                throw new DomainException("La fecha es obligatoria.");

            if (fecha < new DateTime(2000, 1, 1))
                throw new DomainException("La fecha no puede ser anterior al año 2000.");

            if (fecha > DateTime.UtcNow.AddYears(1))
                throw new DomainException("La fecha no puede ser más de 1 año en el futuro.");

            Fecha = fecha;
        }

        public void SetObservaciones(string observaciones)
        {
            if (!string.IsNullOrWhiteSpace(observaciones) && observaciones.Length > 1024)
                throw new DomainException("Las observaciones no pueden exceder 1024 caracteres.");

            Observaciones = observaciones?.Trim() ?? string.Empty;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🌍 MÉTODOS DE TRADUCCIÓN
        // ═══════════════════════════════════════════════════════════════

        public void AddOrUpdateTranslation(string language, string observaciones, string usuario)
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var existing = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.SetObservaciones(observaciones, usuario);
            }
            else
            {
                _translations.Add(new ComprobanteTranslation(Id, lang, observaciones, usuario));
            }
        }

        public string GetObservaciones(string language, string fallback = "es")
        {
            var lang = LanguageHelper.NormalizeLang(language);
            var fb = LanguageHelper.NormalizeLang(fallback);

            var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
            if (match != null && !string.IsNullOrWhiteSpace(match.Observaciones))
                return match.Observaciones;

            if (!string.IsNullOrWhiteSpace(Observaciones))
                return Observaciones;

            var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
            if (fallbackMatch != null) return fallbackMatch.Observaciones;

            return string.Empty;
        }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
        // ═══════════════════════════════════════════════════════════════

        public void Postear(string modificadoPor)
        {
            if (Posteado)
                throw new DomainException("El comprobante ya está posteado.");

            if (Cancelado)
                throw new DomainException("No se puede postear un comprobante cancelado.");

            if (!_detalles.Any(d => !d.Cancelado))
                throw new DomainException("No se puede postear un comprobante sin detalles.");

            if (!EstaBalanceado())
                throw new DomainException("No se puede postear un comprobante desbalanceado.");

            Posteado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void DesPostear(string modificadoPor)
        {
            if (!Posteado)
                throw new DomainException("El comprobante no está posteado.");

            Posteado = false;
            ActualizarAuditoria(modificadoPor);
        }

        public void SoftDelete(string modificadoPor)
        {
            if (Cancelado)
                throw new DomainException("El comprobante ya está cancelado.");

            if (Posteado)
                throw new DomainException("No se puede cancelar un comprobante posteado.");

            Cancelado = true;
            ActualizarAuditoria(modificadoPor);
        }

        public void Reactivar(string modificadoPor)
        {
            if (!Cancelado)
                throw new DomainException("El comprobante no está cancelado.");

            Cancelado = false;
            ActualizarAuditoria(modificadoPor);
        }

        // ═══════════════════════════════════════════════════════════════
        // 📊 MÉTODOS DE CONSULTA Y CÁLCULO
        // ═══════════════════════════════════════════════════════════════

        public decimal GetTotalDebito()
        {
            return _detalles.Where(d => !d.Cancelado).Sum(d => d.Debito);
        }

        public decimal GetTotalCredito()
        {
            return _detalles.Where(d => !d.Cancelado).Sum(d => d.Credito);
        }

        public bool EstaBalanceado()
        {
            return Math.Abs(GetTotalDebito() - GetTotalCredito()) < 0.01m;
        }
    }
}