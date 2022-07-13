using System.Globalization;

namespace Arash.Home.ExcelGenerator.ExcelGenerator.AdapterOptions
{
    public class ToPersianDate : AdapterBase
    {
        public override string Name => "to-persian-date";

        public override string Description => "";

        public override string Execute(string value)
        {
            var dtt = DateTime.Parse(value);
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(dtt)}-{pc.GetMonth(dtt)}-{pc.GetDayOfMonth(dtt)} {pc.GetHour(dtt)}:{pc.GetMinute(dtt)}-{pc.GetSecond(dtt)}";
        }
    }
}
