using System.Drawing.Imaging;
using System.Drawing;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        [HttpGet]
        public FileContentResult QRCode()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry? hostEntry = Dns.GetHostEntry(hostName);
            string text = string.Empty;
            foreach (IPAddress ip in hostEntry.AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToList())
            {
                text += ip.ToString() + " ";
            }
            if (text != string.Empty)
            {
                text = text.Remove(text.Length - 1, 1);
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCoder.PngByteQRCode qrCode = new QRCoder.PngByteQRCode(qrCodeData);
            return File(qrCode.GetGraphic(20), "image/png");
        }
    }
}
