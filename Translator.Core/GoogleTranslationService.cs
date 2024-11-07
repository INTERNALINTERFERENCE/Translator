using System.Text.Json;
using System.Text.RegularExpressions;
using Translator.Core.Abstractions;
using Translator.Core.Abstractions.Commands;
using Translator.Core.Abstractions.Dto;

namespace Translator.Core;

public partial class GoogleTranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;

    public GoogleTranslationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public InformationDto GetInformation()
    {
        return new InformationDto
        {
            ServiceName = ServiceName.Google,
            CacheType = CacheType.Database
        };
    }

    public async Task<TranslateResponse> TranslateAsync(TranslateArguments arguments)
    {
        var requests = arguments.Items
            .Select(argument => GetValidGoogleTranslateApiUrl(argument.From, argument.To, argument.Text))
            .Select(requestUrl => _httpClient.GetAsync(requestUrl));
        
        var responseMessages = await Task.WhenAll(requests);
        var translateResponses = responseMessages.Select(ParseGoogleTranslateApiResponse);
        
        return new(translateResponses);
    }
    
    private TranslationDto ParseGoogleTranslateApiResponse(HttpResponseMessage response)
    {
        // example response: [[["Hola","hello",null,null,10]],null,"en",null,null,null,null,[]], so..
        
        var content = response.Content.ReadAsStringAsync().Result;
        var result = JsonSerializer.Deserialize<dynamic>(content);
        
        return new TranslationDto
        {
            From = GetFromRegex().Match(response.RequestMessage?.RequestUri?.ToString() ?? "").Value,
            To = GetToRegex().Match(response.RequestMessage?.RequestUri?.ToString() ?? "").Value,
            Text = result?[0]?[0]?[0].ToString() ?? "Sorry. Unparsable...",
        };
    }

    /// I hope it would work in future...
    private string GetValidGoogleTranslateApiUrl(string from, string to, string text) =>
        $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={from}&tl={to}&dt=t&q={text}";
    
    [GeneratedRegex("(?<=tl=)[^&]+")]
    private static partial Regex GetToRegex();
    
    [GeneratedRegex("(?<=sl=)[^&]+")]
    private static partial Regex GetFromRegex();
}