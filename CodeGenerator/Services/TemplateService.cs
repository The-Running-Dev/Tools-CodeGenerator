using System;
using System.Reflection;
using System.Text.RegularExpressions;

using RazorEngine;

using Essential.Templating.Razor;
using Essential.Templating.Common.Caching;
using Essential.Templating.Common.Storage;
using Essential.Templating.Razor.Configuration;

using CodeGenerator.Services.Interfaces;

namespace CodeGenerator.Services
{
    public class TemplateService : ITemplateService
    {
        public string Generate<TEntity>(TEntity entity, string templatePath)
        {
            //ITemplateEngine engine = new RazorTemplateEngineBuilder()
            //    .FindTemplatesInDirectory("Models")
            //    .CacheExpiresIn(TimeSpan.FromSeconds(20))
            //    .UseSharedCache()
            //    .Build();

            var embeddedResourceConfig = new RazorTemplateEngineConfiguration
            {
                CodeLanguage = Language.CSharp,
                ResourceProvider = new EmbeddedResourceProvider(Assembly.GetExecutingAssembly()),
                CachePolicy = CachePolicy.Shared
            };

            var engine = new RazorTemplateEngine(embeddedResourceConfig);
            var template = engine.Render(templatePath, entity);

            // Replace HTML line breaks with new lines and trim the white space
            return Regex.Replace(template, @"<br\s?/>", Environment.NewLine).Trim();
        }

        //public MailMessage GetByName<T>(string templateName, T model)
        //{
        //    var embeddedResourceConfig = new RazorTemplateEngineConfiguration
        //    {
        //        CodeLanguage = Language.CSharp,
        //        ResourceProvider = new EmbeddedResourceProvider(Assembly.GetExecutingAssembly(), "Mx.Templates"),
        //        CachePolicy = CachePolicy.Shared
        //    };

        //    //engine = New RazorTemplateEngineBuilder().FindTemplatesInDirectory(String.Format("{0}EmailTemplates")).Build()
        //    var engine = new RazorTemplateEngine(embeddedResourceConfig);

        //    var mailMessage = engine.RenderEmail(templateName, model);

        //    return mailMessage;
        //}
    }
}