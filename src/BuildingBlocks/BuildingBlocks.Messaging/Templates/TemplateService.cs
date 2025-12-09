using BuildingBlocks.Messaging.Email;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Messaging.Templates;

public class TemplateService : ITemplateService
{
    private readonly EmailOptions _options;
    private readonly string _templateBasePath;

    public TemplateService(IOptions<EmailOptions> options)
    {
        _options = options.Value;

        
        _templateBasePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            _options.TemplateFolder
        );
    }

    public async Task<string> RenderTemplateAsync(
        string templateName,
        Dictionary<string, object> data)
    {
        
        var templatePath = Path.Combine(_templateBasePath, $"{templateName}.html");

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template not found: {templateName}.html");
        }

        var template = await File.ReadAllTextAsync(templatePath);

        
        foreach (var kvp in data)
        {
            var placeholder = $"{{{{{kvp.Key}}}}}";
            template = template.Replace(placeholder, kvp.Value?.ToString() ?? string.Empty);
        }

        return template;
    }
}
