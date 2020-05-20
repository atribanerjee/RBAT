using System;

namespace RBAT.Web.Models.Admin
{
    public class UserRolesModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public bool IsSubscriber { get; set; }

        public bool IsApplicationUser { get; set; }

        public bool IsWebServiceUser { get; set; }
    }
}
