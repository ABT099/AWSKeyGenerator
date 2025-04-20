# AWS Key Generator: Secure Key Management CLI

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Version](https://img.shields.io/badge/.NET-8.0-blueviolet.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![NuGet Version](https://img.shields.io/nuget/v/ABT099.AWSKeyGenerator.svg)](https://www.nuget.org/packages/ABT099.AWSKeyGenerator/)

**AWS Key Generator** is a command-line interface (CLI) tool built with .NET 8 designed to streamline the secure generation and management of cryptographic keys within AWS Secrets Manager. This project demonstrates practical application of cloud integration, security best practices, and modern .NET development, packaged as an easily installable .NET global tool.

**View this project on GitHub: [https://github.com/ABT099/AWSKeyGenerator](https://github.com/ABT099/AWSKeyGenerator)** 

## Project Overview

Managing cryptographic keys (like RSA pairs for JWT signing) securely is crucial for application security. Manually generating, storing, and rotating these keys can be error-prone and complex. This tool automates the process by:

1.  Generating cryptographically secure RSA key pairs locally.
2.  Interacting directly with the AWS Secrets Manager API to store these keys securely.
3.  Providing straightforward commands for key creation, rotation, and deletion, adhering to AWS best practices.
4.  Distributing the tool as a .NET global tool for easy installation and use.

This project was built to solve a common developer need and showcase skills in cloud services integration, security-conscious development, building practical CLI tools, and CI/CD automation for publishing.

## Key Features & Skills Demonstrated

*   **Secure RSA Key Generation:**
    *   Utilizes `System.Security.Cryptography` for generating robust RSA public/private key pairs (default 2048-bit).
    *   Exports keys to standard PEM format.
    *   *Skills: C#, .NET Standard Libraries, Cryptography Basics*
*   **AWS Secrets Manager Integration:**
    *   Uses the official AWS SDK for .NET (`AWSSDK.SecretsManager`) to interact with Secrets Manager.
    *   Handles secret creation, retrieval (for rotation), updates, and deletion.
    *   Includes tagging secrets for better organization within AWS.
    *   *Skills: AWS SDK, Cloud Services (Secrets Manager), API Integration, IAM Fundamentals*
*   **Secure Credential Handling:**
    *   Prioritizes AWS credentials from standard environment variables (`AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, `AWS_REGION`).
    *   Provides an interactive prompt as a fallback, preventing hardcoding of sensitive information.
    *   *Skills: Security Best Practices, Environment Configuration*
*   **.NET Global Tool Packaging:**
    *   Configured project (`.csproj`) for packaging as a .NET tool using `<PackAsTool>`.
    *   Defines a user-friendly command name (`aws-key-gen`).
    *   *Skills: .NET Tool Ecosystem, NuGet Packaging*
*   **Automated Publishing with GitHub Actions:**
    *   Implements a CI/CD workflow (`.github/workflows/publish.yml`) to automatically build, version (using Git tags), pack, and publish the tool to NuGet.org on tagged releases (`v*.*.*`).
    *   Uses GitHub Secrets for secure handling of the NuGet API key.
    *   *Skills: CI/CD, GitHub Actions, YAML, Automated Deployments, Versioning Strategy*
*   **Key Lifecycle Management:**
    *   Implements key **creation**, **rotation** (updating the key pair within an existing secret), and **deletion** (both soft delete with recovery window and force delete).
    *   Stores relevant metadata (key type, size, timestamps) alongside keys in Secrets Manager.
    *   *Skills: Key Management Concepts, API Design*
*   **User-Friendly CLI:**
    *   Provides a simple, interactive menu-driven experience for ease of use.
    *   Includes clear prompts, feedback messages, and error handling.
    *   *Skills: Console Application Development, User Interface Design (CLI)*

## Technology Stack

*   **Language:** C#
*   **Framework:** .NET 8
*   **Cloud Provider:** Amazon Web Services (AWS)
*   **AWS Services:** Secrets Manager
*   **Key Libraries:**
    *   `AWSSDK.SecretsManager`
    *   `System.Security.Cryptography`
    *   `System.Text.Json`
*   **CI/CD:** GitHub Actions
*   **Package Manager:** NuGet

## Installation (as .NET Tool)

The easiest way to use the AWS Key Generator is by installing it as a .NET global tool.

1.  **Prerequisite:** Ensure you have the [.NET SDK (version 8.0 or later)](https://dotnet.microsoft.com/download) installed.
2.  **Install:** Open your terminal or command prompt and run:
    ```bash
    dotnet tool install --global ABT099.AWSKeyGenerator
    ```
3.  **Update (Optional):** To update to the latest version later:
    ```bash
    dotnet tool update --global ABT099.AWSKeyGenerator
    ```
4.  **Uninstall (Optional):** To remove the tool:
    ```bash
    dotnet tool uninstall --global ABT099.AWSKeyGenerator
    ```

## Usage (After Installation)

Once installed, you can run the tool directly from your terminal using the command `aws-key-gen`.

### 1. Environment Setup

The tool needs AWS credentials to interact with Secrets Manager. Configure them using one of the following methods:

*   **Environment Variables (Recommended):** Set these variables in your terminal session or system environment:
    ```bash
    export AWS_ACCESS_KEY_ID="YOUR_ACCESS_KEY"
    export AWS_SECRET_ACCESS_KEY="YOUR_SECRET_KEY"
    export AWS_REGION="us-east-1" # Or your desired AWS region
    ```
*   **Interactive Prompt:** If the environment variables are not found, the tool will securely prompt you to enter the Access Key ID, Secret Access Key, and Region.

### 2. Running the Tool

Execute the command:

```bash
aws-key-gen


You will be presented with an interactive menu:

Welcome to DRVNX Key Generator, Made by the Towait himself!
AWS credentials set for region <your-region>

Select an option:
1. Create a key
2. Rotate a key
3. Delete a key
4. Exit
Enter your choice:

Option 1 (Create): Prompts for a secret name, generates a new RSA key pair, and stores it in AWS Secrets Manager.

Option 2 (Rotate): Prompts for the name of an existing secret, generates a new key pair, and updates the secret in Secrets Manager.

Option 3 (Delete): Prompts for a secret name and offers either a soft delete (with a 7-day recovery window) or a force delete (permanent, immediate deletion). Use force delete with extreme caution.

Option 4 (Exit): Closes the application.

```

Required IAM Permissions

The AWS credentials used must belong to an IAM user or role with permissions to interact with Secrets Manager. The minimum required permissions are:

secretsmanager:CreateSecret

secretsmanager:GetSecretValue

secretsmanager:UpdateSecret

secretsmanager:DeleteSecret

secretsmanager:DescribeSecret

secretsmanager:TagResource

Example IAM Policy:

```` json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "secretsmanager:CreateSecret",
                "secretsmanager:GetSecretValue",
                "secretsmanager:UpdateSecret",
                "secretsmanager:DeleteSecret",
                "secretsmanager:DescribeSecret",
                "secretsmanager:TagResource"
            ],
            "Resource": "arn:aws:secretsmanager:<your-region>:<your-account-id>:secret:<secret-name-prefix>*"
            // Example Resource: "arn:aws:secretsmanager:us-east-1:123456789012:secret:myapp/keys/*"
            // Or use "*" for less restrictive access (use with caution)
        }
    ]
}
````