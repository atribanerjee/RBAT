namespace RBAT.Logic {

    /// <summary>
    /// The class to encapsulate EmailSettings properties from appsettings.json
    /// </summary>
    public class EmailSettings {

        public string FromEmail { get; set; }

        public string PrimaryDomain { get; set; }

        public int PrimaryPort { get; set; }

        public string UsernameEmail { get; set; }

        public string UsernamePassword { get; set; }

    }

}
