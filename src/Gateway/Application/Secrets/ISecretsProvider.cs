using System.Threading.Tasks;

namespace Gateway.Application.Secrets
{
    public interface ISecretsProvider
    {
        Task<string> Get(string key);
    }
}