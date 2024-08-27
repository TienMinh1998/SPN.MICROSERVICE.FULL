##  THE PROJECT APPY THE NEW TECHNOLOGIES
- Ocelot getway
- microservice
- docker
- Enitity Framework
- Dapper ORM
## HOW TO RUN USING DOCKER
- in dotnet 8 default port is 8080
-  docker build -t <image_name> -f Dockerfile .
-  docker run -d -p 8888:80 --name <container_name> <image_name>
   vd : docker run -d -p 8888:8080 container_name image_name
   // run with default port NET 8.0
- docker run -d -p 2009:8080 hola_api
