﻿namespace TCC.DigitalID.Services.Config
{
    public class AuthenticationService : IAuthenticationService
    {
        public Configurations _configuration;
        private readonly NafathConfigurations _nafathConfigurations;

        public AuthenticationService(IOptions<NafathConfigurations> nafathConfigurations, IOptions<Configurations> configuration)
        {
            _configuration = configuration.Value;
            _nafathConfigurations = nafathConfigurations.Value;
        }
        /// <summary>
        /// Basic authentication 
        /// </summary>
        /// <param name="authHeader"></param>
        /// <returns></returns>
        public bool IsValidUser(AuthenticationHeaderValue authHeader)
        {

            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));

            string[] userArray = decodedToken.Split(new[] { ':' }, 2);
            string UserName = userArray[0];
            string Password = userArray[1];

            if ((_configuration.ApiUser == UserName) && (_configuration.ApiPass == Password))
                return true;
            else
                return false; // INVALID_USER

        }

        /// <summary>
        /// ApiKey
        /// </summary>
        /// <param name="authHeader"></param>
        /// <returns></returns>
        public bool IsValidApiKey(AuthenticationHeaderValue authHeader)
        {

            if ((_configuration.ApiScheme == authHeader.Scheme) && (_configuration.ApiKey == authHeader.Parameter))
                return true;
            else
                return false; // INVALID_USER

        }

        /// <summary>
        /// ApiKey
        /// </summary>
        /// <param name="authHeader"></param>
        /// <returns></returns>
        public bool IsValidNafathApiKey(AuthenticationHeaderValue authHeader)
        {

            if ((_nafathConfigurations.ApiScheme == authHeader.Scheme) && (_nafathConfigurations.ApiKey == authHeader.Parameter))
                return true;
            else
                return false; // INVALID_USER

        }
    }
}