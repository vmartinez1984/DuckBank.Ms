
# Etapa 1: Restaurar y compilar
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copiar los archivos de proyecto y restaurar dependencias
COPY *.csproj .
RUN dotnet restore

# Copiar el resto de los archivos y construir
COPY . .
RUN dotnet publish -c Release -o /out

# Etapa 2: Configurar el entorno de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /out .

# Exponer el puerto de la aplicación
EXPOSE 80
EXPOSE 443

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "DuckBank.Api.dll"]

#para ejecurar este archivo: docker build --tag ahorrosapi .