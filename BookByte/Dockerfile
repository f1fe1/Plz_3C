#Для створення контейнера з .NET додатком. 
#Використання базового образу .NET SDK для версії 8.0:
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
#Встановлення робочого каталогу в /app:
WORKDIR /app

#Копіювання файлів проекту та файлу .csproj:
COPY *.csproj ./
#Копіювання всіх інших файлів проекту:
COPY . ./

#Запуск dotnet publish для збірки проекту:
RUN dotnet publish -c Release -o out
#Використання базового образу ASP.NET для версії 8.0:
FROM mcr.microsoft.com/dotnet/aspnet:8.0
#Встановлення робочого каталогу в /app:
WORKDIR /app
#Копіювання результатів збірки (виконаного етапу збірки) в контейнер:
COPY --from=build-env /app/out .
#Встановлення точки входу для контейнера:
ENTRYPOINT [ "dotnet", "BookByte.dll" ]