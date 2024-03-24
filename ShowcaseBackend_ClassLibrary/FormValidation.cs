using Ganss.Xss;
using HtmlAgilityPack;
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
            string emailRegexPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            Regex re = new Regex(emailRegexPattern);

            return re.IsMatch(email);
        }

        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            var phoneRegex = @"^(\+\d{1,2}\s?)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
            Regex re = new Regex(phoneRegex);

            return re.IsMatch(phoneNumber);
        }

        //For escaping all HTML and special characters
        public static string StripHTML(string str)
        {
            //Load the HTML into a document
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(str);

            //Return the text that is placed in between the HTML tags
            return htmlDoc.DocumentNode.InnerText;
        }

        public static bool CheckStringLength(string str, int length)
        {
            if (str.Length > length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //For sanitizing HTML specifically for blog posts
        public static string SanitizeHtml(string html)
        {
            var sanitizer = new HtmlSanitizer();

            sanitizer.AllowedTags.Add("h3");
            sanitizer.AllowedTags.Add("h4");
            sanitizer.AllowedTags.Add("h5");
            sanitizer.AllowedTags.Add("h6");
            sanitizer.AllowedTags.Add("p");
            sanitizer.AllowedTags.Add("i");
            sanitizer.AllowedTags.Add("blockquote");
            sanitizer.AllowedTags.Add("strong");
            sanitizer.AllowedTags.Add("ol");
            sanitizer.AllowedTags.Add("ul");
            sanitizer.AllowedTags.Add("li");

            return sanitizer.Sanitize(html);
        }

    }
}
