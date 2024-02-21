using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaPDF.Services;

namespace PruebaPDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService _pdfService;

        public PdfController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePdf([FromBody] string html)
        {
            var pdfBytes = await _pdfService.GeneratePdfFromHtmlAsync(html);
            return File(pdfBytes, "application/pdf", "output.pdf");
        }
    }
}
