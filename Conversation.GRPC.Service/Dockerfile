#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Conversation.GRPC.Service/Conversation.GRPC.Service.csproj", "Conversation.GRPC.Service/"]
RUN dotnet restore "Conversation.GRPC.Service/Conversation.GRPC.Service.csproj"
COPY . .
WORKDIR "/src/Conversation.GRPC.Service"
RUN dotnet build "Conversation.GRPC.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Conversation.GRPC.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Conversation.GRPC.Service.dll"]