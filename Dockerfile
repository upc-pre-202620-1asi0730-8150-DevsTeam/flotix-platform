FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /app
COPY FlotixPlatform.Api/FlotixPlatform.Api.csproj FlotixPlatform.Api/
RUN dotnet restore ./FlotixPlatform.Api/FlotixPlatform.Api.csproj
COPY . .
RUN dotnet publish ./FlotixPlatform.Api/FlotixPlatform.Api.csproj -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=builder /app/out .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "FlotixPlatform.Api.dll"]
