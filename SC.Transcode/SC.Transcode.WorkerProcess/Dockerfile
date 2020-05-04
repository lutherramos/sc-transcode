 
FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src

COPY ./NuGet.config .
COPY ["SC.AmazonCopy.Worker/SC.AmazonCopy.Worker.csproj", "SC.AmazonCopy.Worker/"]
COPY ["SC.AmazonCopy.Service/SC.AmazonCopy.Service.csproj", "SC.AmazonCopy.Service/"]

Run dotnet nuget locals all --clear
RUN dotnet restore "SC.AmazonCopy.Worker/SC.AmazonCopy.Worker.csproj"

COPY . .

WORKDIR "/src/SC.AmazonCopy.Worker"
RUN dotnet build "SC.AmazonCopy.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SC.AmazonCopy.Worker.csproj" -c Release -o /app/publish

FROM base AS final

ENV ASPNETCORE_ENVIRONMENT env

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SC.AmazonCopy.Worker.dll"]