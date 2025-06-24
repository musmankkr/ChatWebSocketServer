using ChatWebSocketServer.Configurations;
using Google.Cloud.Dialogflow.V2;
using Microsoft.Extensions.Options;
using System.IO;
using Environment = System.Environment;

namespace ChatWebSocketServer.Services;

public class DialogflowService
{
    private readonly DialogflowSettings _settings;

    public DialogflowService(IOptions<DialogflowSettings> options, IWebHostEnvironment env)
    {
        _settings = options.Value;

        var fullPath = Path.Combine(env.ContentRootPath, _settings.CredentialsPath);
        //Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _settings.CredentialsPath);

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", fullPath);
        var path = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
        Console.WriteLine($"Auth file path: {path}");
        Console.WriteLine($"Exists: {File.Exists(path)}");
    }

    public async Task<string> DetectIntentAsync(string userInput)
    {
        var sessionId = Guid.NewGuid().ToString();

        var builder = new SessionsClientBuilder();
        var client = await builder.BuildAsync();

        var session = SessionName.FromProjectSession(_settings.ProjectId, sessionId);

        var queryInput = new QueryInput
        {
            Text = new TextInput
            {
                Text = userInput,
                LanguageCode = _settings.LanguageCode
            }
        };

        var response = await client.DetectIntentAsync(session, queryInput);
        return response.QueryResult.FulfillmentText;
    }
}
