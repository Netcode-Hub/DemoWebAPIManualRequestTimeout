# Mastering Web APIs: Step-by-Step Guide to Manually Implementing Request Timeouts for Better Performance
![image](https://github.com/Netcode-Hub/DemoWebAPIManualRequestTimeout/assets/110794348/ddaf788d-03d2-45e7-b9c0-1a303034a23c)

# Introduction:
"Welcome back to my channel, tech enthusiasts! In today's video, we're diving deep into the world of Web APIs. Have you ever wondered how to handle scenarios where a request takes too long to process? Well, today we're going to explore a crucial aspect of API development: manually introducing a Request Timeout. By the end of this tutorial, you'll have a solid understanding of how to manage long-running requests and ensure your APIs remain responsive and reliable. So, let's get started!"

# Add Request Timeout Middleware
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

# Create Service to mimick external service call with delay
    namespace DemoWebAPIManualRequestTimeout.Services
    {
        public interface IService
        {
            Task<string> GetString(CancellationToken token);
        }
        public class Service : IService
        {
            public async Task<string> GetString(CancellationToken token)
            {
                await Task.Delay(5000, token);
                return "Data";
            }
        }
    }

  # Register the service
    builder.Services.AddScoped<IService, Service>();

  # Inject and use in Controller while passing HttpContext.RequestAbborted as CancelationToken parameter.
         [ApiController]
     [Route("[controller]")]
     public class WeatherForecastController(IService service) : ControllerBase
     {
         [HttpGet]
         public async Task<IActionResult> Get()
         {
             var response = await service.GetString(HttpContext.RequestAborted);
             return Ok(response);
         }
     }
  
# Conclusion:
"That wraps up our guide on manually implementing Request Timeout in your Web APIs. We hope this tutorial has been helpful and that you now feel more confident in managing request durations in your projects. Remember, handling timeouts efficiently can greatly improve the performance and user experience of your applications. If you enjoyed this video, please give it a thumbs up and don't forget to subscribe for more tech tutorials and tips. Leave a comment below if you have any questions or topics you'd like us to cover in future videos. Thanks for watching, and we'll see you in the next one!"
