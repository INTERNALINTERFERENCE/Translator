using Translator.Common.Abstractions;
using Translator.Core.Abstractions.Dto;

namespace Translator.Core.Abstractions.Commands;

public class TranslateArguments : EntityItems<TranslationDto>
{
    public TranslateArguments()
    {
    }

    public TranslateArguments(TranslationDto translationDto) 
        : base(translationDto)
    {
    }

    public TranslateArguments(IEnumerable<TranslationDto> translationDtos)
        : base(translationDtos)
    {
    }
}