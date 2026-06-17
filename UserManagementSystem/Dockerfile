FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Docker
WORKDIR /src
COPY ["UserManagementSystem.Main/UserManagementSystem.Main.csproj", "UserManagementSystem.Main/"]
COPY ["UserManagementSystem.DataAccess/UserManagementSystem.DataAccess.csproj", "UserManagementSystem.DataAccess/"]
COPY ["UserManagementSystem.Domain/UserManagementSystem.Domain.csproj", "UserManagementSystem.Domain/"]
COPY ["UserManagementSystem.Application/UserManagementSystem.Application.csproj", "UserManagementSystem.Application/"]
COPY ["UserManagementSystem.Infrastructure/UserManagementSystem.Infrastructure.csproj", "UserManagementSystem.Infrastructure/"]
COPY ["SharedResources.MessageContracts/SharedResources.MessageContracts.csproj", "SharedResources.MessageContracts/"]
RUN dotnet restore "UserManagementSystem.Main/UserManagementSystem.Main.csproj"
COPY . .
WORKDIR "/src/UserManagementSystem.Main"
RUN dotnet build "UserManagementSystem.Main.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Docker
RUN dotnet publish "UserManagementSystem.Main.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagementSystem.Main.dll"]
