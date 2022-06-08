using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace test_Ribbon
{
    public class ComicGetter
    {
        //public static int MaxComicNumber { get; set; }
        public async Task<ComicModel> LoadComic(int comicNumber = 0)
        {
            string url = "";

            if (comicNumber > 0)
            {
                url = $"https://xkcd.com/{comicNumber}/info.0.json";

            }
            else
            {
                url = "https://xkcd.com/info.0.json";
            }
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if(response.IsSuccessStatusCode)
                {
                    ComicModel comic = await response.Content.ReadAsAsync<ComicModel>();

                    //if (comicNumber == 0)
                    //{
                    //    MaxComicNumber = comic.num;
                    //}

                    comic.Url = url;

                    return comic;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase); 
                }
            }
        }
    }
}
