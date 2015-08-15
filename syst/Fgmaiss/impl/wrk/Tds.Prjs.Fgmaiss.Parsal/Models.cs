using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.Prjs.Fgmaiss.ParSal.Models
{
    [Serializable]
    public class CivilServant
    {
        public long ID;
        public string Name;
        public long Registration;
        public long CPF;
        public DateTime RecruitedAt;
        public DateTime ?DismissedAt;
        public List<YearEarning> Earnings = new List<YearEarning>();
        public String ToCsv(){
            return ID.ToString() + ";" + Name + ";" + Registration.ToString() + ";" + CPF.ToString() + ";" + RecruitedAt.ToString() + ";" + (DismissedAt==null?"":DismissedAt.ToString());
        }
        public static CivilServant ParseCsv(String line)
        {
            var result = new CivilServant();
            var fields = line.Split(';');
            result.ID =  long.Parse(fields[0]);
            result.Name = fields[1];
            result.Registration = long.Parse(fields[2]);
            result.CPF = long.Parse(fields[3]);
            result.RecruitedAt = DateTime.Parse(fields[4]);
            if (fields.Length > 5 && fields[5].Length > 5)
                result.DismissedAt = DateTime.Parse(fields[5]);
            return result;
        }
        public String FormatID()
        {
            return ID.ToString().PadLeft(6, '0');
        }

    }

    [Serializable]
    public class YearEarning
    {
        public int Year;
        public List<Double> MonthEarning = new List<Double>();
    }
}
