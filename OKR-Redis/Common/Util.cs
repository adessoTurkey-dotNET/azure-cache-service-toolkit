using System;

namespace OKR_Redis.Common
{
    public static class Util
    {
        public static string GetConfig(string parameterName, string defaultValue)
        {
            return Environment.GetEnvironmentVariable(parameterName, EnvironmentVariableTarget.Process) ?? defaultValue;
        }
    }
}
