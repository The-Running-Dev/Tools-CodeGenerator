namespace CodeGenerator.Services.Interfaces
{
    public interface ITemplateService
    {
        string Generate<TEntity>(TEntity entity, string templatePath);
    }
}