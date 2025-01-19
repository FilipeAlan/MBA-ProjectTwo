using System.Text.Json;

namespace PCF.SPA.Services.Base
{
    public abstract  class WebApiClientBase
    {
       protected static void UpdateJsonSerializerSettings(JsonSerializerOptions settings)
        {
            settings.PropertyNameCaseInsensitive = true;
        }
    }
}
