namespace Translator.Core.Abstractions.Dto;

public class TranslationDto
{
    public required string From { get; init; }
    
    public required string To { get; init; }
    
    public required string Text { get; init; }
}