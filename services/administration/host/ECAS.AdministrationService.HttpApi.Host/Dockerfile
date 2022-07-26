#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/administration/NuGet.Config", "services/administration/"]
COPY ["services/identity/NuGet.Config", "services/identity/"]
COPY ["services/administration/host/ECAS.AdministrationService.HttpApi.Host/ECAS.AdministrationService.HttpApi.Host.csproj", "services/administration/host/ECAS.AdministrationService.HttpApi.Host/"]
COPY ["services/administration/src/ECAS.AdministrationService.Application/ECAS.AdministrationService.Application.csproj", "services/administration/src/ECAS.AdministrationService.Application/"]
COPY ["services/administration/src/ECAS.AdministrationService.Domain/ECAS.AdministrationService.Domain.csproj", "services/administration/src/ECAS.AdministrationService.Domain/"]
COPY ["services/administration/src/ECAS.AdministrationService.Domain.Shared/ECAS.AdministrationService.Domain.Shared.csproj", "services/administration/src/ECAS.AdministrationService.Domain.Shared/"]
COPY ["services/administration/src/ECAS.AdministrationService.Application.Contracts/ECAS.AdministrationService.Application.Contracts.csproj", "services/administration/src/ECAS.AdministrationService.Application.Contracts/"]
COPY ["services/administration/src/ECAS.AdministrationService.HttpApi/ECAS.AdministrationService.HttpApi.csproj", "services/administration/src/ECAS.AdministrationService.HttpApi/"]
COPY ["services/administration/src/ECAS.AdministrationService.EntityFrameworkCore/ECAS.AdministrationService.EntityFrameworkCore.csproj", "services/administration/src/ECAS.AdministrationService.EntityFrameworkCore/"]
COPY ["shared/ECAS.Shared.Consts/ECAS.Shared.Consts.csproj", "shared/ECAS.Shared.Consts/"]
COPY ["services/identity/src/ECAS.IdentityService.Application.Contracts/ECAS.IdentityService.Application.Contracts.csproj", "services/identity/src/ECAS.IdentityService.Application.Contracts/"]
COPY ["services/identity/src/ECAS.IdentityService.Domain.Shared/ECAS.IdentityService.Domain.Shared.csproj", "services/identity/src/ECAS.IdentityService.Domain.Shared/"]
COPY ["shared/ECAS.Shared.Hosting/ECAS.Shared.Hosting.csproj", "shared/ECAS.Shared.Hosting/"]
COPY ["services/identity/src/ECAS.IdentityService.EntityFrameworkCore/ECAS.IdentityService.EntityFrameworkCore.csproj", "services/identity/src/ECAS.IdentityService.EntityFrameworkCore/"]
COPY ["services/identity/src/ECAS.IdentityService.Domain/ECAS.IdentityService.Domain.csproj", "services/identity/src/ECAS.IdentityService.Domain/"]
RUN dotnet restore "services/administration/host/ECAS.AdministrationService.HttpApi.Host/ECAS.AdministrationService.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/services/administration/host/ECAS.AdministrationService.HttpApi.Host"
RUN dotnet build "ECAS.AdministrationService.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECAS.AdministrationService.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECAS.AdministrationService.HttpApi.Host.dll"]