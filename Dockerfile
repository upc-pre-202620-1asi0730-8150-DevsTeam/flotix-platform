FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /app
COPY FlotixPlatform.Api/*.csproj FlotixPlatform.Api/
RUN dotnet restore ./FlotixPlatform.Api
COPY . .
RUN dotnet publish ./FlotixPlatform.Api -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=builder /app/out .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
# Secrets (DATABASE_URL, DATABASE_URL_DIRECT, ...) are injected as environment variables by the host,
# never baked into the image.
ENTRYPOINT ["dotnet", "FlotixPlatform.Api.dll"]
