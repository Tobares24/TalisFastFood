namespace Talis_Fast_Food.Utils
{
    public class SweetAlertHelper
    {
        public static string Mensaje(string titulo, string mensaje, SweetAlertMessageType tipoMensaje)
        {
            return "swal({title: '" + titulo + "',text: '" + mensaje + "',type: '" + tipoMensaje + "'});";
        }
    }

    public enum SweetAlertMessageType
    {
        warning, error, success, info
    }
}