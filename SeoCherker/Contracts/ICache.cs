using System;

namespace OlegChibikov.SympliInterview.SeoChecker.Contracts
{
    /// <summary>
    /// Generic cache interface. Any real cache can be converted to it using the Adapter pattern.
    /// </summary>
    public interface ICache
    {
        TItem? Get<TItem>(string key);

        void Set<TItem>(string key, TItem value, TimeSpan absoluteExpirationRelativeToNow);
    }
}
