using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.Prjs.Fgmaiss.ParMed
{
    public class Transaction
    {
        public DateTime Date;
        public Int32 Registration;
        public String Description = "";
        public Int32 Quantity;
        public Double Value;
        public Item Item;
        public String Raw;
        
        public Transaction()
        {

        }

        public Transaction(Item item)
        {
            Item = item;
           
        }
        public String ToCsvLine() {
            String result = Item.Name + ";" + Item.Quantity + ";" + Item.Value +";" + Date.ToString() + ";" + Registration + ";" + Quantity + ";" + Value + ";" + Description;
            return result;
        }
        public void Parse(IEnumerable<String> rawLines){
            Raw = String.Join(" ", rawLines);
            var words = Raw.Split(' ').ToList();
            
            Date = DateTime.Parse(words[0]);
            for (var i = 1; i < words.Count - 2; i++)
            {
                Int32 i1, i2;
                Double d;
                if (Int32.TryParse(words[i].Replace(".",""), out i1) && Double.TryParse(words[i + 1], out d) && Int32.TryParse(words[i + 2].Replace(".",""), out i2))
                {
                    Quantity = i1;
                    Value = d;
                    Registration = i2;
                    words.RemoveRange(i, 3);
                    words.RemoveAt(0);
                    Description = String.Join(" ", words);
                    break;
                }
            }
        }
       
    }

    public class Item
    {
        public String Name;
        public String Unity;
        public Int32 Quantity;
        public Double Value;
        public List<Transaction> Transactions = new List<Transaction>();
        public String Raw;
        public IEnumerable<String> Parsing;
        
        public Item()
        {

        }
        public Item(String raw)
        {
            
            Raw = raw;
            var words = raw.Split(' ');
            Name = String.Join(" ", words.TakeWhile(el=> el != "Produto:"));
            Unity = String.Join(" ", words.SkipWhile(el => el != "Unidade:").Skip(1).TakeWhile( el => el != "Estoque"));
            var s = words.SkipWhile(el => el != "Atual:").ElementAt(1).Replace(".", "");
            Quantity = Int32.Parse(s);            
            Value = Double.Parse(words.Last());
        }
        public void ParseTransactions(IEnumerable<String> rawLines)
        {
            Parsing = rawLines;
            var lines = rawLines.Where(el => el != Raw).ToList();
            var currentText = new List<String>();
            DateTime.Parse(lines[0].Split(' ')[0]);
            currentText.Add(lines[0]);
            var currentTransaction = new Transaction(this);
            lines.Skip(1).ToList().ForEach(el =>
            {
               
                var words = el.Split(' ');

                DateTime date;
                String supposedDate = el.Split(' ')[0];
                bool repMark = false;
                String rep = "";
                if (supposedDate.Length == 9)
                {
                    
                    rep = supposedDate;
                    supposedDate += '5';
                    repMark = true;
                }
                if (supposedDate.Length == 10 && supposedDate[2] == '/' && supposedDate[5] == '/' && DateTime.TryParse(supposedDate, out date))
                {
                    if (repMark) {
                        repMark = false;
                        el = el.Replace(rep,supposedDate);
                    }
                    currentTransaction.Parse(currentText);
                    Transactions.Add(currentTransaction);
                    currentTransaction = new Transaction(this);
                    currentText = new List<String>();
                    currentText.Add(el);
                }
                else
                {
                    currentText.Add(el);
                }
             
            });
            currentTransaction.Parse(currentText);
            Transactions.Add(currentTransaction);

        }

    }

}
