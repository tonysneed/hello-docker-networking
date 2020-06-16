FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
# EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["DockerNetworking.Api.csproj", "./"]
RUN dotnet restore "./DockerNetworking.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DockerNetworking.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerNetworking.Api.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Production

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerNetworking.Api.dll"]

# dotnet run

# docker build -t docker-networking-api .
# docker run -it --rm --name docker-networking-api -p 80:80 docker-networking-api

# docker-compose build
# docker-compose up
