using MailKit.Net.Smtp;
using MimeKit;
using PizzaStar.Models;

namespace PizzaStar.Data.Helpers
{
    public class EmailSender
    {
        //Зависимость которую зарегистрировали как Singleton объект
        //в который получили настройки почты из файла appsettings.json
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public bool SendResetPassword(string email, string link, string name)
        {
            var message = new MimeMessage();
            //от кого отправляем и заголовок
            message.From.Add(new MailboxAddress("Pizza Star", "pizzastarclient@gmail.com"));
            //кому отправляем
            message.To.Add(new MailboxAddress(name, email));
            //тема письма
            message.Subject = "Ваша ссылка для сброса пароля на Pizza Star";
            //тело письма
            message.Body = new TextPart("html")
            {
                Text = $@"<html lang=""ru"">
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta name=""viewport"" content=""width=device-width""/>
	    <style type=""text/css"">
    * {{   margin: 0;   padding: 0;   font-size: 100%;   font-family: 'Avenir Next', ""Helvetica Neue"", ""Helvetica"", Helvetica, Arial, sans-serif;   line-height: 1.65; }}  img {{   max-width: 100%;   margin: 0 auto;   display: block; }}  body, .body-wrap {{   width: 100% !important;   height: 100%;   background: #efefef;   -webkit-font-smoothing: antialiased;   -webkit-text-size-adjust: none; }}  a {{   color: #71bc37;   text-decoration: none; }}  .text-center {{   text-align: center; }}  .text-right {{   text-align: right; }}  .text-left {{   text-align: left; }}  .button {{   display: inline-block;   color: white !important;  background: #780cba;   border: solid #780cba;   border-width: 10px 20px 8px;   font-weight: bold;   border-radius: 4px; }}  h1, h2, h3, h4, h5, h6 {{   margin-bottom: 20px;   line-height: 1.25; }}  h1 {{   font-size: 32px; }}  h2 {{   font-size: 28px; }}  h3 {{   font-size: 24px; }}  h4 {{   font-size: 20px; }}  h5 {{   font-size: 16px; }}  p, ul, ol {{   font-size: 16px;   font-weight: normal;   margin-bottom: 20px; }}  .container {{   display: block !important;   clear: both !important;   margin: 0 auto !important;   max-width: 580px !important; }}   .container table {{     width: 100% !important;     border-collapse: collapse; }}   .container .masthead {{     padding: 80px 0;     background: #780cba;     color: white; }}     .container .masthead h1 {{       margin: 0 auto !important;       max-width: 90%;       text-transform: uppercase; }}   .container .content {{     background: white;     padding: 30px 35px; }}     .container .content.footer {{       background: none; }}       .container .content.footer p {{         margin-bottom: 0;         color: #888;         text-align: center;         font-size: 14px; }}       .container .content.footer a {{         color: #888;         text-decoration: none;         font-weight: bold; }} 
    </style>
</head>
<body>
<table class=""body-wrap"">
    <tr>
        <td class=""container"">

            <!-- Message start -->
            <table>
                <tr>
                    <td align=""center"" class=""masthead"">

                        <h1>Сброс пароля на Pizza Star</h1>

                    </td>
                </tr>
                <tr>
                    <td class=""content"">

                        <h2>Приветствую вас, {name}!</h2>

                        <p>Как ваши дела сегодня?<br/>Недавно вы запросили сброс пароля для вашего аккаунта. Нажмите на кнопку ниже, чтобы продолжить.</p>

                        <table>
                            <tr>
                                <td align=""center"">
                                    <p>
                                        <a href=""{link}"" class=""button"">Сбросить пароль</a>
                                    </p>
                                </td>
                            </tr>
                        </table>
						<p>Если вы не запрашивали сброс пароля, пожалуйста, проигнорируйте это письмо или ответьте на него, чтобы сообщить нам об этом. 
						Эта ссылка на сброс пароля действительна только в течение <b>следующего часа.</b></p>
                        
                        <p><em>– Pizza Star</em></p>

                    </td>
                </tr>
            </table>

        </td>
    </tr>
    <tr>
        <td class=""container"">

            <!-- Message start -->
            <table>
                <tr>
                    <td class=""content footer"" align=""center"">
                        <p>Сообщение от копании <a href=""#"">Pizza Star</a>.</p>
                        <p><a href=""mailto:pizzastar1@gmail.com"">pizzastar@gmail.com</a> | <a href=""#"">Перейти на сайт</a></p>
                    </td>
                </tr>
            </table>

        </td>
    </tr>
</table>
</body>
</html>"

            };
            return Send(message);
        }

        private bool Send(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                //Указываем smtp сервер почты и порт
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);
                //Указываем свой Email адрес и пароль приложения
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception)
                {
                    //Logging information
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
            return false;
        }
    }
}
