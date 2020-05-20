using RBAT.Logic.TransferModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ISolverService {
        Task<List<FileInfo>> RunSolver(int projectId, long scenarioId, bool isSeasonalModel, bool saveConstraintsAsTXTFile,
                                       bool saveConstraintsAsLINDOFile, bool saveOptimalSolutionsAsTXTFile, bool saveResultsAsVolumes, 
                                       bool isDebugMode, bool saveComponentName, double sensitivityAnalysis, double aridityFactor, SeasonalModel seasonalModel);

        Task<byte[]> RunSolverFromWebService(int projectId, long scenarioId, bool saveConstraintsAsTXTFile,
                                                    bool saveConstraintsAsLINDOFile, bool saveOptimalSolutionsAsTXTFile, bool saveResultsAsVolumes,
                                                    bool isDebugMode, bool saveComponentName, double sensitivityAnalysis, double aridityFactor);
        Task<SeasonalModel> GetSeasonalData(int projectId, long scenarioId, bool isSeasonalModel, DateTime startDate, int numberOfTimeIntervals, double sensitivityAnalysis, double aridityFactor);
    }
}
