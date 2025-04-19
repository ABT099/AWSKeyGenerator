namespace KeyGenerator;

public static class OnBoarding
{
    public static void OnBoard()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Welcome to DRVNX Key Generator, Made by the Towait himself!");

        Console.ForegroundColor = ConsoleColor.White;
        
        var shouldContinue = true;

        do
        {
            var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            var region = Environment.GetEnvironmentVariable("AWS_REGION");
            
            // Prompt for AWS credentials if environment variables not set
            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                Console.WriteLine("AWS credentials not found in environment. Please provide credentials:");

                Console.Write("AWS Access Key ID: ");
                accessKey = Console.ReadLine();

                if (string.IsNullOrEmpty(accessKey))
                {
                    Console.WriteLine("AWS Access Key ID cannot be empty. Please try again.");
                    continue;
                }

                Console.Write("AWS Secret Access Key: ");
                secretKey = Console.ReadLine();

                if (string.IsNullOrEmpty(secretKey))
                {
                    Console.WriteLine("AWS Access Key ID cannot be empty. Please try again.");
                    continue;
                }
            }
            
            if (string.IsNullOrEmpty(region))
            {
                Console.WriteLine("Access Key Found in Environment!");
                Console.WriteLine("Secret Key Found in Environment!");
            
                Console.Write("AWS Region (default: us-east-1): ");
                region = Console.ReadLine();

                if (string.IsNullOrEmpty(region))
                {
                    region = "us-east-1";
                }

                Console.WriteLine($"AWS Region Set To: {region}");
            }

            shouldContinue = false;

            if (
                !string.IsNullOrWhiteSpace(accessKey) &&
                !string.IsNullOrWhiteSpace(secretKey) &&
                !string.IsNullOrWhiteSpace(region))
            {
                AwsCredentials.SetAwsCredentials(accessKey, secretKey, region);
            }
        }
        while (shouldContinue);
    }
}