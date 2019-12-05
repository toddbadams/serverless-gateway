using System.Threading.Tasks;

namespace Gateway.Application.Configuration
{
    public interface IConfigurationProvider
    {
        string Get(string key);
    }
}