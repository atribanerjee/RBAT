using RBAT.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IProjectService {
        void AddProject(string name, string description, DateTime calculationBeginsDate, DateTime calculationEndsDate, int dataReadTypeId, int routingOptionTypeId);
        Project GetProjectByID( int projectId);
        List<Project> GetProjects();
        void UpdateProject( Project listToUpdate );
        bool RemoveProject(List<Project> project);

        List<ProjectNode> GetAllProjectNode(int projectId);
        Task<bool> SaveProjectNode(int projectId, int nodeId, bool isChecked);

        List<ProjectChannel> GetAllProjectChannel(int projectId);
        Task<bool> SaveProjectChannel(int projectId, int channelId, bool isChecked);

        List<ProjectNode> GetProjectNodes(int projectId);
        List<ProjectChannel> GetProjectChannels(int projectId);

        void UpdateProjectMapData(int projectId, string projectMapData);

        Task<Tuple<int, Dictionary<int, int>, Dictionary<int, int>>> CopyProject(int projectId, string username = null);
    }
}
