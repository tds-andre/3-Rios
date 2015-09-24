
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.Prjs.Fgmaiss.Trash
{
    class Scan
    {
        public List<Route> Routes = new List<Route>();
        public String Type;
        public String Location;
        public String Team;
        public static String CsvHeaders(){
            return "Tipo" + ";" + "Região" + ";" + "Equipe" + ";";
        }
        public String ToCsvLine()
        {
            return Type + ";" + Location + ";" + Team +";";
        }
    }

    class Route
    {
        public String Address;
        public Int32 Meters;
        public Byte DailyFrequncy;
        public String WeekRange;
        public Int32 MontlyFrenquency;
        public Int32 TotalMeters;
        public static String CsvHeaders(){
            return "Endereço" + ";" + "Metragem Linear" + ";" + "Quantitativo Linear" + ";" + "Frequência Semanal" + ";" + "Quant. Dia/Mes" + ";" + "Metragem Diária" + ";";
        }
        public String ToCsvLine()
        {
            return Address + ";" + Meters + ";" + DailyFrequncy + ";" + WeekRange + ";" + MontlyFrenquency + ";" + TotalMeters + ";";
        }
    }
}
            