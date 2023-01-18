FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY . .

RUN dotnet publish "src/Ical.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0


WORKDIR /app
COPY --from=build /app ./

EXPOSE 4100

ENTRYPOINT ["dotnet", "Ical.dll"]