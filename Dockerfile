# Use the official .NET 8 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set working directory
WORKDIR /app

# Copy the entire project and build it
COPY . ./
RUN dotnet build

# Install Playwright using the PowerShell script
RUN pwsh bin/Debug/net8.0/playwright.ps1 install
RUN pwsh bin/Debug/net8.0/playwright.ps1 install-deps

# Remove all project files not to reuse them occasionally
RUN rm -rf /app/*

# Run the Playwright tests
ENTRYPOINT ["dotnet", "test", "--logger:trx", "--results-directory", "/app/TestResults"]
