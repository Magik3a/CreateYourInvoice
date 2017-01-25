namespace Services.WebClient.MultiLanguage.Contracts
{
    public interface IMultiLanguageClient
    {
        string GetTranslation(string resourceId);
    }
}