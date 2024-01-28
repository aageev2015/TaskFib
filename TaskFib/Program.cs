using TaskFib.WebApi.Utilities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.TaskFibConfigureSettings(builder.Configuration);
builder.Services.TaskFibAddIterationsWorkload();
builder.Services.TaskFibAddFibonacciServices();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();
