FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Infrastructure/EShpper.Identity/EShpper.Identity.csproj", "Infrastructure/EShpper.Identity/"]
RUN dotnet restore "Infrastructure/EShpper.Identity/EShpper.Identity.csproj"
COPY . .
WORKDIR "/src/Infrastructure/EShpper.Identity"
RUN dotnet build "EShpper.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EShpper.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShpper.Identity.dll"]
