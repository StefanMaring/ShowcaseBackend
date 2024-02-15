using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Rest_API.Models;

namespace Rest_API.Controllers
{

    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailController : ControllerBase {

        [HttpPost(Name = "sendMail")]
        public async Task<IActionResult> Post([FromBody] Mail mail) {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525) {
                Credentials = new NetworkCredential("a8f55ea1a691f4", "a8eaa8f4d6f2bf"),
                EnableSsl = true
            };

            client.Send(
                mail.email, 
                "stefan.maring@windesheim.nl", 
                mail.subject, 
                $"{"Voornaam: " + mail.surname + " " + "Achternaam: " + mail.lastname + " " + "Telefoonnummer: " + mail.tel + "\n" + "Bericht: " + "\n" + mail.message}"
            );
            return Ok("Mail send");
        }
    }
}
