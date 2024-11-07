using Grpc.Core;
using Translator.Client.Grpc.Extensions;
using Translator.Core;
using Translator.Grpc;

namespace Translator.Client.Grpc.Services;

public class TranslationService : Translator.Grpc.TranslationService.TranslationServiceBase
{
    private readonly ITranslationService _translationService;

    public TranslationService(ITranslationService translationService)
    {
        _translationService = translationService;
    }
    
    public override Task<InformationDto> GetInformation(
        Empty request, 
        ServerCallContext context)
    {
        var information = _translationService.GetInformation();
        return Task.FromResult(new InformationDto
        {
            ServiceName = (ServiceName)information.ServiceName, 
            CacheType = (CacheType)information.CacheType
        });
    }
    
    public override async Task<GrpcTranslateResponse> Translate(
        GrpcTranslateArguments arguments,
        ServerCallContext context)
    {
        var translateArguments = arguments.Items
            .Select(translationDto => translationDto.ToDto());
        
        var result = await _translationService.TranslateAsync(new(translateArguments));

        return new GrpcTranslateResponse
        {
            Items = {result.Items.Select(translationDto => translationDto.ToGrpcDto())}
        };
    }
}