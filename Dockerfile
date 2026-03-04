# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# solution ve tüm csproj dosyalarını kopyala
COPY CI_CD_Test.sln .
COPY CI_CD_Test.WebAPI/CI_CD_Test.WebAPI.csproj CI_CD_Test.WebAPI/
COPY CI_CD_Test.Domain/CI_CD_Test.Domain.csproj CI_CD_Test.Domain/
COPY CI_CD_Test.Infrastructure/CI_CD_Test.Infrastructure.csproj CI_CD_Test.Infrastructure/
COPY CI_CD_Test.Tests/CI_CD_Test.Tests.csproj CI_CD_Test.Tests/

# restore
RUN dotnet restore

# tüm dosyaları kopyala
COPY . .

# publish
RUN dotnet publish CI_CD_Test.WebAPI/CI_CD_Test.WebAPI.csproj \
    -c Release \
    -o /app/publish

# ---------- RUNTIME STAGE ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CI_CD_Test.WebAPI.dll"]