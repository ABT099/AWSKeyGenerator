using KeyGenerator;

OnBoarding.OnBoard();

var km = new KeyManager();

await MainAsync();
return;

async Task MainAsync()
{
    var shouldExit = false;

    do
    {
        Console.WriteLine("\nSelect an option:");
        Console.WriteLine("1. Create a key");
        Console.WriteLine("2. Rotate a key");
        Console.WriteLine("3. Delete a key");
        Console.WriteLine("4. Exit");
        Console.Write("Enter your choice: ");

        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                try
                {
                    Console.WriteLine("\nCreating a new key...");
                    Console.Write("Enter secret name: ");
                    var secretName = Console.ReadLine();

                    if (string.IsNullOrEmpty(secretName))
                    {
                        Console.WriteLine("Secret name cannot be empty. Please try again.");
                        continue;
                    }

                    await km.CreateKeyAsync(secretName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while creating the key: {ex.Message}");
                    Console.WriteLine("Please try again.");
                }

                break;
            case "2":
                try
                {
                    Console.WriteLine("\nRotating a key...");
                    Console.Write("Enter secret name to rotate: ");
                    var secretName = Console.ReadLine();

                    if (string.IsNullOrEmpty(secretName))
                    {
                        Console.WriteLine("Secret name cannot be empty. Please try again.");
                        continue;
                    }

                    await km.RotateKeyAsync(secretName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while rotating the key: {ex.Message}");
                    Console.WriteLine("Please try again.");
                }

                break;
            case "3":
                try
                {
                    Console.WriteLine("\nDeleting a key...");
                    Console.Write("Enter secret name to delete: ");
                    var secretName = Console.ReadLine();

                    if (string.IsNullOrEmpty(secretName))
                    {
                        Console.WriteLine("Secret name cannot be empty. Please try again.");
                        continue;
                    }

                    Console.Write("Force delete without recovery window? (y/n, default: n): ");
                    var forceDeleteInput = Console.ReadLine()?.ToLower();
                    var forceDelete = forceDeleteInput is "y" or "yes";

                    if (forceDelete)
                    {
                        Console.WriteLine("WARNING: Force delete will permanently remove the key with NO recovery option.");
                        Console.Write("Are you sure? (type 'yes' to confirm): ");
                        var confirmation = Console.ReadLine();

                        if (confirmation?.ToLower() != "yes")
                        {
                            Console.WriteLine("Delete operation canceled.");
                            continue;
                        }
                    }

                    await km.DeleteKeyAsync(secretName, forceDelete);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while deleting the key: {ex.Message}");
                    Console.WriteLine("Please try again.");
                }

                break;
            case "4":
                Console.WriteLine("Exiting...");
                shouldExit = true;
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
    while (shouldExit is false);
}