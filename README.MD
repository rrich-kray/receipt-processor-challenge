The docker image for the app can be pulled from the following public Azure Container Registry:

receiptprocessorapi.azurecr.io/fetchapi:latest

# Running the app with HTTPS

In order for HTTPS requests to work, you will need to mount a volume containing an HTTPS certificate to the container.

There are many ways to go about this, but I found using dotnet dev-certs to be the easiest. This requires .NET and the .NET CLI to be installed on the user's machine, so if this is not the case feel free to skip below to the next section.

dotnet dev-certs https -ep <PATH_TO_CERT>\<CERT_NAME> -p <CERT_PASSWORD>
dotnet dev-certs https --trust

Then run the docker image using the following command:

docker run --rm -it -p <HOST_HTTP_PORT>:8080 -p <HOST_HTTPS_PORT>:8081 -e ASPNETCORE_URLS="https://0.0.0.0:8081;http://0.0.0.0:8080" -e ASPNETCORE_HTTPS_PORTS=8081 -e ASPNETCORE_Kestrel__Certificates__Default__Password=<CERT_PASSWORD> -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/<CERT_NAME> -v <PATH_TO_CERT>:/https/ receiptprocessorapi.azurecr.io/fetchapi:latest

# Runnig the app without HTTPS

If you are alright with making only HTTP requests, then a much simpler approach is to just use the following command to pull and run the image:

docker run -it -p <HOST_PORT>:8080 receiptprocessorapi.azurecr.io/fetchapi:latest

The host port is up to you. You should then be able to make requests to the API through http://localhost:<HOST_PORT>.

# Running the app with Visual Studio

If you encounter any issues with the above methods, you can always clone the repository to your machine and run the app through Visual Studio.

Simply navigate to the projects root folder, open the .sln file and select "Container (Dockerfile)" in the dropdown next to the solution configuration and solution platform dropdowns in the ribbon. Visual Studio will then build and run the image from the dockerfile.
