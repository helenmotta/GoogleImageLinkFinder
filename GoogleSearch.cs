using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

public class GoogleSearch
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Digite sua pesquisa: ");
        string? search = Console.ReadLine();

        if (String.IsNullOrEmpty(search))
        {
            Console.Write("\n * Por favor, insira uma pesquisa. \n");
            await Main(args);
            return;
        }

        string encodedQuery = Uri.EscapeDataString(search);
        string searchUrl = $"https://www.google.com/search?q={encodedQuery}&tbm=isch&site=imghp";

        GoogleSearch googleSearch = new GoogleSearch();
        string searchResult = await googleSearch.ParseHtml(searchUrl);

        Console.WriteLine(searchResult);

    }

    public async Task<string> ParseHtml(string link)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = new HtmlDocument();

        try
        {
            doc = await web.LoadFromWebAsync(link);

            string hrefContent = doc.DocumentNode.SelectNodes("//a").Where(a => a.InnerHtml.Contains("<img")).Select(b => b.Attributes["href"].Value).FirstOrDefault();
            Regex regex = new Regex(@"imgurl=([^&]+)");
            string imageUrl = regex.Match(hrefContent).Groups[1].Value;

            return imageUrl;
        }
        catch
        {
            string errorMsg = "Erro ao consultar imagem";
            return errorMsg;
        }
    }
}