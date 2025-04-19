namespace KeyGenerator;

public static class AwsCredentials
{
    public static string? AccessKey { get; set; }
    public static string? SecretKey { get; set; }
    public static string? Region { get; set; }
    
    public static void SetAwsCredentials(string accessKey, string secretKey, string region = "us-east-1")
    {
        AccessKey = accessKey;
        SecretKey = secretKey;
        Region = region;
    }
}