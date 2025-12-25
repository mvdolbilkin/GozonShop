FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Gozon.Payments/Gozon.Payments.Api/Gozon.Payments.Api.csproj", "Gozon.Payments/Gozon.Payments.Api/"]
COPY ["Gozon.Shared/Gozon.Shared/Gozon.Shared.csproj", "Gozon.Shared/Gozon.Shared/"]

RUN dotnet restore "Gozon.Payments/Gozon.Payments.Api/Gozon.Payments.Api.csproj"

COPY . .

WORKDIR "/src/Gozon.Payments/Gozon.Payments.Api"
RUN dotnet build "Gozon.Payments.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Gozon.Payments.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gozon.Payments.Api.dll"]