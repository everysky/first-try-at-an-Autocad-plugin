namespace test_Ribbon
{
    public class ComicModel
    {
        public string Url { get; set; }
        public int Num { get; set; }
        public string Alt { get; set; }                 
        public string Img { get; set; }
        public string Title { get; set; }
        public void Write(IComicSummarizer summarizer, IMessageWriter writer)
        {
            writer.WriteMessage(summarizer.GenerateSummary(this));
        }
    }
}
