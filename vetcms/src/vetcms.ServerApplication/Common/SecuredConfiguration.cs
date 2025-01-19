using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Common
{
    internal class SecuredConfiguration : IConfiguration
    {
        private readonly IConfiguration baseConfig;
        private SecuredConfiguration(IConfiguration _baseConfig)
        {
            baseConfig = _baseConfig;
        }

        public string? this[string key] { get => SecureGet(key); set => baseConfig[key] = value; }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return baseConfig.GetChildren();
        }

        public IChangeToken GetReloadToken()
        {
            return baseConfig.GetReloadToken();
        }

        public IConfigurationSection GetSection(string key)
        {
            var section = baseConfig.GetSection(key);
            section.Value = SecureGet(key);
            return section;
        }

        public static SecuredConfiguration FromPlainConfiguration(IConfiguration baseConfig)
        {
            return new SecuredConfiguration(baseConfig);
        }

        private string? SecureGet(string key)
        {
            var value = baseConfig[key];
            if(value == null)
            {
                throw new Exception($"Configuration key {key} not found");
            }

            if (value is string && value.StartsWith("$_ENV_"))
            {
                string enviromentKey = value.Replace("$_ENV_", "");
                value = Environment.GetEnvironmentVariable(enviromentKey);
                if(value == null)
                {
                    throw new Exception($"Environment variable {enviromentKey} not found");
                }
            }
            return value;
        }
    }
}
