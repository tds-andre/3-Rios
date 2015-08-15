using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.Prjs.Fgmaiss.ParMed
{
    class Transaction
    {
        public DateTime Date;
        public Int32 Registration;
        public String Description = "";
        public Int32 Quantity;
        public Double Value;
        public Item Item;
        public String Raw;
        public Transaction(Item item)
        {
            Item = item;
           
        }
        public void Parse(IEnumerable<String> rawLines){
            Raw = String.Join(" ", rawLines);
            var words = Raw.Split(' ').ToList();
            
            Date = DateTime.Parse(words[0]);
            for (var i = 1; i < words.Count - 2; i++)
            {
                Int32 i1, i2;
                Double d;
                if (Int32.TryParse(words[i], out i1) && Double.TryParse(words[i + 1], out d) && Int32.TryParse(words[i + 2], out i2))
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

    class Item
    {
        public String Name;
        public String Unity;
        public Int32 Quantity;
        public Double Value;
        public List<Transaction> Transactions = new List<Transaction>();
        public String Raw;
        public Item(String raw)
        {
            Raw = raw;
            var words = raw.Split(' ');
            Name = String.Join(" ", words.TakeWhile(el=> el != "Produto:"));
            Unity = String.Join(" ", words.SkipWhile(el => el != "Unidade:").TakeWhile( el => el != "Estoque"));
            Quantity = Int32.Parse(words.SkipWhile( el => el != "Atual:").ElementAt(1));
            Value = Double.Parse(words.Last());
        }
        public void ParseTransactions(IEnumerable<String> rawLines)
        {
            var lines = rawLines.Where(el => el != Raw).ToList();
            var currentText = new List<String>();
            var currentTransaction = new Transaction(this);
            lines.ForEach(el =>
            {
                var words = el.Split(' ');
                DateTime date;
                if (DateTime.TryParse(el.Split(' ')[0], out date))
                {
                    currentTransaction.Parse(currentText);
                    Transactions.Add(currentTransaction);
                    currentTransaction = new Transaction(this);
                    currentText = new List<String>();
                }
                else
                {
                    currentText.Add(el);
                }
            });
        }

    }

}
