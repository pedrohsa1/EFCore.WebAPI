using System;
using System.Net;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace ScrapingTjba
{
    class Program
    {
        static void Main(string[] args)
        {
            string capturarProcesso = "0809979-67.2015.8.05.0080";
            Console.WriteLine(scrapingProcessoTjba(capturarProcesso));
        }

        static string scrapingProcessoTjba(string numeroProcesso)
        {
            var wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
            wc.Encoding = Encoding.UTF8;

            try
            {
                Processo processo = BuscarProcesso(numeroProcesso);

                
               string pagina = wc.DownloadString("http://esaj.tjba.jus.br/cpo/sg/search.do;jsessionid=82BCEACF52467D352D912CC6929C3AAA.cposg3?paginaConsulta=1&cbPesquisa=NUMPROC&tipoNuProcesso=SAJ&numeroDigitoAnoUnificado=&foroNumeroUnificado=&dePesquisaNuUnificado=&dePesquisa=" + numeroProcesso);
               var htmlDocument = new HtmlDocument();
               htmlDocument.LoadHtml(pagina);
               HtmlNode[] nodesTabelaProcesso = htmlDocument.DocumentNode.SelectNodes("//table").ToArray();

               if (nodesTabelaProcesso != null)
               {
                    HtmlNode nodeNumeroProcesso = nodesTabelaProcesso[15].ChildNodes[1].ChildNodes[1].ChildNodes[5];
                    HtmlNode nodeClasse = nodesTabelaProcesso[16].ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[1];
                    HtmlNode nodeArea = nodesTabelaProcesso[17];
                    HtmlNode nodeAssunto = nodesTabelaProcesso[14].ChildNodes[7].ChildNodes[3];
                    HtmlNode nodeOrigem = nodesTabelaProcesso[14].ChildNodes[9].ChildNodes[3];
                    HtmlNode nodeDistribuicao = nodesTabelaProcesso[14].ChildNodes[13].ChildNodes[3];
                    HtmlNode nodeRelator = nodesTabelaProcesso[14].ChildNodes[15].ChildNodes[3];

                    processo.NumeroProcesso = nodeNumeroProcesso.InnerText.Trim();
                    processo.Classe = nodeClasse.InnerText.Trim();
                    string[] parts = nodeArea.InnerText.Split(":");
                    processo.Area = parts[1].Trim();
                    processo.Assunto = nodeAssunto.InnerText.Trim();
                    processo.Origem = nodeOrigem.InnerText.Trim();
                    processo.Distribuicao = nodeDistribuicao.InnerText.Trim();
                    processo.Relator = nodeRelator.InnerText.Trim();
                    processo.Movimentacao = new List<Movimentacao>();

                    foreach (HtmlNode nodeMovimentacao in nodesTabelaProcesso[32].ChildNodes)
                    {
                       if (nodeMovimentacao.Name == "tr")
                       {
                           Movimentacao movimentacao = new Movimentacao();
                           movimentacao.Data = Convert.ToDateTime(nodeMovimentacao.ChildNodes[1].InnerText.Trim());
                           movimentacao.Descricao = nodeMovimentacao.ChildNodes[5].InnerText.Trim();
                           processo.Movimentacao.Add(movimentacao);
                       }
                    }
                }

                string msgResponse = "";

                if (processo.Id == 0)
                {
                    if (Gravar(JsonConvert.SerializeObject(processo)))
                        msgResponse = "O Processo foi gravado com sucesso";
                    msgResponse = "Erro ao Gravar o Processo";
                }
                else
                {
                    if (Altera(processo.Id, JsonConvert.SerializeObject(processo)))
                        msgResponse = "O Processo foi gravado com sucesso";
                    msgResponse = "Erro ao Gravar o Processo";
                }

                return msgResponse;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        static bool Gravar(string jsonProcesso)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://localhost:44344/api/processo");
                request.Method = "POST";
                request.ContentType = "application/json; charset-UTF-8";

                var byteArray = Encoding.UTF8.GetBytes(jsonProcesso);
                request.ContentLength = byteArray.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();

                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        static bool Altera(int id, string jsonProcesso)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://localhost:44344/api/processo/" + id);
                request.Method = "PUT";
                request.ContentType = "application/json; charset-UTF-8";

                var byteArray = Encoding.UTF8.GetBytes(jsonProcesso);
                request.ContentLength = byteArray.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();

                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        static Processo BuscarProcesso(string numeroProcesso)
        {
            Processo processo = new Processo()
            {
                NumeroProcesso = string.Empty,
                Classe = string.Empty,
                Area = string.Empty,
                Assunto = string.Empty,
                Origem = string.Empty,
                Distribuicao = string.Empty,
                Relator = string.Empty,
                Movimentacao = new List<Movimentacao>()
            };

            try
            {
                WebRequest request = WebRequest.Create("https://localhost:44344/api/processo/filtro/" + numeroProcesso);
                request.Method = "GET";
                var response = request.GetResponse();
                
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string content = streamReader.ReadToEnd();
                        processo = JsonConvert.DeserializeObject<Processo>(content);
                    }
                    return processo;
                }

                return processo;
            }
            catch (Exception)
            {
                return processo;
            }
        }
    }

    public class Processo
    {
        public int Id { get; set; }
        public string NumeroProcesso { get; set; }
        public string Classe { get; set; }
        public string Area { get; set; }
        public string Assunto { get; set; }
        public string Origem { get; set; }
        public string Distribuicao { get; set; }
        public string Relator { get; set; }
        public List<Movimentacao> Movimentacao { get; set; }

    }

    public class Movimentacao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public Processo Processo { get; set; }
        public int ProcessoId { get; set; }
    }
}
