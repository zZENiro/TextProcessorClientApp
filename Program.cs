using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.Json;

namespace App
{
    partial class Program
    {
        static HttpClient _client = new HttpClient();
        static int PORT;
        static string HOST;
        static string _url;
        static IConfiguration _configuration;

        static StringBuilder line = new StringBuilder();

        static void Main(string[] args)
        {
            _configuration =
                new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddCommandLine(args)
                .Build();

            PORT = int.Parse(_configuration["port"]);
            HOST = _configuration["host"];
            _url = $"http://{HOST}:{PORT}/";

            OutputInfo(_url);
            System.Console.Write("Введите слово: ");
            NextLine();

            while(true)
            {
                line.Append(ReadLineUTF());

                if (ChooseCommandAsync(line.ToString()).Result == "exit")
                    break;

                line.Clear();
            }
        }

        static async Task<string> GetRelativeWordsAsync(string url, string word)
        {
            byte[] data = null;
            data = Encoding.UTF8.GetBytes(word); 
            StringBuilder datastr = new StringBuilder();
            
            for (int i = 0; i < data.Length; ++i)
                datastr.Append(data[i] + " ");

            Uri requestUri = new Uri(url);
            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                msg.Headers.Add("accept", "text/html");
                msg.Headers.Add("accept-language", "*");
                msg.Headers.Add("queryword", datastr.ToString());
                
                using (HttpResponseMessage resp = await _client.SendAsync(msg))
                {
                    using (HttpContent content = resp.Content)
                    {
                        return await content.ReadAsStringAsync();
                    }
                }
            }
        }

    }
}
