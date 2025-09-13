using Gateway.Models;
using Template = Scriban.Template;

namespace Gateway.Helpers;

public static class Message
{
    public static string TransformTemplateBody(Customer customer, string body)
    {
        if (customer == null || string.IsNullOrWhiteSpace(body))
        {
            return string.Empty;
        }
        
        var template = Template.Parse(body);
        var result = template.Render(new
        {
           customer.Name,
           customer.Email
        });
        
        return result;
    }
}