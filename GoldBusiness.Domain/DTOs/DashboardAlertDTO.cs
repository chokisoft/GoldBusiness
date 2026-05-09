namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para alertas y notificaciones del dashboard
    /// </summary>
    public class DashboardAlertDTO
    {
        public string Type { get; set; } = string.Empty; // "warning", "error", "info", "success"
        public string Icon { get; set; } = string.Empty;
        public string MessageKey { get; set; } = string.Empty; // Clave de traducción
        public string Message { get; set; } = string.Empty; // Mensaje formateado
        public int Count { get; set; } // Cantidad (ej: 5 productos)
        public string ActionRoute { get; set; } = string.Empty; // Ruta para ver más
    }

    /// <summary>
    /// Resumen de todas las alertas del dashboard
    /// </summary>
    public class DashboardAlertsDTO
    {
        public List<DashboardAlertDTO> Alerts { get; set; } = new();
        public int TotalAlerts { get; set; }
        public int CriticalAlerts { get; set; }
    }
}
