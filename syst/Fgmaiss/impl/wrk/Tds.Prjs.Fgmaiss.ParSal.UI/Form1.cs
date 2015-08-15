using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Tds.Prjs.Fgmaiss.ParSal.Models;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Threading;
using Tds.Prjs.Fgmaiss.ParSal;
using Tds.Prjs.Fgmaiss.ParSal.Crawlers;



namespace Tds.Prjs.Fgmaiss.ParSal.UI
{
    public partial class Form1 : Form
    {
        String Cookie = "24mj78ndaj8eitapl9qq5cp2p2";
        Manager Manager = new Manager("c:/parsal/");

        public Form1()
        {
            InitializeComponent();
        }

        private String GetBetween(String text, String start, String end, int index)
        {
            int startIndex = text.IndexOf(start, index) + start.Length;
            int endIndex = text.IndexOf(end, startIndex);
            return text.Substring(startIndex, endIndex - startIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //faz requisição http

            var currentPage = 0;
            
            var f = File.CreateText("c:\\m\\cs.csv");

            while (true) {
                var CivilServants = new List<CivilServant>();
                currentPage++;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www1.fgmaiss.com.br/contabil/cgi-bin/pesqgrid.php");
                request.Host = "www1.fgmaiss.com.br";
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Cookie("8jpo2jrlp3005q3lj3qsbf5hq7PESQGRIDfindGrid", "") { Domain = request.Host });
                request.CookieContainer.Add(new Cookie("PHPSESSID", Cookie) { Domain = request.Host });
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                var postData = "";
                if(currentPage == 1)
                    postData = "findGrid=&idpagex=1&lv=find&lm=0&tc=0&mt=0&obj=X3h4ZnVu&qrw=U0VMRUNUICBmdW5jb2RpZ28sIGZ1bm5vbWUgfHwgJyAtIE1BVFJJQ1VMQTogJyB8fCBmdW5tYXRyaWMgfHwgJyAtIENQRjogJyB8fCBmdW5jcGYgfHwgJyAtIERULkFETTogJyB8fCBUT19DSEFSKGZ1bmR0YWRtLCdkZC9NTS95eXl5JykgfHwgQ0FTRSBXSEVOIE5PVCBudWwoZnVuZHRkZW0pIFRIRU4gJyAtIERULkRFTTogJyB8fCBUT19DSEFSKG52bChmdW5kdGRlbSwnMDAwMS0wMS0wMScpLCdkZC9NTS95eXl5JykgRUxTRSAnJyBFTkQgRlJPTSBjdGJmbGguZmxoZnVuIFdIRVJFIHByZmNvZGlnbyA9ICcyOS4xMzguMzc3LzAwMDEtOTMnIEFORCBldGRjb2RpZ28gPSAnMDEnIEFORCBmdW5jb2RpZ298fGZ1bm5vbWUgfHwgJyAtIE1BVFJJQ1VMQTogJyB8fCBmdW5tYXRyaWMgfHwgJyAtIENQRjogJyB8fCBmdW5jcGYgfHwgJyAtIERULkFETTogJyB8fCBUT19DSEFSKGZ1bmR0YWRtLCdkZC9NTS95eXl5JykgfHwgQ0FTRSBXSEVOIE5PVCBudWwoZnVuZHRkZW0pIFRIRU4gJyAtIERULkRFTTogJyB8fCBUT19DSEFSKG52bChmdW5kdGRlbSwnMDAwMS0wMS0wMScpLCdkZC9NTS95eXl5JykgRUxTRSAnJyBFTkQgTElLRSAnJSUnIE9SREVSIEJZIGZ1bm5vbWUsIGZ1bm1hdHJpYw%3D%3D&tit=PESQUISA+DE+SERVIDORES&IDDFGCTBETD=01&IDDFGCTBEXE=2015&IDDPWEBCNPJ=29.138.377%2F0001-93&IDDPWEBMOD=&IDDPWEBFINMOD=&seta=%3C%3C";
                else
                    postData = "findGrid=&idpagex="+(currentPage-1).ToString()+"&lv=find&lm="+(20*(currentPage-2)).ToString()+"&tc=0&mt=0&obj=X3h4ZnVu&qrw=U0VMRUNUICBmdW5jb2RpZ28sIGZ1bm5vbWUgfHwgJyAtIE1BVFJJQ1VMQTogJyB8fCBmdW5tYXRyaWMgfHwgJyAtIENQRjogJyB8fCBmdW5jcGYgfHwgJyAtIERULkFETTogJyB8fCBUT19DSEFSKGZ1bmR0YWRtLCdkZC9NTS95eXl5JykgfHwgQ0FTRSBXSEVOIE5PVCBudWwoZnVuZHRkZW0pIFRIRU4gJyAtIERULkRFTTogJyB8fCBUT19DSEFSKG52bChmdW5kdGRlbSwnMDAwMS0wMS0wMScpLCdkZC9NTS95eXl5JykgRUxTRSAnJyBFTkQgRlJPTSBjdGJmbGguZmxoZnVuIFdIRVJFIHByZmNvZGlnbyA9ICcyOS4xMzguMzc3LzAwMDEtOTMnIEFORCBldGRjb2RpZ28gPSAnMDEnIEFORCBmdW5jb2RpZ298fGZ1bm5vbWUgfHwgJyAtIE1BVFJJQ1VMQTogJyB8fCBmdW5tYXRyaWMgfHwgJyAtIENQRjogJyB8fCBmdW5jcGYgfHwgJyAtIERULkFETTogJyB8fCBUT19DSEFSKGZ1bmR0YWRtLCdkZC9NTS95eXl5JykgfHwgQ0FTRSBXSEVOIE5PVCBudWwoZnVuZHRkZW0pIFRIRU4gJyAtIERULkRFTTogJyB8fCBUT19DSEFSKG52bChmdW5kdGRlbSwnMDAwMS0wMS0wMScpLCdkZC9NTS95eXl5JykgRUxTRSAnJyBFTkQgTElLRSAnJSUnIE9SREVSIEJZIGZ1bm5vbWUsIGZ1bm1hdHJpYw%3D%3D&tit=PESQUISA+DE+SERVIDORES&IDDFGCTBETD=01&IDDFGCTBEXE=2015&IDDPWEBCNPJ=29.138.377%2F0001-93&IDDPWEBMOD=&IDDPWEBFINMOD=&seta=%3E%3E";

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }


                var response = request.GetResponse();

                //copia conteúdo da responsta http para uma string
                var responseStream = response.GetResponseStream();
                var memStream = new MemoryStream();
                responseStream.CopyTo(memStream);
                var responseText = System.Text.Encoding.Default.GetString(memStream.ToArray());

                var currentIndex = responseText.IndexOf("<tr class=ld_tr");
                if (currentIndex == -1)
                    break;                
                
                while (currentIndex > 0)
                {
                    var currentCS = new CivilServant();
                    var s = GetBetween(responseText, ">", "</tr>", currentIndex);
                    currentCS.ID = Int64.Parse(GetBetween(s, "','", "','", s.IndexOf("xxfun")) );
                    currentCS.Name =  GetBetween(s, currentCS.ID.ToString() + "','", " -", 0);
                    currentCS.Registration = Int64.Parse(GetBetween(s, "MATRICULA: ", " -", 0));
                    currentCS.CPF = Int64.Parse(GetBetween(s, "CPF: ", " - ", 0).Replace(".", "").Replace("-", ""));
                    currentCS.RecruitedAt = DateTime.Parse(s.Substring(s.IndexOf(".ADM: ") + 6, 10), new System.Globalization.CultureInfo("fr-FR", true));
                    if(s.IndexOf("DT.DEM") > 0)
                        currentCS.DismissedAt = DateTime.Parse(s.Substring(s.IndexOf(".DEM: ") + 6, 10), new System.Globalization.CultureInfo("fr-FR", true));
                    CivilServants.Add(currentCS);
                    currentIndex = responseText.IndexOf("<tr class=ld_tr", currentIndex + 10);
                
                
                }
                CivilServants.ForEach(el => f.WriteLine(el.ToCsv()));
                f.Flush();
            }

            
           
            f.Close();
        }

       

      

        

        private void button2_Click(object sender, EventArgs e)
        {
            FillServantEarning.Cookie = Cookie;
            Manager.Start();
         
            
               
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(List<CivilServant>));
            System.IO.StreamReader file = new System.IO.StreamReader(@"c:\parsal\SerializationOverview.xml");
            List<CivilServant> overview = new List<CivilServant>();
            overview = (List<CivilServant>)reader.Deserialize(file);

            var f = File.CreateText("c:/parsal/out.csv");
            
            overview.ForEach(
                cs => cs.Earnings.ForEach(
                    er =>
                        {
                            for(var meI = 0; meI < 12 ; meI++){
                                var me = er.MonthEarning[meI];
                                f.WriteLine(cs.ToCsv() +";" + (meI+1).ToString() +"/" + er.Year.ToString()  +";" + me.ToString());
                            }
                        }                        
                    
                )
            );
            f.Close();

            //Console.WriteLine(overview.title);

        }
    }
}
