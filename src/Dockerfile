FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DecisionHelper.API/DecisionHelper.API.csproj", "DecisionHelperAPI/"]
RUN dotnet restore "DecisionHelperAPI/DecisionHelper.API.csproj"
WORKDIR "/src/DecisionHelperAPI"
COPY . .
RUN dotnet build "./DecisionHelper.API/DecisionHelper.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DecisionHelper.API/DecisionHelper.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DecisionHelper.API.dll"]