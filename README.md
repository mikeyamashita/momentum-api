


<!-- build image
docker build -t momentum-api . 
publish container
docker run -d -p 5001:8080 --name momentum-api momentum-api -->

//github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md

//create certificate
macos/linux:
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p <cert-password>
dotnet dev-certs https --trust
windows: 
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p <cert-password>
dotnet dev-certs https --trust

//create application secrets
dotnet user-secrets init -p momentum-api.csproj
dotnet user-secrets -p momentum-api.csproj set "Kestrel:Certificates:Development:Password" "<cert-password>"

//build and run image and container for ssl
MacOS/Linux:
docker build --pull -t momentum-api .
docker run --name momentum-api --rm -it -p 8001:443 -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_HTTPS_PORTS=8001 -e ASPNETCORE_ENVIRONMENT=Development -v ${HOME}/.microsoft/usersecrets/:/root/.microsoft/usersecrets -e ASPNETCORE_Kestrel__Certificates__Default__Password="<cert-password>" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v ${HOME}/.aspnet/https:/https/ momentum-api

//build and publish image and container for non ssl
MacOS/Linux:
docker build --pull -t momentum-api .
docker run --name momentum-api --rm -it -p 8001:8080 -e ASPNETCORE_ENVIRONMENT=Development -v ${HOME}/.microsoft/usersecrets/:/root/.microsoft/usersecrets -v ${HOME}/.aspnet/https:/root/.aspnet/https/ momentum-api

//build and run image and container for ssl
Windows:
docker build --pull -t momentum-api .
docker run --name momentum-api --rm -it -p 8001:443 -e ASPNETCORE_URLS="https://+" -e ASPNETCORE_HTTPS_PORTS=8001 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_Kestrel__Certificates__Default__Password="<password>" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v $env:USERPROFILE\.aspnet\https:/https/ momentum-api

// create postgres docker container
postgres setup:
docker run --name postgres-momentum -p 5433:5432 -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=<password> -d postgres-momentum
dotnet tool install --global dotnet-ef
dotnet ef database update
