# Hello Docker Networking

Demo of using two `docker-compose.yml` files that share the name network.

> **Prerequisites**: Install Docker for Desktop.

## Run without Docker

1. First start the **.Api** project.
    ```
    cd DockerNetworking.Api
    dotnet run
    ```

2. Then start the **.Worker** project.
    ```
    cd DockerNetworking.Worker
    dotnet run
    ```

    - The **launchSettings.json** file set the `DOTNET_ENVIRONMENT` environment variable to `Development`.
    - The **appsettings.Development.json** file sets the value of `ClientOptions.ApiHostName` to `localhost:5000`.
    - The `Worker.ExecuteAsync` method has a loop in which the `weatherforecast` endpoint in the **.Api** service is called.
    - The host name is retrieved from the `clientOptions.ApiHostName` setting.

## Run with Docker

1. Build and run the **.Api** container image.
    ```
    docker build -t docker-networking-api .
    docker run -it --rm --name docker-networking-api -p 80:80 docker-networking-api
    ```

2. Build and run the **.Worker** container image.
    ```
    docker build -t docker-networking-worker .
    docker run -it --rm --name docker-networking-worker docker-networking-worker
    ```

    - Note that the **.Worker** service times out on the HTTP call to the **.Api** service. For this to work both services need to reside in the same network.

> **Note:** While it is possible to place both services in the same `docker-compose.yml` file, this demo will illustrate how to connect services running in two separate `docker-compose.yml` files.

## Run with Docker Compose

1. Build and run the **.Api** service using `docker-compose`.
    ```
    docker-compose build
    docker-compose up
    ```

    - A `my-network` named bridge network is created and the `docker-networking-api` service is added to it.
    ```yml
    networks:
      my-network:
        name: my-network
    ```

2. Build and run the **.Worker** service using `docker-compose`.
    ```
    docker-compose build
    docker-compose up
    ```
    - The `my-network` network is added as an external network.
    ```yml
    networks: 
      my-network:
        external: true
    ```
    - The **Worker** service will invoke the **Api** `weatherforecast` endpoint at a regular time interval, logging the output.
    - The **appsettings.json** file sets the value of `ClientOptions.ApiHostName` to `docker-networking-api`, which is the service name defined in the **.Api** `docker-compose.yml` file.
    - Run `docker network ls` to view networks and verify that `my-network` has been created.
    - Run `docker network inspect my-network` to verify that both containers belong to the same network.