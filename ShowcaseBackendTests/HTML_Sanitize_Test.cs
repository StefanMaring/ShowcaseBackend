using Ganss.Xss;
using Rest_API_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowcaseBackendTests {
    [TestFixture]
    public class HTML_Sanitize_Test {
        [Test]
        public void testHTMLSanitization() {
            string htmlWithScriptTag = "<script>alert('This is a script tag');</script><iframe></iframe>";
            string htmlWithAllowedTags = "<h3>This is a heading</h3><p>This is a paragraph</p>";

            string sanitizedHtmlWithScriptTag = FormValidation.SanitizeHtml(htmlWithScriptTag);
            string sanitizedHtmlWithAllowedTags = FormValidation.SanitizeHtml(htmlWithAllowedTags);

            Assert.IsFalse(sanitizedHtmlWithScriptTag.Contains("<script>"));
            Assert.IsFalse(sanitizedHtmlWithScriptTag.Contains("<iframe>"));

            Assert.IsTrue(sanitizedHtmlWithAllowedTags.Contains("<h3>"));
            Assert.IsTrue(sanitizedHtmlWithAllowedTags.Contains("<p>"));
        }

        [Test]
        public void testHTMLStripSanitization() {
            string htmlString = "<p>Test <strong>test</strong> test <em>test</em> test.</p><script>alert('hallo');</script><p> Hallo Wereld</p>";
            string expectedText = "Test test test test test. Hallo Wereld";

            string strippedText = FormValidation.StripHTML(htmlString);

            Assert.AreEqual(expectedText, strippedText);
        }
    }
}
