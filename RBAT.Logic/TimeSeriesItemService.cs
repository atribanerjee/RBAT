using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using RBAT.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class TimeSeriesItemService
    {
        private int remainingNumberOfDaysAYear;
        private int observedYear;

        public async Task<IList<ITimeSeriesItem>> GenerateTimeSeriesData(int elementID, DateTime startDate, TimeComponent timeComponent, List<PastedTimeComponent> pasteList, string timeSeriesItem, int? projectID = null)
        {
            List<ITimeSeriesItem> timeSeriesList = new List<ITimeSeriesItem>();
            SetValuesForAYear(startDate);

            await Task.Run(() => pasteList.ForEach(element =>
            {
                timeSeriesList.Add(CreateTimeSeriesItem(elementID, startDate, timeComponent, element.Value, timeSeriesItem, projectID));
                startDate = SetNewDate(startDate, timeComponent, element.TimeStep.GetValueOrDefault(), remainingNumberOfDaysAYear);

                if (timeComponent == TimeComponent.Week)
                    DoCalculationForAWeekTimeComponent(startDate);
            }));

            return timeSeriesList;
        }

        private ITimeSeriesItem CreateTimeSeriesItem(int elementID, DateTime startDate, TimeComponent timeComponent, double? itemValue, string timeSeriesItem, int? projectID = null)
        {
            TimeSeriesItemCreator factory = new ConcreteTimeSeriesItemCreator();
            return factory.GetItem(elementID, startDate.GetTimeComponentValueCalculatedFromBeginingOfTheYear(timeComponent), timeComponent, itemValue, timeSeriesItem, projectID);
        }

        private DateTime SetNewDate(DateTime startDate, TimeComponent timeComponent, int timeComponentValue, int remainingNumberOfDaysAYear)
        {
            switch (timeComponent)
            {
                case TimeComponent.Month:
                    return startDate.AddMonths(1);
                case TimeComponent.Week:
                    return (remainingNumberOfDaysAYear < 10) ? startDate.AddDays(remainingNumberOfDaysAYear) : startDate.AddDays(7);
                case TimeComponent.Day:
                    return startDate.AddDays(1);
                case TimeComponent.Custom:
                    return startDate.AddDays(timeComponentValue);
                default:
                    return startDate;
            }
        }

        private void DoCalculationForAWeekTimeComponent(DateTime date)
        {
            if (observedYear == date.Year)
                remainingNumberOfDaysAYear = remainingNumberOfDaysAYear - 7;
            else
                SetValuesForAYear(date);
        }

        private void SetValuesForAYear(DateTime date)
        {
            remainingNumberOfDaysAYear = (new DateTime(date.Year, 12, 31)).DayOfYear;
            observedYear = date.Year;
        }        
    }

    public class PastedTimeComponent
    {
        public int? TimeStep { get; set; }
        public double Value { get; set; }
    }
}
