using System.Security.Cryptography;
using System.Text.Json;
using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace KeyGenerator;

public class KeyManager
{
    private readonly AmazonSecretsManagerClient _client;
    private const int DefaultRsaKeySize = 2048;

    public KeyManager()
    {
        var credentials = new BasicAWSCredentials(AwsCredentials.AccessKey, AwsCredentials.SecretKey);
        var regionEndpoint = RegionEndpoint.GetBySystemName(AwsCredentials.Region);
        _client = new AmazonSecretsManagerClient(credentials, regionEndpoint);
        Console.WriteLine($"AWS credentials set for region {AwsCredentials.Region}");
    }
    
    public async Task CreateKeyAsync(string secretName, string? description = null)
    {
        Console.WriteLine("Generating a secure RSA key pair for JWT...");

        var (privateKey, publicKey) = KeyGenerator.GenerateRsaKey(DefaultRsaKeySize);

        var secretValue = new Dictionary<string, string>
        {
            { "privateKeyPem", privateKey },
            { "publicKeyPem", publicKey },
            { "keyType", "RSA" },
            { "algorithm", "RS256" },
            { "keySize", DefaultRsaKeySize.ToString() },
            { "createdAt", DateTime.UtcNow.ToString("o") }
        };

        var secretString = JsonSerializer.Serialize(secretValue);

        var request = new CreateSecretRequest
        {
            Name = secretName,
            Description = description ?? $"RSA key pair for JWT created on {DateTime.UtcNow:yyyy-MM-dd}",
            SecretString = secretString,
            Tags =
            [
                new Tag { Key = "Application", Value = "Drvnx.KeyGenerator" },
                new Tag { Key = "Environment", Value = "Production" },
                new Tag { Key = "KeyType", Value = "RSA" },
                new Tag { Key = "Usage", Value = "JWT" }
            ]
        };

        try
        {
            var response = await _client.CreateSecretAsync(request);
            Console.WriteLine($"RSA key pair created successfully with ARN: {response.ARN}");
        }
        catch (ResourceExistsException)
        {
            Console.WriteLine("A secret with this name already exists. Use rotation instead.");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating RSA key: {ex.Message}");
            throw;
        }
    }
    
    public async Task RotateKeyAsync(string secretName)
    {
        Console.WriteLine("Rotating RSA key pair...");

        try
        {
            // Retrieve the existing secret to maintain any metadata
            var getRequest = new GetSecretValueRequest { SecretId = secretName };
            var getResponse = await _client.GetSecretValueAsync(getRequest);

            // Parse the existing secret
            var existingSecret = JsonSerializer.Deserialize<Dictionary<string, string>>(getResponse.SecretString)
                ?? throw new InvalidOperationException("Failed to parse the existing secret.");

            var (privateKey, publicKey) = KeyGenerator.GenerateRsaKey(DefaultRsaKeySize);
            
            existingSecret["privateKeyPem"] = privateKey;
            existingSecret["publicKeyPem"] = publicKey;
            existingSecret["updatedAt"] = DateTime.UtcNow.ToString("o");

            // Update the secret
            var updateRequest = new UpdateSecretRequest
            {
                SecretId = secretName,
                SecretString = JsonSerializer.Serialize(existingSecret)
            };

            await _client.UpdateSecretAsync(updateRequest);
            Console.WriteLine("RSA key pair rotated successfully.");
        }
        catch (ResourceNotFoundException)
        {
            Console.WriteLine("Secret not found. Create a new RSA key instead.");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rotating RSA key: {ex.Message}");
            throw;
        }
    }
    
    
    public async Task DeleteKeyAsync(string secretName, bool forceDelete = false)
    {
        Console.WriteLine($"Deleting key '{secretName}'...");

        try
        {
            // Create delete request
            var request = new DeleteSecretRequest
            {
                SecretId = secretName
            };

            // For force delete, don't include RecoveryWindowInDays (AWS will use 0)
            // Otherwise set to 7 days (minimum allowed)
            if (!forceDelete)
            {
                // Set to minimum allowed value of 7 days
                request.RecoveryWindowInDays = 7;
            }
            else
            {
                // For force delete, set ForceDeleteWithoutRecovery to true instead
                request.ForceDeleteWithoutRecovery = true;
            }

            var response = await _client.DeleteSecretAsync(request);

            if (forceDelete)
            {
                Console.WriteLine($"Key '{secretName}' permanently deleted.");
            }
            else
            {
                Console.WriteLine($"Key '{secretName}' scheduled for deletion on {response.DeletionDate.ToLocalTime()}");
                Console.WriteLine("You can recover this secret before the scheduled deletion date.");
            }
        }
        catch (ResourceNotFoundException)
        {
            Console.WriteLine($"Secret '{secretName}' not found.");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting key: {ex.Message}");
            throw;
        }
    }
}