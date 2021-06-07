using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", (Func<string>)(() => "Hello World!"));

await app.RunAsync();





















/* ×î¼ò»¯´úÂë */
//var app2 = WebApplication.Create(args);
//app2.MapGet("/", (Func<string>)(() => "Hello World!"));
//app2.Run();