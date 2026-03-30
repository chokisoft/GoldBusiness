using System.Threading.Tasks;

namespace GoldBusiness.Application.Interfaces
{
    public interface ITranslatorService
    {
        Task<string> TranslateAsync(string text, string from, string to);
    }
}