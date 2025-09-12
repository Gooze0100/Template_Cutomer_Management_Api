namespace Shared;

public static class Constants
{
    public struct Config
    {
        public const string DefaultConnectionString = "DefaultConnectionString";
        public const string AppSettingsSectionKey = "AppSettings";
        public const string AuthenticationBearerSectionKey = "Authentication:Schemes:Bearer";
    }
    
    public struct CacheTags
    {
        public const string Customer = "customer";
        public const string Template = "template";
    }
}