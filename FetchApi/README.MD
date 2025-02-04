# Running the app:

The app can be run one of two ways. Both require Docker to be installed on the host device.

1.) Using Visual Studio.

Clone the repo, open the sln file in the root directory using Visual Studio, and select "Container (DockerFile)" from the run menu. Running the app from this menu should build the image based on the docker file and subsequently run the image.

2.) Using the command line.

Navigate to the root folder and run the following command to build the container image:

docker build -t <IMAGE_NAME> .

Running this image will allow you to make HTTP requests to the ASP.NET server, but HTTPS commands will require a dev certificate from the host device to be mapped to a volume on the container.

To accomplish this, create a dev certificate using the following commands:

dotnet dev-certs https -ep <PATH_TO_CERT>\aspnetapp.pfx -p password
dotnet dev-certs https --trust

For the sake of simplicity, the app is configured to use "aspnetapp.pfx" and "password" for the certificate name and password, respectively, but the path of the certificate is up to you.

Then run the docker image using the following command:

docker run --rm -it -p <HOST_HTTPS_PORT>:443 -p <HOST_HTTP_PORT>:80 -v <PATH_TO_CERT>:/https/ <IMAGE_NAME>

The app is also configured to use port 443 for HTTPS and port 80 for HTTP., as well as /https/ for the container's certificate path.
