# Running the app:

The app can be run one of two ways;

1.) Using Visual Studio.

Open the sln file in the root directory in Visual Studio, and select "Container (DockerFile)" from the run menu. Running the app from this menu should build the image based on the docker file and subsequently run the image.


2.) Using the command line.

Navigate to the root folder and run the following command to build the container image:

docker build -t IMAGE_NAME .

Then run the docker image using the following command:

docker run -it -p HOST_PORT:CONTAINER_PORT IMAGE_NAME

