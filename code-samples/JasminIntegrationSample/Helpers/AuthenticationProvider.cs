using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Jasmin.IntegrationSample
{
    internal class AuthenticationProvider
    {
        #region Members

        private string clientId;
        private string clientSecret;

        private TokenClient tokenClient;

        private string accessToken;
        private DateTime tokenExpirationDate;

        #endregion

        #region Constructors

        public AuthenticationProvider(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        #endregion

        #region Protected Properties

        protected TokenClient TokenClient
        {
            get
            {
                if (this.tokenClient == null)
                {
                    string getTokenAddress = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Constants.Identity.BaseUriKey, Constants.Identity.TokenUriKey);

                    Console.WriteLine("");
                    Console.WriteLine("Start by requesting an Access Token");
                    Console.WriteLine("");
                    Console.WriteLine("Request - GET");
                    Console.WriteLine("{0}", getTokenAddress);


                    this.tokenClient = new TokenClient(getTokenAddress, this.clientId, this.clientSecret, style: AuthenticationStyle.PostValues);
                }

                return this.tokenClient;
            }
        }

        #endregion

        #region Public Methods

        public async Task SetAccessTokenAsync(HttpClient client)
        {
            if (string.IsNullOrEmpty(this.accessToken) || this.tokenExpirationDate <= DateTime.Now)
            {
                await this.RequestAccessTokenAsync();
            }

            client.SetBearerToken(this.accessToken);
        }

        public async Task RequestAccessTokenAsync()
        {
            TokenResponse tokenResponse = await this.TokenClient.RequestClientCredentialsAsync(Constants.Identity.ApplicationScopes);
            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            this.accessToken = tokenResponse.AccessToken;
            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                throw new Exception("Failed to obtain the JASMIN API access token.");
            }

            this.tokenExpirationDate = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);
        }

        #endregion
    }
}
