﻿namespace PMS.Domain.Interfaces;

public interface IEmailService
{
    Task SendHtmlEmailAsync(string toEmail, string subject, string htmlBodyTemplate, Dictionary<string, string> replacements);
}