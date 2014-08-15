using System;

namespace Esimo
{
    public enum ServerEnvironment
    {
        Dev,
        QA,
        Production
    }

    public interface IServerMode
    {
        string GetBaseUrl();

        string GetBasePort();
    }
}
