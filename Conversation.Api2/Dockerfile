#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Conversation.Api2/Conversation.Api2.csproj", "Conversation.Api2/"]
RUN dotnet restore "Conversation.Api2/Conversation.Api2.csproj"
COPY . .
WORKDIR "/src/Conversation.Api2"
RUN dotnet build "Conversation.Api2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Conversation.Api2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Conversation.Api2.dll"]