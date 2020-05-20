using RBAT.Core.Clasess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ITimeComponentService
    {
        Task SaveAllFromFile(int elementID, DateTime startDate, TimeComponent timeComponent, List<PastedTimeComponent> listFromFile, int projectID = 0);
    }
}
