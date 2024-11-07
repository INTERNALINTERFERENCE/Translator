using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Translator.Core;
using Translator.Core.Abstractions.Commands;
using Translator.Core.Abstractions.Dto;

var serviceProvider = new ServiceCollection()
    .AddScoped<HttpClient>()
    .AddScoped<ITranslationService, GoogleTranslationService>()
    .BuildServiceProvider();

var translationService = serviceProvider.GetRequiredService<ITranslationService>();

Console.WriteLine("Write text to translate:");
var text = Console.ReadLine();

Console.WriteLine("Write source language:");
var fromLanguage = Console.ReadLine();

Console.WriteLine("Write target language:");
var toLanguage = Console.ReadLine();

var result = await translationService.TranslateAsync(new TranslateArguments
{
    Items = [
        new TranslationDto
        {
            From = fromLanguage,
            To = toLanguage,
            Text = text
        }
    ]
});

var jsonSerializerOptions = new JsonSerializerOptions
{
    Converters = { new JsonStringEnumConverter() }
};

Console.WriteLine($"Translation: {JsonSerializer.Serialize(result, jsonSerializerOptions)}");

var serializedInformation = JsonSerializer.Serialize(
    translationService.GetInformation(), 
    jsonSerializerOptions);

Console.WriteLine($"Information about service: {serializedInformation}");