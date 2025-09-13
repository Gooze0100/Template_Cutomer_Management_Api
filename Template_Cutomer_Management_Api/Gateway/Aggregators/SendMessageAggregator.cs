using System.Net;
using System.Text;
using System.Text.Json;
using Gateway.Helpers;
using Gateway.Models;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

namespace Gateway.Aggregators;

public class SendMessageAggregator : IDefinedAggregator
{
    public async Task<DownstreamResponse> Aggregate(List<HttpContext> responseHttpContexts)
    {
        var responses = responseHttpContexts.Select(x => x.Items.DownstreamResponse()).ToArray();
        
        string customerJson = await responses[0].Content.ReadAsStringAsync();
        string templateJson = await responses[1].Content.ReadAsStringAsync();
        
        JsonSerializerOptions options = new ()
        {
            PropertyNameCaseInsensitive = true
        };

        Customer customer = JsonSerializer.Deserialize<Customer>(customerJson, options);
        Template template = JsonSerializer.Deserialize<Template>(templateJson, options);

        string body = Message.TransformTemplateBody(customer, template.Body);
        
        var combined = new
        {
            Customer = customer,
            Template = template,
            Body = body
        };
        
        // Sending message, it should send to another API to send message with this information, to send to client.
        Console.WriteLine(body);

        var combinedContent = JsonSerializer.Serialize(combined);

        return new DownstreamResponse(
            new StringContent(combinedContent, Encoding.UTF8, "application/json"),
            HttpStatusCode.OK,
            responses.SelectMany(x => x.Headers).ToList(),
            "OK"
        );
    }
}