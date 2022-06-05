#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Consumers/Emails/Core31.Consumers.CreateEmail/Core31.Consumers.CreateEmail.csproj", "Consumers/Emails/Core31.Consumers.CreateEmail/"]
COPY ["Consumers/Emails/Core31.Consumers.CreateEmail.Data/Core31.Consumers.CreateEmail.Data.csproj", "Consumers/Emails/Core31.Consumers.CreateEmail.Data/"]
COPY ["Shared/Core31.Shared.Models/Core31.Shared.Models.csproj", "Shared/Core31.Shared.Models/"]
RUN dotnet restore "Consumers/Emails/Core31.Consumers.CreateEmail/Core31.Consumers.CreateEmail.csproj"
COPY . .
WORKDIR "/src/Consumers/Emails/Core31.Consumers.CreateEmail"
RUN dotnet build "Core31.Consumers.CreateEmail.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Core31.Consumers.CreateEmail.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core31.Consumers.CreateEmail.dll"]