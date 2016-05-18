using Evaluate.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Evaluate.ViewModels
{
    public class Email
    {
        private static string From(Common.EmailType type)
        {
            string from = string.Empty;
            from = "hola@proaviationtest.com";
            //switch (type)
            //{
            //    case Common.EmailType.Signup:
            //        from = "jesteban.ar@gmail.com";
            //        break;
            //    default:
            //        from = "cristh1@mail.com";
            //        break;
            //}
            return from;
        }

        private static string Password(Common.EmailType type)
        {
            string password = string.Empty;
            //password = "Pr@123Test";
            password = "aviation1234";
            //switch (type)
            //{
            //    case Common.EmailType.Signup:
            //        password = "H1mselfjalisc@gm";
            //        break;
            //    default:
            //        password = "123456";
            //        break;
            //}
            return password;
        }

        private static string Subject(Common.EmailType type)
        {
            string subject = string.Empty;
            switch (type)
            {
                case Common.EmailType.Signup:
                    subject = "Bienvenido a Pro Aviation Test";
                    break;
                case Common.EmailType.ForgotPassword:
                    subject = "Recuperar Contraseña de Pro Aviation Test";
                    break;
                default:
                    subject = "Pro Aviation Test";
                    break;
            }
            return subject;
        }

        private static string Body(Common.EmailType type, string add, string person)
        {
            string body = string.Empty;
            var path = string.Empty;
            switch (type)
            {
                case Common.EmailType.Signup:
                    path = System.Web.HttpContext.Current.Server.MapPath("~/files/templates/Signup.html");
                    using (StreamReader reader = new StreamReader(path))
                    {
                        body = reader.ReadToEnd();
                    }
                    //body = body.Replace("{Title}", title);
                    body = body.Replace("{url}", add);
                    break;
                case Common.EmailType.ForgotPassword:
                    path = System.Web.HttpContext.Current.Server.MapPath("~/files/templates/ForgotPassword.html");
                    using (StreamReader reader = new StreamReader(path))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{PersonName}", person);
                    body = body.Replace("{url}", add);
                    break;
                default:
                    body = "Pro Aviation Test";
                    break;
            }
            return body;
        }

        public static void Signup(Common.EmailType type, string to, string code, string person)
        {
            Send(type, to, Body(type, code, person));
        }

        public static void ForgotPassword(Common.EmailType type, string to, string code, string person)
        {
            Send(type, to, Body(type, code, person));
        }

        private static void Send(Common.EmailType type, string to, string body)
        {
            /*-------------------------MENSAJE DE CORREO----------------------*/

            //Creamos un nuevo Objeto de mensaje
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();

            //Direccion de correo electronico a la que queremos enviar el mensaje
            mmsg.To.Add(to);

            //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario

            //Asunto
            mmsg.Subject = Subject(type);
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

            //Direccion de correo electronico que queremos que reciba una copia del mensaje
            //mmsg.Bcc.Add("destinatariocopia@servidordominio.com"); //Opcional

            //Cuerpo del Mensaje
            mmsg.Body = body;
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML

            //Correo electronico desde la que enviamos el mensaje
            mmsg.From = new System.Net.Mail.MailAddress(From(type), "Pro Aviation Test");

            /*-------------------------CLIENTE DE CORREO----------------------*/

            //Creamos un objeto de cliente de correo
            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();

            //Hay que crear las credenciales del correo emisor
            cliente.Credentials =
                new System.Net.NetworkCredential(From(type), Password(type));

            /*hotmail*/
            cliente.Port = 8889;// 587;
            //
            cliente.Host = "mail.proaviationtest.com";// "smtp.live.com";
            //cliente.EnableSsl = true;

            /*gmail*/
            //cliente.Port = 587;
            //cliente.EnableSsl = true;
            //cliente.Host = "smtp.gmail.com"; 

            /*-------------------------ENVIO DE CORREO----------------------*/

            try
            {
                //Enviamos el mensaje      
                cliente.Send(mmsg);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //Aquí gestionamos los errores al intentar enviar el correo
            }
        }

        //private string Url(Common.EmailType type)
        //{
        //    string url = string.Empty;
        //    switch (type)
        //    {
        //        case Common.EMailType.Signup:
        //            url = "http://proaviationtest.com/Account/ConfirmEmail";
        //            break;
        //        default:
        //            url = "http://residenciasmedicasrd.com/Account/Recover.aspx";
        //            break;
        //    }
        //    return url;
        //}
    }
}