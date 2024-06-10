using DinkToPdf;
using DinkToPdf.Contracts;
using System.Drawing.Printing;

namespace ThongFastFood_Client.Services
{
    public class PDFService : IPDFService
    {
        private readonly IConverter _converter;
        public PDFService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdf(string contentHTML, 
                                Orientation orientation = Orientation.Portrait, // bố cục dọc
                                DinkToPdf.PaperKind paperKind = DinkToPdf.PaperKind.A4) // loại trang giấy A4
        {
            var globalSetting = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new MarginSettings() { Top = 10, Bottom = 10 }
            };

            var objectSetting = new ObjectSettings()
            {
                PagesCount = true,
                HtmlContent = contentHTML,
                WebSettings = { DefaultEncoding = "utf-8" },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSetting,
                Objects = { objectSetting }
            };

            return _converter.Convert(pdf);
        }
    }
}
