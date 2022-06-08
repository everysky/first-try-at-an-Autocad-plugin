namespace test_Ribbon /* this code came from project test_parcels */
{
    public class ComicSummarizer : IComicSummarizer
    {
        public string GenerateSummary(ComicModel comic)
        {
            var message = $"Comic n°{comic.Num}";
            message += $"\n\"{comic.Title}\"";
            message += "alternative title:";
            message += $"\n\"{comic.Alt}\"";
            message += "To see the comic:";
            message += $"\n{comic.Url}";

            return message;
        }
    }
}
