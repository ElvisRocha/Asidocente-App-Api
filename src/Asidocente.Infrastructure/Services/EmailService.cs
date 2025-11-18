using Asidocente.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asidocente.Infrastructure.Services;

/// <summary>
/// Email service implementation (placeholder for SendGrid)
/// </summary>
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        // TODO: Implement SendGrid integration
        _logger.LogInformation("Sending email to {To} with subject {Subject}", to, subject);
        await Task.CompletedTask;
    }

    public async Task SendTemplateEmailAsync(string to, string templateId, object templateData, CancellationToken cancellationToken = default)
    {
        // TODO: Implement SendGrid template integration
        _logger.LogInformation("Sending template email to {To} with template {TemplateId}", to, templateId);
        await Task.CompletedTask;
    }

    public async Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body, CancellationToken cancellationToken = default)
    {
        // TODO: Implement SendGrid bulk email
        _logger.LogInformation("Sending bulk email to {Count} recipients", recipients.Count());
        await Task.CompletedTask;
    }
}
