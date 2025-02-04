The docker image can be pulled from the following public Azure Container Registry:

receiptprocessorapi.azurecr.io/fetchapi:latest

# Running the app with HTTPS

In order for HTTPS requests to work, you will need to mount a volume containing an HTTPS certificate to the container.

There are many ways to go about this, but I found using dotnet dev-certs to be the easiest.

dotnet dev-certs https -ep <PATH_TO_CERT>\<CERT_NAME> -p <PASSWORD>
dotnet dev-certs https --trust

Then run the docker image using the following command:

docker run --rm -it -p <HOST_HTTP_PORT>:8080 -p <HOST_HTTPS_PORT>:8081 -e ASPNETCORE_URLS="https://0.0.0.0:8081;http://0.0.0.0:8080" -e ASPNETCORE_HTTPS_PORTS=8081 -e ASPNETCORE_Kestrel__Certificates__Default__Password=<PASSWORD> -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/<CERT_NAME> -v <PATH_TO_CERT>:/https/ receiptprocessorapi.azurecr.io/fetchapi:latest

# Easier approach without HTTPS

If you are alright with making only HTTP requests, run the following command to pull and run the image from the registry:

docker run -it -p <HOST_PORT>:8080 receiptprocessorapi.azurecr.io/fetchapi:latest

The host port is up to you. You should then be able to make requests to http://localhost:<HOST_PORT>.
