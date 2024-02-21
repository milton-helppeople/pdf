using PruebaPDF.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class PdfService : IPdfService
{
    public async Task<byte[]> GeneratePdfFromHtmlAsync(string html)
    {
        // Se crea un nombre de archivo temporal para el PDF y el HTML
        var pdfFilePath = Path.GetTempFileName();
        var htmlFilePath = Path.GetTempFileName();
        var wkhtmltopdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdf", "wkhtmltopdf.exe");

        try
        {
            // Se guarda el HTML en un archivo temporal
            await File.WriteAllTextAsync(htmlFilePath, html);

            // Se configuran los detalles de la ejecución de wkhtmltopdf
            var processInfo = new ProcessStartInfo
            {
                FileName = wkhtmltopdfPath, // Nombre del ejecutable de wkhtmltopdf
                Arguments = $"--load-error-handling ignore {htmlFilePath} {pdfFilePath}", // Argumentos para convertir el HTML a PDF
                RedirectStandardOutput = true, // Se redirige la salida estándar para capturarla
                RedirectStandardError = true, // Se redirige la salida de errores para capturarla
                UseShellExecute = false, // Se especifica que no se va a usar el shell para ejecutar el proceso
                CreateNoWindow = true // Se especifica que no se cree una ventana para el proceso
            };

            // Se crea y se inicia el proceso de conversión de HTML a PDF
            using (var process = new Process())
            {
                process.StartInfo = processInfo; // Se asignan los detalles de configuración al proceso
                process.Start(); // Se inicia el proceso

                await process.WaitForExitAsync(); // Se espera a que el proceso termine de ejecutarse

                // Si el proceso se ejecutó correctamente (código de salida 0)
                if (process.ExitCode == 0)
                {
                    // Se lee el contenido del PDF generado
                    return await File.ReadAllBytesAsync(pdfFilePath);
                }
                else
                {
                    // Si hubo algún error, se lanza una excepción con el mensaje de error
                    throw new Exception($"Error al generar PDF: {process.StandardError.ReadToEnd()}");
                }
            }
        }
        finally
        {
            // Se limpian los archivos temporales
            File.Delete(htmlFilePath);
            File.Delete(pdfFilePath);
        }
    }
}
