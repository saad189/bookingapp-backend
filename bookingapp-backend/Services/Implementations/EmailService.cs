using bookingapp_backend.Models;
using bookingapp_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using System.IO;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using bookingapp_backend.Repository.Interfaces;
using bookingapp_backend.Configurations;

namespace bookingapp_backend.Services.Implementations
{
    public class EmailService : IEmailService
    {

        private readonly MailSetting _emailSettings;
        private readonly IEmailRepository _emailRepository;

        public EmailService(IOptions<MailSetting> emailSettings, IEmailRepository emailRepository)
        {
            _emailSettings = emailSettings.Value;
            _emailRepository = emailRepository;
        }

        public Email CreateEmail(string receiverEmail,string receiverName, string subject, Booking booking)
        {
            return new Email
            {
                ReceiverEmail = receiverEmail,
                Body = $"Hello {receiverName},<br> Your booking with the title: <strong>'{booking.Title}'</strong>" +
                $" has been <strong>{subject.Substring(subject.IndexOf(' '))}</strong>.<br><br> Regards, <br> CurtinLab Staff",
                Subject = $"{subject} - {booking.StartTime.ToLocalTime()} :: {booking.EndTime.ToLocalTime()}",
                SentTime = DateTime.Now,
            };
        }

        public async Task SendEmailAsync(Email emailRequest)
        {
            _emailRepository.Create(emailRequest);

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.DisplayName,_emailSettings.Mail));
            email.Sender = MailboxAddress.Parse(_emailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(emailRequest.ReceiverEmail));
            email.Subject = emailRequest.Subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = emailRequest.Body;
            email.Body = builder.ToMessageBody();

            var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.Mail, _emailSettings.Password);

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
