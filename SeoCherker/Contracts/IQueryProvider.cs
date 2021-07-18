namespace OlegChibikov.SympliInterview.SeoChecker.Contracts
{
    public interface IQueryProvider
    {
        string GetRelativeUriPart(string requestKeywords);
    }
}