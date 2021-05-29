namespace Pokedex.FunTranslationsClient.Models
{
    public class TranslationResponse
    {
        public SuccessResponse Success { get; set; }
        public ContentsResponse Contents { get; set; }
    }

    public class SuccessResponse
    {
        public int Total { get; set; }
    }

    public class ContentsResponse
    {
        public string Translated { get; set; }
        public string Text { get; set; }
        public string Translation { get; set; }
    }
}
