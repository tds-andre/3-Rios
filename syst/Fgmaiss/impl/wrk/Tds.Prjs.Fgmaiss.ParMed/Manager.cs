using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;


namespace Tds.Prjs.Fgmaiss.ParMed
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

        public IEnumerable<String> ExtractRows(String text)
        {

            return text.Split('\n')
                .Where(el => el[0] == ' ')
                .Where(el => !el.Split(' ').Where(el2 => (el2.Length > 1)).ToList()[0].StartsWith("FPRE"));

        }

        public void Execute(String filename)
        {
            var reader = new StreamReader(filename);
            var lines = reader.ReadToEnd().Split('\n').ToList();
            while (lines.Contains("Entradas e Saídas"))
            {
                var o = lines.IndexOf("Entradas e Saídas");
                var f = lines.IndexOf("N° Tipo de Movimentação Qtde. Valor");
                f = lines.IndexOf("N° Tipo de Movimentação Qtde. Valor", f + 1);
                lines.RemoveRange(o, f - o + 1);
                //lines.Insert(o, "####");
            }
            List<String> lines2 = new List<String>();
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("Produto: Unidade:"))
                {
                    lines2.Add(lines[i - 2] + " " + lines[i - 1] + " " + lines[i]);
                    lines2.RemoveAt(lines2.Count - 2);
                    lines2.RemoveAt(lines2.Count - 2);
                }
                else
                    lines2.Add(lines[i]);
            }

            var rawItems = lines2.Where(el => el.Contains("Produto: Unidade:")).Distinct();
            
            var items = rawItems.Select(el => new Item(el)).ToList();
            int ff = 0;
            int oo = 0;
            for (var i = 0; i < items.Count - 1; i++)
            {
                if(i==0)
                    oo= lines2.IndexOf(items[i].Raw) + 1;
                else
                    oo = ff;
                
                ff = lines2.IndexOf(items[i + 1].Raw, oo + 1);
                items[i].ParseTransactions(lines2.GetRange(oo, ff - oo));
            }

            using (StreamWriter outfile = new StreamWriter(filename + ".csv"))
            {
                items.ForEach(item => item.Transactions.ForEach( trans => outfile.WriteLine(trans.ToCsvLine()) ));
               
            }
            //Read(filename);
            /*var ss = ExtractRows(Read(filename)).ToList();
            var result = ss.Select(el => new Log(el)).ToList();
            using (StreamWriter outfile = new StreamWriter(filename + ".csv"))
            {
                //outfile.Write(pdfText.ToCsvLine());
                result.ForEach(el => outfile.WriteLine(el.ToCsvLine()));
            }

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Item>));
            System.IO.FileStream file = System.IO.File.Create(filename+".xml");

            writer.Serialize(file, items);
            file.Close();
             */
        }
    }
}
