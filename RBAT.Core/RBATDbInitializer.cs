using System.Linq;
using RBAT.Core.Models;

namespace RBAT.Core {

    public class RBATDbInitializer {

        public void Seed( RBATContext ctx ) {
            if (!ctx.NodeTypes.Any() ) {
                ctx.NodeTypes.Add( new NodeType { Id = 1, Name = "Reservoir", Description = "Reservoir" } );
                ctx.NodeTypes.Add( new NodeType { Id = 2, Name = "Consumptive Use", Description = "Consumptive Use" } );
                ctx.NodeTypes.Add( new NodeType { Id = 3, Name = "Junction", Description = "Junction" } );
                ctx.SaveChanges();
            }

            if (!ctx.ChannelTypes.Any())
            {
                ctx.ChannelTypes.Add(new ChannelType { Id = 1, Name = "River reach", Description = "River reach" });
                ctx.ChannelTypes.Add(new ChannelType { Id = 2, Name = "Diversion channel", Description = "Diversion channel" });
                ctx.ChannelTypes.Add(new ChannelType { Id = 3, Name = "Return flow", Description = "Return flow" });
                ctx.SaveChanges();
            }

            if (!ctx.TimeStepTypes.Any())
            {
                ctx.TimeStepTypes.Add(new TimeStepType { Id = 1, Name = "Daily", CalculationFlag = true, DataFlag = true });
                ctx.TimeStepTypes.Add(new TimeStepType { Id = 2, Name = "Weekly", CalculationFlag = true, DataFlag = true });
                ctx.TimeStepTypes.Add(new TimeStepType { Id = 3, Name = "Monthly", CalculationFlag = true, DataFlag = true });
                ctx.TimeStepTypes.Add(new TimeStepType { Id = 4, Name = "Custom", CalculationFlag = true, DataFlag = true });
                ctx.SaveChanges();
            }

            if (!ctx.RoutingOptionTypes.Any())
            {
                ctx.RoutingOptionTypes.Add(new RoutingOptionType { Id = 1, Name = "Steady State Calculations", Description = "Steady State Calculations" });
                ctx.RoutingOptionTypes.Add(new RoutingOptionType { Id = 2, Name = "SSARR Channel Routing", Description = "Standard SSARR routing with interval halving if need be (invoked automatically if certain conditions are met) and time of travel calculated based on the channel outflow" });
                ctx.SaveChanges();
            }

            if (!ctx.ScenarioTypes.Any())
            {
                ctx.ScenarioTypes.Add(new ScenarioType { Id = 1, Name = "STO", Description = "STO" });
                ctx.ScenarioTypes.Add(new ScenarioType { Id = 2, Name = "MTO", Description = "MTO" });
                ctx.ScenarioTypes.Add(new ScenarioType { Id = 3, Name = "MTO year by year", Description = "MTO year by year" });                
                ctx.SaveChanges();
            }

            if (!ctx.MeasuringUnits.Any())
            {
                ctx.MeasuringUnits.Add(new MeasuringUnit { Id = 1, Name = "Millimeters (per area)", Description = "Millimeters (per area)" });
                ctx.MeasuringUnits.Add(new MeasuringUnit { Id = 2, Name = "Meters (per area)", Description = "Meters (per area)" });
                ctx.MeasuringUnits.Add(new MeasuringUnit { Id = 3, Name = "m3/s", Description = "Flow per second" });
                ctx.SaveChanges();
            }
        }

    }

}
