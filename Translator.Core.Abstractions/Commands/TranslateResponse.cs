using Translator.Common.Abstractions;
using Translator.Core.Abstractions.Dto;

namespace Translator.Core.Abstractions.Commands;

public class TranslateResponse : EntityItems<TranslationDto>
{
    public TranslateResponse()
    {
    }

    public TranslateResponse(TranslationDto translationDto) 
        : base(translationDto)
    {
    }

    public TranslateResponse(IEnumerable<TranslationDto> translationDtos)
        : base(translationDtos)
    {
    }
}