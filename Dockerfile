# ==========================================================
# ЕТАП 1: BUILD
# ==========================================================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1. Копіюємо ВЕСЬ вміст репозиторію
COPY . . 

# 2. Відновлюємо залежності, використовуючи правильну назву файлу
RUN dotnet restore "CarSharing.API.csproj"

# 3. Публікуємо (компілюємо) додаток.
RUN dotnet publish "CarSharing.API.csproj" -c Release -o /app/publish

# ==========================================================
# ЕТАП 2: FINAL
# ==========================================================
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Копіюємо скомпільовані файли з етапу 'build'
COPY --from=build /app/publish .

# Визначаємо команду запуску
ENTRYPOINT ["dotnet", "CarSharing.API.dll"]