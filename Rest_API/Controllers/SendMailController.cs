using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Rest_API.Models;
using Rest_API_ClassLibrary;
using System.Diagnostics;

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

            if(!FormValidation.ValidateEmail(mail.email)) {
                return BadRequest(new { message = "Vul een geldig email adres in!" });
            }

            if(!FormValidation.ValidatePhoneNumber(mail.tel)) {
                return BadRequest(new { message = $"Telefoonnummer mag alleen nummers bevatten!" });
            }

            //Build up the email and send, returns a 200 OK
            client.Send(
                FormValidation.StripHTML(mail.email), 
                "stefan.maring@windesheim.nl", 
                FormValidation.StripHTML(mail.subject), 
                $"{"Voornaam: " + FormValidation.StripHTML(mail.surname) + " " + "Achternaam: " + FormValidation.StripHTML(mail.lastname) + " " + "Telefoonnummer: " + FormValidation.StripHTML(mail.tel) + "\n" + "Bericht: " + "\n" + FormValidation.StripHTML(mail.message)}"
            );
            return Ok(new { message = "Email succesvol verzonden!" });
        }
    }
}
