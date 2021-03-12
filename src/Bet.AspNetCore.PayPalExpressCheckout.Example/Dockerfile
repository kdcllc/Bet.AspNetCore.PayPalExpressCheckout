#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Bet.AspNetCore.PayPalExpressCheckout.Example/Bet.AspNetCore.PayPalExpressCheckout.Example.csproj", "Bet.AspNetCore.PayPalExpressCheckout.Example/"]
RUN dotnet restore "Bet.AspNetCore.PayPalExpressCheckout.Example/Bet.AspNetCore.PayPalExpressCheckout.Example.csproj"
COPY . .
WORKDIR "/src/Bet.AspNetCore.PayPalExpressCheckout.Example"
RUN dotnet build "Bet.AspNetCore.PayPalExpressCheckout.Example.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bet.AspNetCore.PayPalExpressCheckout.Example.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bet.AspNetCore.PayPalExpressCheckout.Example.dll"]