namespace SmartEP.Service.DataAnalyze.PollutionIndex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPollutionIndexCaculator
    {
        #region Methods

        void AddFactor(string factorName, Enums.CaculatorType caculatorType, decimal? factorConcentration, Enums.PollutionIndexType pollutionIndexType);

        void Clear();

        void Clear(Enums.PollutionIndexType pollutionIndexType);

        System.Collections.Generic.List<PollutionLevelInfo> GetALLFactorPollutionLevelInfos(Enums.PollutionIndexType pollutionIndexType);

        PollutionLevelInfo GetPollutionLevelInfo(string factorName, Enums.CaculatorType caculatorType, Enums.PollutionIndexType pollutionIndexType);

        System.Collections.Generic.List<PollutionLevelInfo> GetPrimaryPollutions(Enums.PollutionIndexType pollutionIndexType);

        void RemoveFactor(string factorName, Enums.CaculatorType caculatorType, Enums.PollutionIndexType pollutionIndexType);

        #endregion Methods
    }
}