#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["InstitutionService/InstitutionService.csproj", "InstitutionService/"]
RUN dotnet restore "InstitutionService/InstitutionService.csproj"
COPY ./ .
WORKDIR "/src/InstitutionService"
RUN dotnet build "InstitutionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InstitutionService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InstitutionService.dll"]