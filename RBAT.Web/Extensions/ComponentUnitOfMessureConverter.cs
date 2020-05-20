namespace RBAT.Web.Extensions
{
    public static class ComponentUnitOfMessureConverter
    {
        public static string GetNodeUOM(RBAT.Core.Models.NodeType nodeType, bool isNetEvaporation, bool useUnitsOfVolume)
        {
            if (useUnitsOfVolume)
            {
                return "m3";
            }

            if (nodeType.Id == RBAT.Core.Models.NodeType.NodeTypeReservoir)
            {
                if (isNetEvaporation)
                {
                    return "m3";
                }
                else
                {
                    return "m3/s";
                }
            }

            return null;
        }

        public static string GetChannelUOM(RBAT.Core.Models.ChannelType channelType, bool useUnitsOfVolume)
        {
            if (useUnitsOfVolume)
            {
                return "m3";
            }
            

            return null;
        }
    }
}
