### Jasmin Code Sample - OAuth Client Credentials - Javascript - Node.js

This code sample illustrates how the OAuth 2.0 client credentials grant type can be used with the Jasmin Web API and Node.js.

To run this sample you need to do 4 simple steps:

##### 1. Install Node.js for your platform (MacOS, Windows or Linux)
Just smash nodejs.org in your browsers address bar and you should be good to go.

##### 2. Edit app.js and set CLIENT_ID and CLIENT_SECRET with your values
```javascript  
  auth: {
    user: '<CLIENT_ID>', // TODO : put your application client id here
    pass: '<CLIENT_SECRET>' // TODO : put your application client secret here
  },
```

##### 3.	Initialize the project (install all dependencies)
```
npm install
```

##### 4. Run the app
```
node app.js
```
