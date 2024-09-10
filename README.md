# Paynet Challenge

This repository contains two sub-projects: **Paynet-Challenge-API** and **Paynet-Challenge-App**. It is a code challenge developed for an interview, showcasing the integration of a backend API and a frontend application.

- **paynet-challenge-api**: The backend, built with .NET 8/MongoDb.
- **paynet-Challenge-app**: The frontend, built with Angular 17.

## Project Description

The **Paynet Challenge** is a simple web application. It includes user management and authentication handle.

- **paynet-challenge-api**: Exposes RESTful APIs for handling authentication (JWT) and user management.
- **paynet-challenge-app**: Provides a responsive user interface that interacts with the API for handling user data and auth.

### Default user 

The API has a `DbInizializer` that always create a user with email: **admin@paynet.com.br** and password: **vowedida** only for test purposes.

## Installation via Docker

To simplify the setup, you can run both the API and App using Docker.

### Prerequisites

- Docker (Ensure Docker is installed and running on your machine)

### Steps

1. Clone the repository:
    ```bash
    git clone https://github.com/kelvi-ribeiro/paynet-challenge
    cd paynet-challenge
    cd docker
    ```

2. Build and start the services using Docker Compose:
    ```bash
    docker-compose up --build
    ```
    
## Installation Locally

To run both projects locally, follow the instructions below.

### Prerequisites

- .NET 8.0
- npm (version 6 or later)
- Angular CLI (version 17 or later)
- MongoDB (Ensure MongoDB is running locally or use Docker for MongoDB)

### paynet-challenge-api setup

1. Clone the repository:
    ```bash
    git clone https://github.com/kelvi-ribeiro/paynet-challenge
    ```

2. Start the API server:
    ```bash
    cd paynet-challenge/paynet-challenge-api/src/Paynet.Challenge.Api
    dotnet run
    ```

 The API will run on `localhost:5243`.

### paynet-challenge-app setup

1. Navigate to the frontend directory:
    ```bash
    cd ../paynet-challenge-app
    ```

2. Install dependencies:
    ```bash
    npm install
    ```

3. Run the Angular development server:
    ```bash
    npm start
    ```

   The application will be available at `http://localhost:4200`.

   This will build the backend and frontend, and start the services:
   - API: `http://localhost:5243`
   - App: `http://localhost:80`

### Stopping the services:
To stop the Docker containers, run:
```bash
docker-compose down
```

## Deployed Application on AWS EC2

The Paynet Challenge has been deployed to an AWS EC2 instance for testing purposes.

Frontend URL: http://18.213.95.77
API URL: http://18.213.95.77:5243
