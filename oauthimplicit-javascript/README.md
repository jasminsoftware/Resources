## JASMIN Authentication - Implicit flow grant ##
The implicit grant type is used for applications and web applications where the client secret confidentiality is not guaranteed.

The client will redirect the user to the authorization server with the following parameters:
- response_type -> the value token
- client_id -> the client identifier
- redirect_uri -> the redirect URI after login success. This parameter is optional, but if not sent the user will be redirected to a pre-registered redirect URI.
- scope -> list of scopes
