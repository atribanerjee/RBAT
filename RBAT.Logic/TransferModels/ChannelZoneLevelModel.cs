using System.ComponentModel;

namespace RBAT.Logic.TransferModels
{
    public class ChannelZoneLevelModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
 
    enum ChannelZoneLevel {
        [Description("Zone 6 ↑")] ZoneAboveIdeal6 = 6,
        [Description("Zone 5 ↑")] ZoneAboveIdeal5 = 5,
        [Description("Zone 4 ↑")] ZoneAboveIdeal4 = 4,
        [Description("Zone 3 ↑")] ZoneAboveIdeal3 = 3,
        [Description("Zone 2 ↑")] ZoneAboveIdeal2 = 2,
        [Description("Zone 1 ↑")] ZoneAboveIdeal1 = 1,
        [Description("Ideal Zone")] IdealZone = 0,
        [Description("Zone 1 ↓")] ZoneBelowIdeal1 = -1,
        [Description("Zone 2 ↓")] ZoneBelowIdeal2 = -2,
        [Description("Zone 3 ↓")] ZoneBelowIdeal3 = -3,
        [Description("Zone 4 ↓")] ZoneBelowIdeal4 = -4,
        [Description("Zone 5 ↓")] ZoneBelowIdeal5 = -5,
        [Description("Zone 6 ↓")] ZoneBelowIdeal6 = -6,
        [Description("Zone 7 ↓")] ZoneBelowIdeal7 = -7,
        [Description("Zone 8 ↓")] ZoneBelowIdeal8 = -8,
        [Description("Zone 9 ↓")] ZoneBelowIdeal9 = -9,
        [Description("Zone 10 ↓")] ZoneBelowIdeal10 = -10
    }
}
