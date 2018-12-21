var request = require('request');
 
request({
  url: 'https://identity.primaverabss.com/core/connect/token',
  method: 'POST',
  auth: {
    user: '<CLIENT_ID>', // TODO : put your application client id here
    pass: '<CLIENT_SECRET>' // TODO : put your application client secret here
  },
  form: {
    'grant_type': 'client_credentials',
    'scope': 'application',
  }
}, function(err, res) {
  if (res) {
    console.log("Could not obtain acess token.");
  }
  else {
    var json = JSON.parse(res.body);
    console.log("Access Token:", json.access_token);
  }
});