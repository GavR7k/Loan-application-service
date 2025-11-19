# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# копируем решение и проектные файлы
COPY ["LoanService/LoanService.sln", "LoanService/"]
COPY ["LoanService/LoanService/LoanService.csproj", "LoanService/LoanService/"]

# восстанавливаем зависимости
RUN dotnet restore "LoanService/LoanService.sln"

# копируем всё остальное
COPY . .

# публикуем решение
WORKDIR "/src/LoanService"
RUN dotnet publish "LoanService.sln" -c Release -o /app/publish

# Этап запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "LoanService.dll"]

