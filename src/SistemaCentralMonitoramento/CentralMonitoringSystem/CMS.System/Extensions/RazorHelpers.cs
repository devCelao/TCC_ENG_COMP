using Microsoft.AspNetCore.Mvc.Razor;
using System.Text;

namespace CMS.System.Extensions
{
    public static class RazorHelpers
    {
        public static string FormataSituacao(this RazorPage page, int valor)
        {
            return valor == 1 ? "Ligado" : "Desligado";
        }
    }
}
