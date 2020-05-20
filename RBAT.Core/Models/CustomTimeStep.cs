using RBAT.Core.Clasess;

namespace RBAT.Core.Models {

    public class CustomTimeStep : BaseEntityIntID {
        public decimal Length { get; set; }

        public Scenario Scenario { get; set; }
        public long ScenarioId { get; set; }
    }
}
