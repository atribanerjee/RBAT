using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Clasess
{
    public class TimeSeriesItem : BaseEntityLongID
    {
        public TimeSeriesItem() { }

        public TimeSeriesItem(int year, int timeComponentType, int timeComponentValue, double? value)
        {
            Year = year;
            TimeComponentType = timeComponentType;
            TimeComponentValue = timeComponentValue;
            Value = value;
        }

        [Required]
        public int Year { get; set; }
        [Required]
        public int TimeComponentType { get; set; }
        [Required]
        public int TimeComponentValue { get; set; }
        
        public virtual double? Value { get; set; }
    }    
}
