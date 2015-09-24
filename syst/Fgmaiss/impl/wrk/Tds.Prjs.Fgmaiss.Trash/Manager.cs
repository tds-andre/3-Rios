using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;

namespace Tds.Prjs.Fgmaiss.Trash
{
    public class Manager
    {
        public String Read(String filename)
        {
            StringBuilder pdfText = new StringBuilder();
            PdfReader pdfReader = new PdfReader(filename);
            for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                pdfText.Append(PdfTextExtractor.GetTextFromPage(pdfReader, i));

            using (StreamWriter outfile = new StreamWriter(filename + ".txt"))
            {
                outfile.Write(pdfText.ToString());
            }


            return pdfText.ToString();
        }

        public void Execute(String filename)
        {
            List<Scan> scans = new List<Scan>();
            
            var reader = new StreamReader(filename);
            var lines = reader.ReadToEnd().Split('\n').ToList();
            Boolean atTrashArea = true;
            Boolean newGuy = false;
            Scan scan = null;

            for (int i = 0; i < lines.Count; i++)
            {
                
                if (lines[i].StartsWith("Varrição"))
                {
                    scan = new Scan();
                    var split1 = lines[i].Split('-').ToList();
                    scan.Type = split1[0].TrimEnd();
                    if(split1.Count > 1){
                        var split2 = split1[1].Split('/').ToList();
                        scan.Location = split2[0].Trim();
                        if(split2.Count > 1)
                            scan.Team = split2[1].Trim();
                    }
                    scans.Add(scan);

                }
                else if (lines[i].StartsWith("Total"))
                    continue;
                else
                {
                    var split = lines[i].Split(' ').Skip(1).ToList();
                    int trash;
                    int addressEnd = 0;
                    for (var j = 0; j < split.Count - 1; j++)
                    {
                        if (int.TryParse(split[j].Replace(".", ""), out trash) && (split[j + 1].Length == 1) && int.TryParse(split[j + 1], out trash))
                        {
                            addressEnd = j;
                            break;
                        }
                    }
                    if(addressEnd==0)
                        throw new Exception("Erro no parse");

                    var route = new Route(); 
                    route.Address = split.Take(addressEnd).Aggregate((workingSentence, next) => workingSentence + " " + next);
                    route.Meters = Int32.Parse(split[addressEnd].Replace(".",""));
                    route.DailyFrequncy = Byte.Parse(split[addressEnd+1]);
                    var final = split.Skip(addressEnd + 2).ToList();
                    route.WeekRange = final.Take(final.Count - 2).Aggregate((workingSentence, next) => workingSentence + " " + next);
                    route.MontlyFrenquency = Int32.Parse(final[final.Count-2]);
                    route.TotalMeters = Int32.Parse(final[final.Count - 1].Replace(".", ""));
                    scan.Routes.Add(route);


                }

                using (StreamWriter outfile = new StreamWriter(filename + ".csv"))
                {
                    outfile.WriteLine(Scan.CsvHeaders() + Route.CsvHeaders());
                    scans.ForEach(el => el.Routes.ForEach(route => outfile.WriteLine(el.ToCsvLine() + route.ToCsvLine())));

                }

               
            }
        }
    }
}
