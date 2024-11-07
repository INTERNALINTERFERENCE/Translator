using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Translator.Core.Abstractions;
using Translator.Core.Abstractions.Commands;
using Translator.Core.Abstractions.Dto;

namespace Translator.Core;

public partial class GoogleTranslationService : ITranslationService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    
    public GoogleTranslationService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }
    
    public InformationDto GetInformation()
    {
        return new InformationDto
        {
            ServiceName = ServiceName.Google,
            CacheType = CacheType.InMemory
        };
    }

    public async Task<TranslateResponse> TranslateAsync(TranslateArguments arguments)
    {
        var requests = arguments.Items
            .Select(argument => GetTranslation(argument.From, argument.To, argument.Text));
        
        var translateResponses = await Task.WhenAll(requests);
        return new(translateResponses);
    }
    
    private async Task<TranslationDto> GetTranslation(string from, string to, string text)
    {
        var cacheKey = $"{from}_{to}_{text}";

        if (_cache.TryGetValue(cacheKey, out TranslationDto? cachedTranslation))
            return cachedTranslation!;

        var responseMessage = await _httpClient.GetAsync(GetValidGoogleTranslateApiUrl(from, to, text));
        var translation = ParseGoogleTranslateApiResponse(responseMessage, from, to);

        _cache.Set(cacheKey, translation, TimeSpan.FromMinutes(10));

        return translation;
    }

    
    private TranslationDto ParseGoogleTranslateApiResponse(HttpResponseMessage response, string from, string to)
    {
        // example response: [[["Hola","hello",null,null,10]],null,"en",null,null,null,null,[]], so..
        
        var content = response.Content.ReadAsStringAsync().Result;
        var result = JsonSerializer.Deserialize<dynamic>(content);
        
        return new TranslationDto
        {
            From = from,
            To = to,
            Text = result?[0]?[0]?[0].ToString() ?? "Sorry. Unparsable...",
        };
    }

    /// I hope it would work in future...
    private string GetValidGoogleTranslateApiUrl(string from, string to, string text) =>
        $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={from}&tl={to}&dt=t&q={text}";
}