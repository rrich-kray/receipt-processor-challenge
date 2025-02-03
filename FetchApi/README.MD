# Running the app:

The app can be run one of two ways;

1.) Using Visual Studio.

Clone the repo, open the sln file in the root directory using Visual Studio, and select "Container (DockerFile)" from the run menu. Running the app from this menu should build the image based on the docker file and subsequently run the image.

2.) Using the command line.

This requires Docker to be installed on the host device.

Navigate to the root folder and run the following command to build the container image:

docker build -t IMAGE_NAME .

Then run the docker image using the following command:

docker run -it -p HOST_PORT:CONTAINER_PORT IMAGE_NAME

I would recommend using 8080 or 8081 for CONTAINER_PORT as those are what the Dockerfile exposes.

NOTE: If run via the command line, requests will only work over HTTP at this time.