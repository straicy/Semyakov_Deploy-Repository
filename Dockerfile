# ==========================================================
# ЕТАП 1: BUILD
# ==========================================================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# ВАЖЛИВО: Замініть "MyWebApi.csproj" на фактичну назву вашого .csproj
# Якщо назва інша, наприклад, "CarSharing.API.csproj", використовуйте її!
COPY ["MyWebApi.csproj", "."]

RUN dotnet restore "MyWebApi.csproj"

COPY . .

# Використовуємо ту ж назву для публікації
RUN dotnet publish "MyWebApi.csproj" -c Release -o /app/publish

# ==========================================================
# ЕТАП 2: FINAL
# ==========================================================
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# ВАЖЛИВО: Використовуйте назву фінальної DLL
# Якщо це MyWebApi.csproj, то це MyWebApi.dll
ENTRYPOINT ["dotnet", "MyWebApi.dll"]