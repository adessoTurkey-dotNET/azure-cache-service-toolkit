# OKR-Redis

### Azure Cache Service Functions
This repository contains a collection of HTTP triggered Azure Functions for managing the Azure Cache service. The following functions are included:

- FlushCache: Clears all entries from the cache.
- DeleteCache: Deletes a specific entry from the cache.
- AddtoCache: Adds a new entry to the cache.
- GetCache: Retrieves a value from the cache.

### Prerequisites
To use these functions, you will need the following:

- An Azure account
- An Azure Cache service instance
- The Azure Functions extension for Visual Studio

### Getting Started
- Clone this repository to your local machine.
- Open the solution in Visual Studio.
- Replace the placeholder values in the local.settings.json file with your own Azure Cache service connection.
- Build and run the solution to test the functions locally.
- Deploy the functions to Azure using the Azure Functions extension.
- Add your local.settings.json parameters to your Function App Configuration(for best practices reference values from Azure KeyVault.)

### Usage
To use these functions, you can call them via HTTP requests or by triggering them through other Azure services. Refer to the Azure Functions documentation for more information on how to use and trigger Azure Functions.

Example code piece for calling the AddToCache function by using RestSharp:

```csharp
using RestSharp;

string connectionString;
string value;

var client = new RestClient(connectionString);
client.Timeout = -1;
var request = new RestRequest(Method.POST);
request.AddQueryParameter("key", value);
IRestResponse response = client.Execute(request);

console.WriteLine(response.Content);
```

### Using Azure Function app Configurations
Configuration in Azure Functions refers to the process of setting up and managing the various options and settings that control the behavior of your functions. These configurations can include things like connection strings for external resources, runtime settings, and application settings that your functions may need in order to run properly. 
 
You can easily read your values from your Azure Functions app Configurations by using ConfigurationManager.cs.

```csharp
        private ConfigurationManager()
        {
            this.APPINSIGHTS_INSTRUMENTATIONKEY = Util.GetConfig("APPINSIGHTS_INSTRUMENTATIONKEY", "");
            this.CacheConnection = Util.GetConfig("CacheConnection", "");
        }
        
         static RedisCacheService()
        {
            var connection = ConfigurationManager.Instance.CacheConnection;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(connection);
        }
 ```

### Contributions
You are very welcome for contributions to this repository! If you have an idea for a new function or improvement, please open an issue or submit a pull request.

### License
This project is licensed under the MIT License. See LICENSE for details.
