#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Core31.Gateway.Api/Core31.Gateway.Api.csproj", "Core31.Gateway.Api/"]
RUN dotnet restore "Core31.Gateway.Api/Core31.Gateway.Api.csproj"
COPY . .
WORKDIR "/src/Core31.Gateway.Api"
RUN dotnet build "Core31.Gateway.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Core31.Gateway.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core31.Gateway.Api.dll"]