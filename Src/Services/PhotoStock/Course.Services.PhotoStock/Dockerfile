FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
EXPOSE 80
COPY Src/Shared/MicroService.Shareds/*.csproj Src/Shared/MicroService.Shareds/
COPY Src/Services/PhotoStock/Course.Services.PhotoStock/*.csproj Src/Services/PhotoStock/Course.Services.PhotoStock/
RUN dotnet restore Src/Services/PhotoStock/Course.Services.PhotoStock/*.csproj
COPY . ./
RUN dotnet publish Src/Services/PhotoStock/Course.Services.PhotoStock/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","Course.Services.PhotoStock.dll" ]