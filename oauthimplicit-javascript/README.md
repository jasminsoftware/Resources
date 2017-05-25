## JASMIN Authentication - Implicit grant flow ##
The implicit grant type is used for mobile apps and web applications (i.e. applications that run in a web browser), where the client secret confidentiality is not guaranteed. The implicit grant type is also a redirection-based flow but the access token is given to the user-agent to forward to the application, so it may be exposed to the user and other applications on the user's device. Also, this flow does not authenticate the identity of the application, and relies on the redirect URI (that was registered with the service) to serve this purpose.
The implicit grant type does not support refresh tokens.

To perform this type of authentication, you must first register your application and obtain your Client ID for accessing the JASMIN API.

After obtaining the Client ID, the client must redirect the user to the authorization server submiting the following parameters:
- response_type: the response type; the value should be "token".
- client_id: the client identifier obtaining after the registration.
- redirect_uri: the redirect URI after login success. This parameter must match the registered redirect URI.
- scope: list of scopes

```javascript
function doLogin() {

    var CLIENT_ID = 'your_client_id';
    var AUTHORIZATION_ENDPOINT = 'https://identity.primaverabss.com/core/connect/authorize';
    var CALLBACK_ENDPOINT = 'your_callback_endpoint'

    try {

        var authUrl = AUTHORIZATION_ENDPOINT +
                    '?response_type=token' +
                    '&client_id=' + CLIENT_ID +
                    '&redirect_uri=' + CALLBACK_ENDPOINT +
                    '&scope=application';

        window.location.replace(authUrl);
    }
    catch (err) {
        console.log(err.message);
    }
}
```

The user will be prompt with the JASMIN login to authenticate his identity. After successful authentication, the service redirects the user-agent to the application redirect URI, and includes a URI fragment containing the access token.

The application can now extract the access token from the full redirect URI that the user-agent has retained.

```javascript
window.onload = function () {

    var data = (document.URL).split("#")[1];

    var responseParameters = (data).split("&");
    var parameterMap = [];

    for (var i = 0; i < responseParameters.length; i++) {
        parameterMap[responseParameters[i].split("=")[0]] = responseParameters[i].split("=")[1];
    }

    if (parameterMap.access_token !== undefined && parameterMap.access_token !== null && parameterMap.token_type !== null) {

        var oauth_response = {
            access_token: parameterMap.access_token,
            expires_in: parameterMap.expires_in,
            token_type: parameterMap.token_type,
            scope: parameterMap.scope
        };

        // save token

    } else {
        // Problem authenticating;
    }
}
```

This token can now be used by the application to access the user's account via the service API, limited to the scope of access, until the token expires or is revoked.
