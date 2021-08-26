using IASK.InterviewerEngine.Models.Output;
using System.Collections.Generic;
using System.Collections.Immutable;
using UMKBRequests;

namespace IASK.InterviewerEngine
{
    public static class Constants
    {
        public const ushort UsersLib = 902;
        #region Interviewer parameters
        public const ulong IDDataToSymptoms = 6370086;
        public const ulong IDDataToRiskFactor = 6360086;
        public const ulong ID_GROUP = 1220077;
        public const ulong ID_OR = 1230077;
        public const ulong ID_AND = 1240077;
        public const ulong IDTriggerTestBreath = 11740086;
        public const double ProbabilityTreshhold = 0.05;
        public const string HeaderIdb = "0";
        public const string NextButtonIdb = "0";
        public const string NextButtonLabel = "Далее";
        public const int CycleTypeId = 77;
        public const int RequiredId = 1;
        internal static readonly ImmutableDictionary<ulong, InterfaceUnit.UnitType> Functions =
            ImmutableDictionary.CreateRange(new Dictionary<ulong, InterfaceUnit.UnitType>()
            {
                {IdParser.GetNewBigId(71,856),InterfaceUnit.UnitType.CALENDAR },
                {IdParser.GetNewBigId(71,859),InterfaceUnit.UnitType.TIME_INTERVAL },
                {IdParser.GetNewBigId(71,839),InterfaceUnit.UnitType.SCHEDULE }
            });

        #endregion

        #region varchars parsing parameters
        /// <summary>
        /// Для сбора Range-й - параметрических значений с определенным шагом
        /// </summary>
        public const string INPUT_MASK = "input_mask";
        public static readonly ImmutableList<string> VarcharFieldsToContent = ImmutableList.CreateRange(new string[] { "mask_Input", "regular_find", "ValueInputFormat" });
        internal const string InputMaskField = "mask_";
        internal const string InputMaskField2 = "ValueInput";
        internal const string RegexField = "reg";
        internal const string OnePageInterfaceField = "one_page_interface";
        #endregion

        #region Semantic requests parameters
        internal const ushort PageLevel = 1919;
        internal const ushort CheckerLevel = 1918;
        internal const ushort QuestioningLevel = 1925;
        internal const short RouteToRiskFactor = 2;
        internal const short RouteReverseSymptoms = 0;
        internal const short RoutePrevention = 0;
        internal const short RouteParent = 2;
        internal const short RouteChild = 0;
        internal const short RouteEkvivalent = 1;
        internal const short RoutePrevalence = 0;
        internal const short RouteCheackerTrigger = 0;
        public const short RouteTriggerSensor = 0;
        internal const short RouteCheackerSensorTurnOn = 0;
        internal const short RouteTriggerCollector = 0;
        internal const short RouteCheackerNosology = 0;
        internal const short RouteCorrection = 1;
        internal const short RouteBinarization = 0;
        internal const ulong GetPrevalenceIDMain = 1910011;
        internal const short LibOfLibs = 910;
        internal const int TreeDeep = 3;//Глубина запроса графа при формировании дерева


        internal static readonly ImmutableList<ushort> PageLevels = ImmutableList.CreateRange(new ushort[] { PageLevel, 60,168});

