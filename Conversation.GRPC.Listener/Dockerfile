#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Conversation.GRPC.Listener/Conversation.GRPC.Listener.csproj", "Conversation.GRPC.Listener/"]
RUN dotnet restore "Conversation.GRPC.Listener/Conversation.GRPC.Listener.csproj"
COPY . .
WORKDIR "/src/Conversation.GRPC.Listener"
RUN dotnet build "Conversation.GRPC.Listener.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Conversation.GRPC.Listener.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Conversation.GRPC.Listener.dll"]