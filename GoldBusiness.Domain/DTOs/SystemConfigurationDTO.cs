using GoldBusiness.Domain.Enums;
using System;

namespace GoldBusiness.Domain.DTOs
{
    public class SystemConfigurationDTO
    {
        public int Id { get; set; }
        public string CodigoSistema { get; set; } = string.Empty;
        public string Licencia { get; set; } = string.Empty;
        public string NombreNegocio { get; set; } = string.Empty;
        public string? PersonaContacto { get; set; }
        public int? FormaJuridicaId { get; set; }
        public string? Direccion { get; set; }
        
        public int PaisId { get; set; }
        public int ProvinciaId { get; set; }
        public int MunicipioId { get; set; }
        public int CodigoPostalId { get; set; }
        
        public string? Municipio { get; set; }
        public string? Provincia { get; set; }
        public string? CodPostal { get; set; }
        public string? Imagen { get; set; }
        public string? Web { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        
        public int? CuentaPagarId { get; set; }
        public int? CuentaCobrarId { get; set; }
        
        // ═══════════════════════════════════════════════════════════════
        // 🧾 DATOS FISCALES DE LA EMPRESA (NULLABLE para flexibilidad)
        // ═══════════════════════════════════════════════════════════════
        
        public string? IdentificadorFiscal { get; set; }
        public TipoIdentificacionFiscal? TipoIdentificadorFiscal { get; set; }
        public RegimenFiscal? RegimenFiscal { get; set; }
        public decimal? TasaIvaDefecto { get; set; }
        public bool? RegistradaIva { get; set; }
        public bool? IvaInternacional { get; set; }
        
        public DateTime Caducidad { get; set; }
        
        public string? CreadoPor { get; set; }
        public DateTime? FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }
        
        public string? CuentaPagarCodigo { get; set; }
        public string? CuentaPagarDescripcion { get; set; }
        public string? CuentaCobrarCodigo { get; set; }
        public string? CuentaCobrarDescripcion { get; set; }
        
        // Propiedades calculadas (readonly, no se setean desde el form)
        public bool EstaVigente => Caducidad > DateTime.Now;
        public bool EstaVencida => Caducidad <= DateTime.Now;
        public bool ProximoAVencer => Caducidad > DateTime.Now && Caducidad <= DateTime.Now.AddDays(30);
        public int? DiasRestantes => EstaVigente ? (int?)(Caducidad - DateTime.Now).TotalDays : null;
        public string EstadoLicencia => EstaVencida ? "Vencida" : (ProximoAVencer ? "Próximo a vencer" : "Vigente");
        public bool TieneCuentasConfiguradas => CuentaPagarId.HasValue && CuentaCobrarId.HasValue;
        
        public bool Activo { get; set; }
        public bool Cancelado { get; set; }
    }
}