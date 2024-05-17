using System.ComponentModel.DataAnnotations;

namespace Railsware.MailtrapClient.Mail
{
    public class MailValidator
    {
        public static bool TryValidate(MailMessage message)
        {
            var valContext = new ValidationContext(message, null, null);
            var validationsResults = new List<ValidationResult>();

            // validate data annotation attributes compliance
            bool isValidByAttributes = Validator.TryValidateObject(message, valContext, validationsResults, true);

            // check txt and html body values only if attribute validation passed
            // extensive logging and error output is over the bounds of the task
            return isValidByAttributes && (message.Text != null || message.Html != null);
        }
    }
}