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
            // Prompt for AWS credentials if environment variables not set
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID")) ||
                string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")))
            {
                Console.WriteLine("AWS credentials not found in environment. Please provide credentials:");

                Console.Write("AWS Access Key ID: ");
                var accessKey = Console.ReadLine();

                if (string.IsNullOrEmpty(accessKey))
                {
                    Console.WriteLine("AWS Access Key ID cannot be empty. Please try again.");
                    continue;
                }

                Console.Write("AWS Secret Access Key: ");
                var secretKey = Console.ReadLine();

                if (string.IsNullOrEmpty(secretKey))
                {
                    Console.WriteLine("AWS Access Key ID cannot be empty. Please try again.");
                    continue;
                }

                Console.Write("AWS Region (default: us-east-1): ");
                var region = Console.ReadLine();

                if (string.IsNullOrEmpty(region))
                {
                    region = "us-east-1";
                }

                shouldContinue = false;
                AwsCredentials.SetAwsCredentials(accessKey, secretKey, region);
            }
            else
            {
                Console.WriteLine("AWS credentials found in environment variables.");
                shouldContinue = false;
            }
        }
        while (shouldContinue);
    }
}