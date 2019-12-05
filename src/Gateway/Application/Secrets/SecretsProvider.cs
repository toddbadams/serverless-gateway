using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace Gateway.Application.Secrets
{
    public class SecretsProvider : ISecretsProvider
    {
                
        private static readonly Dictionary<string, string> SecretsCache = new Dictionary<string, string>();

        // using a simple caching pattern for the key vault and secrets which is described here:
        // https://mcguirev10.com/2017/12/23/easy-configuration-sharing-with-azure-key-vault.html

        private static KeyVaultClient _keyVaultClient;
        public static KeyVaultClient CTradeKeyVaultClient
        {
            get
            {
                if (!(_keyVaultClient is null)) return _keyVaultClient;

                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                return  new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            }
        }
        public async Task<string> Get(string key)
        {
            if (SecretsCache.ContainsKey(key)) return SecretsCache.ContainsKey(key) ? SecretsCache[key] : string.Empty;

            var secretBundle = await CTradeKeyVaultClient.GetSecretAsync("endpoint", key).ConfigureAwait(false);
            if (!SecretsCache.ContainsKey(key)) SecretsCache.Add(key, secretBundle.Value);
            return SecretsCache.ContainsKey(key) ? SecretsCache[key] : string.Empty;
        }
    }
}