# ==========================================================
# ЕТАП 1: BUILD (Використовуємо образ, де є повний SDK)
# ==========================================================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1. Копіюємо файл проекту
COPY ["CarSharing.API.csproj", "."]

# 2. Відновлюємо залежності
RUN dotnet restore "CarSharing.API.csproj"

# 3. Копіюємо решту файлів (після відновлення)
COPY . .

# 4. Публікуємо (компілюємо) додаток. Результат у /app/publish
RUN dotnet publish "CarSharing.API.csproj" -c Release -o /app/publish

# ==========================================================
# ЕТАП 2: FINAL (Використовуємо менший образ Runtime для запуску)
# ==========================================================
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Копіюємо скомпільовані файли з етапу 'build'
COPY --from=build /app/publish .

# Визначаємо команду, яка буде виконана при запуску контейнера
ENTRYPOINT ["dotnet", "CarSharing.API.dll"]