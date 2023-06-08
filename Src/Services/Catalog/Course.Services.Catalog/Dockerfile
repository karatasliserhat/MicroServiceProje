FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
EXPOSE 80
COPY Src/Shared/MicroService.Shareds/*.csproj Src/Shared/MicroService.Shareds/
COPY Src/Services/Catalog/Course.Services.Catalog/*.csproj Src/Services/Catalog/Course.Services.Catalog/
RUN dotnet restore Src/Services/Catalog/Course.Services.Catalog/*.csproj
COPY . ./
RUN dotnet publish Src/Services/Catalog/Course.Services.Catalog/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","Course.Services.Catalog.dll" ]

