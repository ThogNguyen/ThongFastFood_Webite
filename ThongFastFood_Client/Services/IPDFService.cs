using DinkToPdf;

namespace ThongFastFood_Client.Services
{
    public interface IPDFService
    {
        public byte[] GeneratePdf(string contentHTML,
                                Orientation orientation = Orientation.Portrait,
                                PaperKind paperKind = PaperKind.A4);
    }
}
