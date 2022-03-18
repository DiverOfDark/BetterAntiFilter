FROM mcr.microsoft.com/dotnet/sdk:6.0 as net-builder

ARG IsProduction=false
ARG CiCommitName=local
ARG CiCommitHash=sha

WORKDIR /build
ADD AntiFilterCleaned AntiFilterCleaned
ADD AntiFilterCleaned.sln .
RUN dotnet restore

RUN dotnet publish --output out/ --configuration Release --runtime linux-x64 --self-contained true AntiFilterCleaned

FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app
COPY --from=net-builder /build/out ./net

ADD run.sh .
RUN chmod +x run.sh

ENTRYPOINT ["/app/run.sh"]
