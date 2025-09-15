# Template Customer Management App

## Instructions how to run app
### Docker desktop
- First install Docker desktop.
- Find compose.yaml file in application folder.
- Open terminal or cmd on same location as compose.yaml file.
- Run command **docker-compose up --build**, it should build and run in docker desktop new volume with containers and start sql server first, then start Apis and seed data to the databases, also build and start Gateway.

### Authorization JWT
- For usage of Apis and Gateway you need to create a JWT token. To create a token you need to go and use Auth Api in browser enter: http://localhost:5003/scalar/v1 and use **/login endpoint** to get token. It will be valid for **1 hour**.
- With this token you can get data straight from **Customer Management Api** and **Template Management Api** which IP's is below. There enter **access_token** in Bearer Token place and use all endpoints with required parameters in documentation.

### Postman
- Add **access_token** from Auth Api to Authorization section as Bearer Token in Postman. Now you are ready to use all endpoints.
- Use of Gateway is a little bit trickier it is without open Api documentation, because of **Ocelot**. You can use postman instead.
- Use **GET** request with IP http://localhost:5000/message/{customerId}/{templateId} e.g. http://localhost:5000/message/1/1 and you will send message through gateway to both Apis it will aggregate your requested customer data with template and send message (write to console). You can check that in gateway docker container if it was printed.

#### Gateway
- GET 
	- customer/{customerId}
	- template/{templateId}
	- message/{customerId}/{templateId}
- POST, PATCH, DELETE 
	- customer/{action}
	- template/{action}

## IPs

Project 					| IP
---------------------------	| --------------------------------
**Gateway**					| http://localhost:5000
**Customer Management Api** | http://localhost:5001/scalar/v1
**Template Management Api** | http://localhost:5002/scalar/v1
**Auth Api**				| http://localhost:5003/scalar/v1


