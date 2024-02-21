namespace PruebaPDF.Services
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdfFromHtmlAsync(string html);
    }
}