        internal static readonly ImmutableList<ushort> LibSymptoms = ImmutableList.CreateRange(new ushort[] { 25, 54, 61 });
        internal static readonly ImmutableList<ushort> Level_bSymptoms = ImmutableList.CreateRange(new ushort[] { 184, 920, 930, 1102, 1104, 279 });
        internal static readonly ImmutableList<ushort> Level_bWork = ImmutableList.CreateRange(new ushort[] { 1, 3, 183, 184, 920, 921, 930, 990, 1102, 1104, 279 });
        internal static readonly ImmutableList<ushort> Level_bNext = ImmutableList.CreateRange(new ushort[] { 920, 930, 1102, 1104 });
        internal static readonly ImmutableList<ushort> Level_bStop = ImmutableList.CreateRange(new ushort[] { 184, 279 });
        internal static readonly ImmutableList<ushort> LevelSymptoms = ImmutableList.CreateRange(new ushort[] { 1502 });
        internal static readonly ImmutableList<ushort> LibRiskFactor = ImmutableList.CreateRange(new ushort[] { 54 });
        internal static readonly ImmutableList<ushort> LevelRiskFactor = ImmutableList.CreateRange(new ushort[] { 1501 });
        internal static readonly ImmutableList<ushort> LibPrevention = ImmutableList.CreateRange(new ushort[] { 25, 54, 61 });
        internal static readonly ImmutableList<ushort> LevelPrevention = ImmutableList.CreateRange(new ushort[] { 1550 });
        internal static readonly ImmutableList<ushort> LibSymptomsParent = ImmutableList.CreateRange(new ushort[] { 25, 54, 61 });
        public static readonly ImmutableList<ushort> LevelToParent = ImmutableList.CreateRange(new ushort[] { 1051, 1050 });
        internal static readonly ImmutableList<ushort> LibEkvivalent = ImmutableList.CreateRange(new ushort[] { 25, 54, 61 });
        internal static readonly ImmutableList<ushort> LevelToEkvivalent = ImmutableList.CreateRange(new ushort[] { 1000 });
        internal static readonly ImmutableList<ushort> LevelNatureSymptom = ImmutableList.CreateRange(new ushort[] { 1401 });
        internal static readonly ImmutableList<ushort> LevelPerson = ImmutableList.CreateRange(new ushort[] { 1404 });
        internal static readonly ImmutableList<ushort> LevelConditionSymptom = ImmutableList.CreateRange(new ushort[] { 1414 });
        internal static readonly ImmutableList<ushort> LibBinarization = ImmutableList.CreateRange(new ushort[] { 25, 54 });
        internal static readonly ImmutableList<ushort> libPrevalence = ImmutableList.CreateRange(new ushort[] { 25, 138, 61 });
        internal static readonly ImmutableList<ushort> LevelPrevalence = ImmutableList.CreateRange(new ushort[] { 1403 });
        internal static readonly ImmutableList<ushort> TreeLib = ImmutableList.CreateRange(new ushort[] { 121 });
        public static readonly ImmutableList<ushort> LibChecker = ImmutableList.CreateRange(new ushort[] { 86 });
        internal static readonly ImmutableList<ushort> LevelCheckerTrigger = ImmutableList.CreateRange(new ushort[] { 1937 });
        public static readonly ImmutableList<ushort> LevelTriggerSensor = ImmutableList.CreateRange(new ushort[] { 1935 });
        internal static readonly ImmutableList<ushort> LevelCheckerSensorTurnOn = ImmutableList.CreateRange(new ushort[] { 1938 });
        internal static readonly ImmutableList<ushort> LevelTriggerCollector = ImmutableList.CreateRange(new ushort[] { 1936 });
        internal static readonly ImmutableList<ushort> LevelCheckerNosology = ImmutableList.CreateRange(new ushort[] { 1939 });
        internal static readonly ImmutableList<ushort> LevelCorrection = ImmutableList.CreateRange(new ushort[] { 3005 });
        internal static readonly ImmutableList<ushort> HospitalsLevel = ImmutableList.CreateRange(new ushort[] { 958, 955, 969 });//level-ы Лечебно-профилактических учреждений
        internal static readonly ImmutableList<ushort> SearchLevelbExceptions = ImmutableList.CreateRange(new ushort[] { 0, 1 });//levelb, исключаемые из формирования поисковой строки

        #endregion

        #region other requests parameters
        public static readonly ImmutableList<string> NamesFilters = ImmutableList.CreateRange(new string[] {"name","short"});
        #endregion

    }
}
