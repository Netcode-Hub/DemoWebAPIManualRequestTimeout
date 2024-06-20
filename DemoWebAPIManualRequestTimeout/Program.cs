using DemoWebAPIManualRequestTimeout.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IService, Service>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Use(async (context, next) =>
{
    var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500)); // Set your default timeout here
    context.RequestAborted = cts.Token;

    try
    {
        await next();
    }
    catch (OperationCanceledException) when (cts.IsCancellationRequested)
    {
        context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
        await context.Response.WriteAsync("Request timed out.");
    }
});
app.Run();
