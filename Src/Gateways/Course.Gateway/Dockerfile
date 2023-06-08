FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
EXPOSE 80
COPY Src/Gateways/Course.Gateway/*.csproj Src/Gateways/Course.Gateway/
RUN dotnet restore Src/Gateways/Course.Gateway/*.csproj
COPY . .
RUN dotnet publish Src/Gateways/Course.Gateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Course.Gateway.dll"]