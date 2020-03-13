namespace Jasmin.IntegrationSample
{
    public class SessionContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionContext"/> class.
        /// </summary>
        public SessionContext()
        {
            ClientId = string.Empty;
            ClientSecret = string.Empty;
            AccountKey = string.Empty;
            SubscriptionKey = string.Empty;
            CompanyKey = string.Empty;
            CultureKey = Constants.DefaultCulture;
            OrderResource = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the company key.
        /// </summary>
        public string CompanyKey { get; set; }

        /// <summary>
        /// Gets or sets the account key.
        /// </summary>
        public string AccountKey { get; set; }

        /// <summary>
        /// Gets or sets the subscription key.
        /// </summary>
        public string SubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets the culture key.
        /// </summary>
        public string CultureKey { get; set; }

        /// <summary>
        /// Gets or sets the order resource.
        /// </summary>
        public OrderResource OrderResource { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns true if initial arguments are set.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid()
        {
            return  !string.IsNullOrEmpty(ClientId) &&
                    !string.IsNullOrEmpty(ClientSecret) &&
                    !string.IsNullOrEmpty(AccountKey) &&
                    !string.IsNullOrEmpty(SubscriptionKey);
        }

        /// <summary>
        /// Determines whether session has defined a compay.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if has compay; otherwise, <c>false</c>.
        /// </returns>
        public bool HasCompay()
        {
            return !string.IsNullOrEmpty(CompanyKey);
        }

        #endregion Public Methods
    }
}
