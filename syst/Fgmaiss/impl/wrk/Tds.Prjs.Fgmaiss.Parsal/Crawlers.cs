using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Prjs.Fgmaiss.ParSal.Models;
using System.Net;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.IO;


namespace Tds.Prjs.Fgmaiss.ParSal.Crawlers
{
    public class FillServantEarning
    {
        public static String Cookie = "";


       

        public CivilServant Execute(CivilServant servant) {
            var beginYear = servant.RecruitedAt.Year;
            var endYear = servant.DismissedAt.HasValue ? servant.DismissedAt.Value.Year : 2015;

            for (var year = beginYear; year <= endYear; year++)
            {
                var s = GetPdfText(year.ToString(), servant.FormatID());
                var indO = s.IndexOf("TOTAL DE VENCIMENTOS ") + "TOTAL DE VENCIMENTOS ".Length;
                if (indO > 0)
                {
                    var indF = s.IndexOf("\n", indO);
                    var vencs = s.Substring(indO, indF - indO);
                    var earning = new YearEarning();
                    earning.Year = year;
                    earning.MonthEarning = vencs.Split(' ').Take(12).Select(el => Double.Parse(el)).ToList();
                    servant.Earnings.Add(earning);
                }
            }
            servant.Earnings.OrderBy(el => el.Year);
            return servant;
        }


        private String GetPdfText(String year, String servantID)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www1.fgmaiss.com.br/contabil/relflh/flhrel037pdf.php?prc=MjkuMTM4LjM3Ny8wMDAxLTkz&etd=MDE=&ser=" + servantID + "&ano=" + year + "&fv1=300&fv2=000&fv3=000&fv4=000&fv5=000&fv00=500&fv01=186&fv02=008&fv03=012&fv04=071&fv05=007&fv06=073&fv07=189&fv08=406&fv09=039&fv10=041&fv11=000&fv12=000&fv13=000&fv14=000&fv15=000");
            request.Host = "www1.fgmaiss.com.br";
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie("8jpo2jrlp3005q3lj3qsbf5hq7PESQGRIDfindGrid", "") { Domain = request.Host });
            request.CookieContainer.Add(new Cookie("PHPSESSID", Cookie) { Domain = request.Host });
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };


            //salva conteúdo da resposta (pdf) http em um arquivo            
            var responseStream = request.GetResponse().GetResponseStream();
            String pdfFilePath = "c:/parsal/" + servantID +  year.ToString() + ".pdf";
            var pdfOut = File.Create(pdfFilePath);
            responseStream.CopyTo(pdfOut);
            pdfOut.Close();

            //resultado
            StringBuilder pdfText = new StringBuilder();
            PdfReader pdfReader = new PdfReader(pdfFilePath);
            for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                pdfText.Append(PdfTextExtractor.GetTextFromPage(pdfReader, i));

            return pdfText.ToString();
        }
    }
}
