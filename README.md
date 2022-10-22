# Chatiz - Chat Application
<p align="center">
  <img 
    width="500"
    height="393"
    src="https://user-images.githubusercontent.com/92651125/197267879-3c5e0b83-5f6a-4a4c-a414-80e77f2bb319.png"
  >
</p>

## Description
### ***For screenshots - skip to the bottom of the page.***  


Chatiz is a chat application that allows you to communicate with people from all over the world.  
From a more professional point of view, **Chatiz is a chat application for web browsers and Android devices, connected to a back-end RESTful server with a DB connection**: React.JS usage for the web application, Server side in ASP.NET core (C#) with a MariaDB connection (local database), and Android side in Java.  
This application was created as part of the course "Advanced Programming 2" and inspired by the application "Whatsapp Web". In the code we can find various techniques like ORM, MVC, Entity framework, web services and more.  

## Part 2: Server
The project is divided into 3 parts:

 1. Browser Side in React.JS. [link](https://github.com/ido106/Chatiz_Browser)
 2. **Server Side** in ASP.NET core (C#) with a MariaDB connection. [link](https://github.com/ido106/Chatiz_Server)
 3. Android side in Java. [link](https://github.com/ido106/Chatiz_Android)  

As marked, in this part I will show the **server** side.  
Compared to the first part, we will have to ask the server for the data (users, messages, contacts, etc.) in the client side (browser), therefore we will have to change the hard-coded requests to **server requests**. At the same time, the design of the browser has not changed. Thus, we will take the client side from [part 1 repository](https://github.com/ido106/Chatiz_Browser), under the branch '*develop*'. The server side is stored in this repository.

The server side implements a **[RESTful api](https://en.wikipedia.org/wiki/Representational_state_transfer)** in ASP.NET core MVC form.  
It also works with ***web services*** in order to break the dependency between the way we save the data and the code. In this project I decided to save the data in a database called **[MariaDB](https://mariadb.org/)** (mysql) and you will have to install it in your computer.  
Another thing that I did in order to reduce dependencies is **breaking the project into sub-projects**, so that each sub-project is only responsible for its own part.  
There are also **verifications and security** for taking data from the server, which you can take only after **jwt** verification.

The **list of requests** from the server includes receiving, sending, updating and deleting new users, contacts, messages, and more. **All information will be saved in a database**, which means that even after we disconnect from the browser the data will be saved and restored after reconnecting.  
To test the server requests, you can run the server and use the **Swagger** to perform simulations of sending requests to the server (in JSON form) which will also update the database.  
In order for the requests to work, first you need to create and connect to a new user to pass the verification. Copy the returned token and put it in the appropriate place on the top right of the Swagger page.  


## Instruction Manual

### Downloading The Server
 1. Download [MariaDB](https://mariadb.org/download/?t=mariadb&p=mariadb&r=10.11.0&os=windows&cpu=x86_64&pkg=msi&m=truenetwork) and remember the password you choose.
 2. Right click on a new folder, then open the terminal.
 3. In the terminal, enter `git clone https://github.com/ido106/Chatiz_Server.git`.
 4. Open the `WebApp.sln`file.
 5. Under Solution Explorer, choose Repository -> WebAppContext.cs, then change the password in the `connection string`to your MariaDB password.
 6. Go to Tools -> NuGet Package manager -> Package Managaer Console.
 7. Enter `Update-Database;`.
 8. Run the WebApi.
 9. Enjoy !

### Using The Browser Chat
You can also use the browser chat which is synchronized with the server.

 1. Download and install [Node.JS](https://nodejs.org/en/download/) .
 2. Right click on a new folder, then open the terminal.
 3. Download npm: enter `npm install -g npm` in the terminal.
 
 To run the code:
 1. In the same folder that you opened, enter `git clone --branch Develop https://github.com/ido106/Chatiz_Browser.git` in the terminal.
 2. Enter the folder "Advanced2\web_app".
 3. Open the terminal, and enter `npm install react-scripts`.
 4. Enter `npm start`.
 5. Enjoy !

## Screenshots
*For screenshots of the client side visit the [first part](https://github.com/ido106/Chatiz_Browser).*

### Swagger Requests List
<p align="center">
  <img 
    width="900"
    src="https://user-images.githubusercontent.com/92651125/197361683-ce7c4aaf-16c1-4fef-8172-5a164a23a197.png"
  >
</p>
  
  <br/><br/>
<p align="center">
  <img 
    width="900"
    src="https://user-images.githubusercontent.com/92651125/197361775-7cdbe72b-af22-469a-9798-7296e0e85cdd.png"
  >
</p>



## **Enjoy	:smile:**
