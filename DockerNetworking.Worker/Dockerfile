FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["DockerNetworking.Worker.csproj", "./"]
RUN dotnet restore "./DockerNetworking.Worker.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DockerNetworking.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerNetworking.Worker.csproj" -c Release -o /app/publish

FROM base AS final
ENV DOTNET_ENVIRONMENT=Production
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerNetworking.Worker.dll"]

# dotnet run

# docker build -t docker-networking-worker .
# docker run -it --rm --name docker-networking-worker docker-networking-worker

# docker-compose build
# docker-compose up
