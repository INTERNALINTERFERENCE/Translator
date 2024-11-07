using Translator.Core.Abstractions.Commands;
using Translator.Core.Abstractions.Dto;

namespace Translator.Core;

public interface ITranslationService
{
    InformationDto GetInformation();
    Task<TranslateResponse> TranslateAsync(TranslateArguments arguments);
}