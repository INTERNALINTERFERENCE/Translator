using Translator.Core.Abstractions.Dto;
using Translator.Grpc;

namespace Translator.Client.Grpc.Extensions;

public static class Dto
{
    public static TranslationDto ToDto(this GrpcTranslationDto translationDto) =>
        new()
        {
            From = translationDto.From,
            To = translationDto.To,
            Text = translationDto.Text
        };
    
    public static GrpcTranslationDto ToGrpcDto(this TranslationDto translationDto) =>
        new()
        {
            From = translationDto.From,
            To = translationDto.To,
            Text = translationDto.Text
        };
}