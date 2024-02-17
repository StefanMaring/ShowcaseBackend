using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rest_API_ClassLibrary
{
    public class FormValidation
    {
        public static bool ValidateEmail(string email)
        {
            try
            {
                //If the constructor for MailAdress succeeds, the email is valid
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            var phoneRegex = @"^(\+\d{1,2}\s?)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
            Regex re = new Regex(phoneRegex);

            return re.IsMatch(phoneNumber);
        }

        public static string StripHTML(string str)
        {
            //Load the HTML into a document
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(str);

            //Return the text that is placed in between the HTML tags
            return htmlDoc.DocumentNode.InnerText;
        }
    }
}
