FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Asmp2.Server/Asmp2.Server.csproj", "Asmp2.Server/"]
COPY ["Asmp2.Shared/Asmp2.Shared.csproj", "Asmp2.Shared/"]
COPY ["Asmp2.Client/Asmp2.Client.csproj", "Asmp2.Client/"]
COPY ["Asmp2.Server.Tests.Unit/Asmp2.Server.Tests.Unit.csproj", "Asmp2.Server.Tests.Unit/"]
RUN dotnet restore "Asmp2.Server/Asmp2.Server.csproj"
RUN dotnet restore "Asmp2.Server.Tests.Unit/Asmp2.Server.Tests.Unit.csproj"
COPY . .
RUN dotnet test "Asmp2.Server.Tests.Unit/Asmp2.Server.Tests.Unit.csproj"
WORKDIR "/src/Asmp2.Server"
RUN dotnet build "Asmp2.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Asmp2.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Asmp2.Server.dll"]
