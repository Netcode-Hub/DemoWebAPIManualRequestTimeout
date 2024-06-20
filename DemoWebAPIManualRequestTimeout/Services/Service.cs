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
