using System;
using Microsoft.Extensions.Configuration;

namespace mvcdemo.Extensions
{
    public static class EnvironmentVariableExtension
    {
        public static string GetConnectionStringFromEnvironment(this IConfiguration configuration, string name = null)
        {
            if (string.IsNullOrEmpty(name))
                return Environment.GetEnvironmentVariable("ConnectionMvcDemo");
            return Environment.GetEnvironmentVariable(name);
        }
    }
}