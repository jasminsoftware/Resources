(function () {

    'use strict';

    try {

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
    }
    catch (err) {
        console.log(err.message);
    }

}());