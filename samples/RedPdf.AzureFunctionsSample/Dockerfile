# RedPdf on Azure Functions (isolated worker, containerized). Build from the repository root:
#   docker build -f samples/RedPdf.AzureFunctionsSample/Dockerfile -t redpdf-functions-sample .
#   docker run --rm -p 8080:80 redpdf-functions-sample
#   curl -o render.pdf http://localhost:8080/api/render
#
# The same image deploys to Azure Container Apps or a Functions Premium/Dedicated plan.
# On a Consumption plan (no custom containers) set REDPDF_LICENSE_KEY and the sample uses
# the AzureFunctions preset, which configures Chromium for the Azure sandbox.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY samples/RedPdf.AzureFunctionsSample/ .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0
# Debian's Chromium plus fonts; RedPdf finds it through REDPDF_CHROMIUM_PATH, so the image
# never downloads a browser at runtime.
RUN apt-get update \
    && apt-get install -y --no-install-recommends chromium fonts-liberation \
    && rm -rf /var/lib/apt/lists/*
ENV REDPDF_CHROMIUM_PATH=/usr/bin/chromium \
    AzureWebJobsSecretStorageType=files \
    AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=build /app /home/site/wwwroot
