using Microsoft.Extensions.Options;
using System.Numerics;
using TaskFib.Service;
using TaskFib.Service.Contract;
using TaskFib.WebApi.Utilities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TaskFibSettings>(
    builder.Configuration.GetSection(TaskFibSettings.AppSettingGroupName));

builder.Services.AddSingleton<IIterationsWorkloadAsync>((IServiceProvider sp) =>
{
    var settings = sp.GetRequiredService<IOptions<TaskFibSettings>>();
    return new IterationsWorkloadSleepAsync(settings.Value.SleepWorkloadDelayMS);
});

builder.Services.AddSingleton<ISequenceValueServiceAsync<BigInteger>, FibonacciServiceAsync>();
builder.Services.AddSingleton<ISubsequenceServiceAsync<BigInteger>, SubsequenceServiceAsync>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();
