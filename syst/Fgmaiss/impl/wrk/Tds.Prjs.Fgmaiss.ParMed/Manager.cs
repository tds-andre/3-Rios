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
            while (lines.Contains("\nEntradas e Saídas\n"))
            {
                var o = lines.IndexOf("\nEntradas e Saídas\n");
                var f = lines.IndexOf("\nN° Tipo de Movimentação Qtde. Valor\n");
                lines.RemoveRange(o, f - o + 1);
                lines.Insert(o, "####");
            }

            var rawItems = lines.Where(el => el.Contains("Produto: Unidade:")).Distinct();            
            var items = rawItems.Select(el => new Item(el)).ToList();
            for (var i = 0; i < items.Count - 1; i++)
            {
                var o = lines.IndexOf(items[i].Raw) + 1;
                var f = lines.IndexOf(items[i + 1].Raw);
                items[i].ParseTransactions(lines.GetRange(o, f - o));
            }

            //Read(filename);
            /*var ss = ExtractRows(Read(filename)).ToList();
            var result = ss.Select(el => new Log(el)).ToList();
            using (StreamWriter outfile = new StreamWriter(filename + ".csv"))
            {
                //outfile.Write(pdfText.ToCsvLine());
                result.ForEach(el => outfile.WriteLine(el.ToCsvLine()));
            }*/

        }
    }
}
