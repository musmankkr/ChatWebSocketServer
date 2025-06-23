using ChatWebSocketServer.Configurations;
using Google.Cloud.Dialogflow.V2;
using Microsoft.Extensions.Options;
using Environment = System.Environment;

namespace ChatWebSocketServer.Services;

public class DialogflowService
{
    private readonly DialogflowSettings _settings;

    public DialogflowService(IOptions<DialogflowSettings> options)
    {
        _settings = options.Value;

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _settings.CredentialsPath);
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
