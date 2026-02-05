    using GoldBusiness.Domain.Exceptions;
    using GoldBusiness.Domain.Translation;

    namespace GoldBusiness.Domain.Entities
    {
        public class Cuenta
        {
            private readonly HashSet<CuentaTranslation> _translations = new();

            public int Id { get; private set; }
            public string Codigo { get; private set; } = string.Empty;
            public string Descripcion { get; private set; } = string.Empty;
            public int SubGrupoCuentaId { get; private set; }
            public SubGrupoCuenta SubGrupoCuenta { get; private set; } = null!;
            public bool Cancelado { get; private set; }
            public string CreadoPor { get; private set; } = string.Empty;
            public DateTime FechaHoraCreado { get; private set; }
            public string ModificadoPor { get; private set; } = string.Empty;
            public DateTime? FechaHoraModificado { get; private set; }

            public IReadOnlyCollection<CuentaTranslation> Translations => _translations;
            public IReadOnlyCollection<Localidad> LocalidadCuentaInventarioNavigation { get; } = new HashSet<Localidad>();
            public IReadOnlyCollection<Localidad> LocalidadCuentaCostoNavigation { get; } = new HashSet<Localidad>();
            public IReadOnlyCollection<Localidad> LocalidadCuentaVentaNavigation { get; } = new HashSet<Localidad>();
            public IReadOnlyCollection<Localidad> LocalidadCuentaDevolucionNavigation { get; } = new HashSet<Localidad>();
            public IReadOnlyCollection<SystemConfiguration> ConfiguracionCuentaPagarNavigation { get; } = new HashSet<SystemConfiguration>();
            public IReadOnlyCollection<SystemConfiguration> ConfiguracionCuentaCobrarNavigation { get; } = new HashSet<SystemConfiguration>();

        // Constructor protegido para EF Core
        protected Cuenta() { }

            // Constructor con validaciones
            public Cuenta(string codigo, string descripcion, int subGrupoCuentaId, string creadoPor)
            {
                SetCodigo(codigo);
                SetDescripcion(descripcion);

                SubGrupoCuentaId = subGrupoCuentaId;
                CreadoPor = creadoPor ?? throw new ArgumentNullException(nameof(creadoPor));
                FechaHoraCreado = DateTime.UtcNow;
                Cancelado = false;
            }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 MÉTODOS DE DOMINIO - VALIDACIONES
        // ═══════════════════════════════════════════════════════════════

        public void SetCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 8)
                throw new DomainException("El código debe tener exactamente 8 caracteres.");

            if (!codigo.All(char.IsDigit))
                throw new DomainException("El código debe ser numérico (00000000-99999999).");

            Codigo = codigo;
        }

        public void SetDescripcion(string descripcion)
            {
                if (string.IsNullOrWhiteSpace(descripcion))
                    throw new DomainException("La descripción es obligatoria.");
            
                if (descripcion.Length > 256) // ✅ Agregada validación de longitud
                    throw new DomainException("La descripción no puede exceder 256 caracteres.");

                Descripcion = descripcion.Trim(); // ✅ Agregado Trim()
            }

            // ═══════════════════════════════════════════════════════════════
            // 🌍 MÉTODOS DE TRADUCCIÓN
            // ═══════════════════════════════════════════════════════════════

            public void AddOrUpdateTranslation(string language, string descripcion, string usuario)
            {
                var lang = NormalizeLang(language);
                var existing = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
                if (existing != null)
                {
                    existing.SetDescripcion(descripcion, usuario);
                }
                else
                {
                    _translations.Add(new CuentaTranslation(Id, lang, descripcion, usuario));
                }
            }

            public string GetDescripcion(string language, string fallback = "es")
            {
                var lang = NormalizeLang(language);
                var fb = NormalizeLang(fallback);

                var match = _translations.FirstOrDefault(t => string.Equals(t.Language, lang, StringComparison.OrdinalIgnoreCase));
                if (match != null && !string.IsNullOrWhiteSpace(match.Descripcion))
                    return match.Descripcion;

                if (!string.IsNullOrWhiteSpace(Descripcion))
                    return Descripcion;

                var fallbackMatch = _translations.FirstOrDefault(t => string.Equals(t.Language, fb, StringComparison.OrdinalIgnoreCase));
                if (fallbackMatch != null) return fallbackMatch.Descripcion;

                return string.Empty;
            }

            // ═══════════════════════════════════════════════════════════════
            // 🔧 MÉTODOS DE ACTUALIZACIÓN Y ESTADO
            // ═══════════════════════════════════════════════════════════════

            public void Update(string descripcion, int subGrupoCuentaId, string modificadoPor)
            {
                SetDescripcion(descripcion);
                SubGrupoCuentaId = subGrupoCuentaId;
                ActualizarAuditoria(modificadoPor); // ✅ Usa método privado
            }

            public void SoftDelete(string modificadoPor)
            {
                if (Cancelado) // ✅ Agregada validación
                    throw new DomainException("La cuenta ya está cancelada.");
            
                Cancelado = true;
                ActualizarAuditoria(modificadoPor); // ✅ Usa método privado
            }

            public void Reactivar(string modificadoPor) // ✅ Método nuevo
            {
                if (!Cancelado)
                    throw new DomainException("La cuenta no está cancelada.");
            
                Cancelado = false;
                ActualizarAuditoria(modificadoPor);
            }

            // ═══════════════════════════════════════════════════════════════
            // 🔧 MÉTODOS PRIVADOS
            // ═══════════════════════════════════════════════════════════════

            private void ActualizarAuditoria(string usuario) // ✅ Método privado nuevo
            {
                ModificadoPor = usuario ?? throw new ArgumentNullException(nameof(usuario));
                FechaHoraModificado = DateTime.UtcNow;
            }

            private static string NormalizeLang(string? lang)
            {
                if (string.IsNullOrWhiteSpace(lang)) return "es";
                var parts = lang.Split('-', StringSplitOptions.RemoveEmptyEntries);
                return parts[0].ToLowerInvariant();
            }
        }
    }