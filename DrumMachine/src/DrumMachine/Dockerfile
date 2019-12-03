FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build

WORKDIR /src
COPY *.sln ./

COPY ["src/DrumMachine/DrumMachine.csproj", "src/DrumMachine/"]

RUN dotnet restore "src/DrumMachine/DrumMachine.csproj"
COPY . .
WORKDIR /src/src/DrumMachine
RUN dotnet build "DrumMachine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DrumMachine.csproj" -c Release -o /app/publish

# use runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

EXPOSE 5001

CMD ["dotnet", "publish/DrumMachine.dll"]