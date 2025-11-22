# ==========================================================
# ЕТАП 1: BUILD (Використовуємо образ, де є повний SDK)
# ==========================================================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Копіюємо файл проекту MyWebApi.csproj у робочу директорію
COPY ["MyWebApi.csproj", "."]

# Відновлюємо залежності (використовуючи файл .csproj)
RUN dotnet restore "MyWebApi.csproj"

# Копіюємо решту файлів проекту
COPY . .

# Публікуємо (компілюємо) додаток і поміщаємо результат у /app/publish
# Тут MyWebApi.csproj - це назва твого файлу проекту
RUN dotnet publish "MyWebApi.csproj" -c Release -o /app/publish

# ==========================================================
# ЕТАП 2: FINAL (Використовуємо образ, де є лише Runtime)
# ==========================================================
# Ми перемикаємося на менший образ, де є лише те, що потрібно для запуску
# (але цього достатньо, щоб виконати команду ENTRYPOINT)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Копіюємо скомпільовані файли з етапу 'build'
COPY --from=build /app/publish .

# Визначаємо команду, яка буде виконана при запуску контейнера
# MyWebApi.dll - це назва фінального файлу, скомпільованого з MyWebApi.csproj
ENTRYPOINT ["dotnet", "MyWebApi.dll"]