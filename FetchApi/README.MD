# Running the app:

The app can be run one of two ways;

1.) Using Visual Studio.

Clone the repo, open the sln file in the root directory using Visual Studio, and select "Container (DockerFile)" from the run menu. Running the app from this menu should build the image based on the docker file and subsequently run the image.

2.) Using the command line.

Navigate to the root folder and run the following command to build the container image:

docker build -t IMAGE_NAME .

Then run the docker image using the following command:

docker run -it -p HOST_PORT:CONTAINER_PORT IMAGE_NAME

I would recommend using 8080 or 8081 for CONTAINER_PORT as those are what the Dockerfile exposes.

IMPORTANT NOTE: If you choose to use the command-line approach, you may have to make the requests over HTTP instead of HTTPS, as I encountered SSL issues when running the application in such a manner on my machine. Visual Studio runs the image in such a way that HTTPS commands work.

If it helps, you can try using dotnet dev certs to generate an SSL certificate on your local machine, mount the certificate's folder to the container, and configuring environmenmt variables for the container such that ASP.NET looks in the container for the certificates.

See the following reference for further detail:

https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-9.0



