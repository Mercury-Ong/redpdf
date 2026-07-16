# RedPdf in Docker. Build from the repository root:
#   docker build -f samples/RedPdf.DockerSample/Dockerfile -t redpdf-docker-sample .
#   docker run --rm -v "$PWD/out:/out" -e REDPDF_OUT_DIR=/out redpdf-docker-sample
#
# Pass a license (or 30-day trial) key to use the supported LinuxContainer preset:
#   docker run --rm -v "$PWD/out:/out" -e REDPDF_OUT_DIR=/out \
#     -e REDPDF_LICENSE_KEY=REDPDF.... redpdf-docker-sample

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY samples/RedPdf.DockerSample/ .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:8.0
# Debian's Chromium plus fonts; RedPdf finds it through REDPDF_CHROMIUM_PATH, so the image
# never downloads a browser at runtime. Alternatively reference the RedPdf.Chromium.linux-x64
# package and drop these two layers.
RUN apt-get update \
    && apt-get install -y --no-install-recommends chromium fonts-liberation \
    && rm -rf /var/lib/apt/lists/*
ENV REDPDF_CHROMIUM_PATH=/usr/bin/chromium

WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "RedPdf.DockerSample.dll"]
