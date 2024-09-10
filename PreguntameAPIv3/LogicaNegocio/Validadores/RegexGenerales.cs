using System.Text.RegularExpressions;

namespace PreguntameAPIv3.LogicaNegocio.Validadores
{
    public static partial class RegexGenerales
    {
        [GeneratedRegex(@"['""\\;|<>\-]+")]
        private static partial Regex CaracteresProhibidosRegex();


        [GeneratedRegex(@"[\s]+")]
        private static partial Regex EspaciosBlancoRegex();


        [GeneratedRegex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$")]
        private static partial Regex EmailRegex();


        [GeneratedRegex(@"^[\p{L}]+(\s[\p{L}]+)*$")]
        private static partial Regex AlfabeticosRegex();


        // Metodos de clase
        public static bool ValidarCaracteresProhibidos(string valor)
        {
            return CaracteresProhibidosRegex().IsMatch(valor);
        }

        public static bool ValidarEspaciosBlanco(string valor)
        {
            return EspaciosBlancoRegex().IsMatch(valor);
        }

        public static bool ValidarSoloAlfabeticos(string valor)
        {
            return AlfabeticosRegex().IsMatch(valor);
        }

        public static bool ValidarFormatoEmail(string valor)
        {
            return EmailRegex().IsMatch(valor);
        }
    }
}
