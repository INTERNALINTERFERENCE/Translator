
using Translator.Client.Grpc.Services;
using Translator.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<ITranslationService, GoogleTranslationService>();

var app = builder.Build();

app.MapGrpcService<TranslationService>();
app.Run();