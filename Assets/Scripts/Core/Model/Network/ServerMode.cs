using System;

namespace Esimo
{
    public class ServerMode : IServerMode
    {
        private const string Domain = "esimo.com";
        private const string Protocol = "http";
        private const string Protocol_Secure = "https";
        private const string Port = "8888";

        public ServerEnvironment environment {get;set;}

        public ServerMode(ServerEnvironment env)
		{
			this.environment = env;
		}

        public string GetBaseUrl()
        {
            switch(environment)
			{
				case ServerEnvironment.Dev:
                    return "localhost";
                case ServerEnvironment.QA:
                    return String.Format("{0}://{1}:{2}", Protocol_Secure, Domain, GetBasePort());
				case ServerEnvironment.Production:
                    return String.Format("{0}://{1}:{2}", Protocol_Secure, Domain, GetBasePort());
			}
			return string.Empty;
        }

        public string GetBasePort()
        {
            return Port;
        }
    }
}