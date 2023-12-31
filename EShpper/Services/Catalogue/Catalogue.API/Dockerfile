#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV DOTNET_URLS=http://+:9000
WORKDIR /src
COPY ["Services/Catalogue/Catalogue.API/Catalogue.API.csproj", "Services/Catalogue/Catalogue.API/"]
COPY ["Services/Catalogue/Catalogue.Application/Catalogue.Application.csproj", "Services/Catalogue/Catalogue.Application/"]
COPY ["Services/Catalogue/Catalogue.Core/Catalogue.Core.csproj", "Services/Catalogue/Catalogue.Core/"]
COPY ["Services/Catalogue/Catalogue.Ifrastructure/Catalogue.Infrastructure.csproj", "Services/Catalogue/Catalogue.Ifrastructure/"]
COPY ["Infrastructure/Common.Logging/Common.Logging.csproj", "Infrastructure/Common.Logging/"]
ENTRYPOINT ["dotnet", "run"]
RUN dotnet restore "Services/Catalogue/Catalogue.API/Catalogue.API.csproj"
COPY . .
WORKDIR "/src/Services/Catalogue/Catalogue.API"
RUN dotnet build "Catalogue.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalogue.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalogue.API.dll"]