using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RBAT.Logic;

namespace RBAT.Web.Services {

    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender {

        public EmailSender( IOptions<EmailSettings> emailSettings ) {
            _emailSettings = emailSettings.Value;
        }

        public EmailSettings _emailSettings { get; }

        public async Task Execute( string email, string subject, string message ) {
            try {
                MailMessage mail = new MailMessage {
                    From = new MailAddress( _emailSettings.UsernameEmail, _emailSettings.FromEmail )
                };
                mail.To.Add( new MailAddress( email ) );

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using ( SmtpClient smtp = new SmtpClient( _emailSettings.PrimaryDomain, _emailSettings.PrimaryPort ) ) {
                    smtp.Credentials = new NetworkCredential( _emailSettings.UsernameEmail, _emailSettings.UsernamePassword );
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync( mail );
                }
            } catch ( Exception ex ) {
                //do something here
            }
        }

        public Task SendEmailAsync( string email, string subject, string message ) {
            if ( string.IsNullOrEmpty( email ) ) {
                return Task.CompletedTask;
            }

            Execute( email, subject, message ).Wait();
            return Task.FromResult( 0 );
        }

    }

}
