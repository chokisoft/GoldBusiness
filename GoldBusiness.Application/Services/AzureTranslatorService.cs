using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GoldBusiness.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace GoldBusiness.Application.Services
{
    public class AzureTranslatorService : ITranslatorService
    {
        private readonly HttpClient _http;
        private readonly string _endpoint;
        private readonly string _key;
        private readonly string? _region;
        private readonly ILogger<AzureTranslatorService> _logger;

        public AzureTranslatorService(HttpClient http, IConfiguration config, ILogger<AzureTranslatorService> logger)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _endpoint = config["Translator:Endpoint"]?.TrimEnd('/') ?? "https://api.cognitive.microsofttranslator.com";
            _key = config["Translator:Key"] ?? throw new InvalidOperationException("Translator:Key not configured");
            _region = config["Translator:Region"];
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // LOG SEGURO: muestra endpoint, región y últimos 4 chars de la key para verificar configuración
            var keyLast4 = string.IsNullOrEmpty(_key) ? "(none)" : (_key.Length > 4 ? _key[^4..] : _key);
            _logger.LogInformation("AzureTranslator configured -> endpoint={Endpoint}, region={Region}, keyLast4=****{KeyLast4}", _endpoint, _region ?? "(none)", keyLast4);
        }

        public async Task<string> TranslateAsync(string text, string from, string to)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            if (string.IsNullOrWhiteSpace(to)) return text;

            var route = $"/translate?api-version=3.0&to={to}";
            if (!string.IsNullOrWhiteSpace(from))
            {
                route += $"&from={from}";
            }

            var requestUri = _endpoint + route;
            var body = new object[] { new { Text = text } };
            var jsonBody = JsonSerializer.Serialize(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = content };
                request.Headers.Add("Ocp-Apim-Subscription-Key", _key);

                // Solo agregar region si está definida y NO es "global"
                var regionValue = _region?.Trim();
                if (!string.IsNullOrWhiteSpace(regionValue) && !string.Equals(regionValue, "global", StringComparison.OrdinalIgnoreCase))
                {
                    request.Headers.Add("Ocp-Apim-Subscription-Region", regionValue);
                }

                _logger.LogDebug("AzureTranslator: Request -> Uri: {Uri}, From: {From}, To: {To}, Body: {BodyPreview}",
                    requestUri, from, to, text.Length > 200 ? text[..200] + "..." : text);

                var resp = await _http.SendAsync(request);

                var raw = await resp.Content.ReadAsStringAsync();
                _logger.LogDebug("AzureTranslator: Raw response: {RawResponse}", raw);

                resp.EnsureSuccessStatusCode();

                using var doc = JsonDocument.Parse(raw);

                if (doc.RootElement.GetArrayLength() > 0 &&
                    doc.RootElement[0].TryGetProperty("translations", out var translations) &&
                    translations.GetArrayLength() > 0)
                {
                    var textTranslated = translations[0].GetProperty("text").GetString();
                    _logger.LogDebug("AzureTranslator: Translated text (first): {Translated}", textTranslated);
                    return textTranslated ?? string.Empty;
                }

                _logger.LogWarning("AzureTranslator: No translations found in response.");
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AzureTranslator: Error translating text. From={From} To={To}", from, to);
                return string.Empty;
            }
        }
    }
}