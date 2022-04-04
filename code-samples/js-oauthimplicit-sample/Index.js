
function doLogin() {

    var CLIENT_ID = 'your_client_id';
    var AUTHORIZATION_ENDPOINT = 'https://identity.primaverabss.com/connect/authorize';
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
