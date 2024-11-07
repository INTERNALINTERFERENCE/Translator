using Microsoft.AspNetCore.Mvc;
using Translator.Core;
using Translator.Core.Abstractions.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ITranslationService, GoogleTranslationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/translate", (
        [FromBody] TranslateArguments arguments,
        [FromServices] ITranslationService translationService) =>
    translationService.TranslateAsync(arguments));

app.Run();
