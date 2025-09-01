# Docker Build and Run Instructions

1. Build the Docker image: docker build -t user-service .
2. Run the container: docker run -p 8080:80 user-service
3. The application will be available at:
- HTTP: http://localhost:8080
- Swagger UI: http://localhost:8080/swagger