namespace WebApi.Settings;

public class CorsSettings
{
    public bool EnableCors { get; set; } = true;
    public bool AllowAllOrigins { get; set; }
    public bool AllowCredentials { get; set; } = true;
    public string[] AllowedOrigins { get; set; } = [];
    public string[] AllowedMethods { get; set; } = [];
    public string[] AllowedHeaders { get; set; } = [];
}

