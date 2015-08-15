using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.Prjs.Fgmaiss.JoinSal
{
    class Log
    {
        public Log(String text) {
            var ss = text.Split(' ').Where(el => el.Length > 1).ToList();
            Registration = int.Parse(ss[0]);
            Earning = double.Parse(ss[ss.Count-1]);
        }

        public String ToCsvLine(){
            return (Registration.ToString() + ';' + Earning.ToString());
        }
        int Registration;
        double Earning;
    }
}
