﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Registration
{
    public class SendRegistrationEmail : IRegisterAsInstancePerLifetime
    {
        private readonly CreateEmailConfirmationLink _createEmailConfirmationLink;

        public SendRegistrationEmail(CreateEmailConfirmationLink createEmailConfirmationLink,
                                     SendEmail sendEmail)
        {
            _createEmailConfirmationLink = createEmailConfirmationLink;
            _sendEmail = sendEmail;
        }

        private readonly SendEmail _sendEmail;

        public void Run(User user)
        {
            var mail = new MailMessage();
            mail.To.Add(user.EmailAddress);
            mail.From = new MailAddress(Settings.EmailDefaultFrom);

            var emailBody = new StringBuilder();
            emailBody.Append("Schön dass Du dabei bist.");
            emailBody.Append("Um Dein Benutzerkonto zu bestätigen,");
            emailBody.Append("folge bitte diesem Link:" + _createEmailConfirmationLink.Run(user));

            mail.Subject = "Willkommen bei True Or False";
            mail.Body = emailBody.ToString();
            
            _sendEmail.Run(mail);
        }
    }
}
