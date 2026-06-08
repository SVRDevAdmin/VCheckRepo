using LiteDB;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vcheck_Listener.Lib.Models;
using static Vcheck_Listener.Views.ConfigurationWindow;

namespace Vcheck_Listener.Lib.Repositories
{
    class TestResultRepository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        //public static LiteDatabase db = new LiteDatabase("Filename=Storage/TestResult.db;Password=Vch@ck123;");

        public static TestResultReferenceRangeModel GetReferenceRangeByParameterAnalyzerSpecies(string parameter, string analyzer, string species, string ageGroup)
        {
            try
            {
                ILiteCollection<TestResultReferenceRangeModel> sTestResultReferenceRange = null;

                using (var db = new LiteDatabase("Filename=Storage/TestResult.db;Password=Vch@ck123;"))
                {
                    sTestResultReferenceRange = db.GetCollection<TestResultReferenceRangeModel>("TestResultReferenceRange");
                }                

                IEnumerable<TestResultReferenceRangeModel> referenceRangeInfos = new List<TestResultReferenceRangeModel>();

                if (string.IsNullOrEmpty(analyzer))
                {
                    referenceRangeInfos = sTestResultReferenceRange.Find(x => x.Parameter == parameter);
                }
                else
                {
                    referenceRangeInfos = sTestResultReferenceRange.Find(x => x.Parameter == parameter && x.Analyzer.Contains(analyzer));
                }

                TestResultReferenceRangeModel referenceRangeInfo = new TestResultReferenceRangeModel();

                if (referenceRangeInfos != null && referenceRangeInfos.Count() > 0)
                {
                    referenceRangeInfo = referenceRangeInfos.FirstOrDefault(x => x.Species == species && x.AgeGroup == ageGroup);
                }
                else
                {
                    return referenceRangeInfo;
                }


                if (referenceRangeInfo != null)
                {
                    return referenceRangeInfo;
                }
                else
                {
                    referenceRangeInfo = referenceRangeInfos.FirstOrDefault(x => x.Species == species);

                    if (referenceRangeInfo != null)
                    {
                        return referenceRangeInfo;
                    }
                    else
                    {
                        return referenceRangeInfos.FirstOrDefault(x => x.AgeGroup == ageGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error >>> " + ex.ToString());

                return null;
            }
        }
    }
}
