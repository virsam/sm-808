FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env

COPY ./src /app
COPY ./NuGet.config /app

WORKDIR /app

RUN dotnet restore DrumMachine

WORKDIR /app/DrumMachine

RUN dotnet publish -c Release -o out

# use runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:3.0

COPY --from=build-env /app/DrumMachine/out /DrumSequencing
RUN rm -rf /app

WORKDIR /DrumSequencing
CMD ["dotnet", "DrumMachine.dll"]