using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UMKBRequests.Models.API.Semantic;

namespace IASK.InterviewerEngine
{

    /// <summary>
    /// Класс промежуточных данных нозологии.
    /// </summary>
    internal class ETItem
    {
        public readonly PlexNewBigId MasterPlex;

        public bool ContainMasterPlex;

        /// <summary>
        /// Индекс элемента графа
        /// </summary>
        public ulong id; // индекс элемента графа

        /// <summary>
        /// Название концепта/связи
        /// </summary>
        public string name; // название концепта/связи

        /// <summary>
        /// Название концепта/связи левого плеча
        /// </summary>

        public string name_ida;

        /// <summary>
        /// Коментарий добавочный к NameDebug
        /// </summary>

        public string nameDebugAdd;

        /// <summary>
        /// Тип элемента графа:
        /// 0 - ребро;
        /// 1 - вершина;
        /// 2 - промежуточный признак (ребро в центре вершина).
        /// </summary>
        public short type; // тип элемента графа: 0 - ребро; 1 - вершина; 2 - промежуточный признак (ребро в центре вершина)

        /// <summary>
        /// вес ребра
        /// </summary>
        private double? _value_a;

        /// <summary>
        /// вес ребра
        /// </summary>
        public double? Value_A
        {
            get
            {
                if (this.type == 1) return 1;

                CheckerValue checker = new CheckerValue();
                List<double> values = new List<double>();

                HashSet<uint> keys = new HashSet<uint>();

                foreach (uint key in ConditionSymptom.Keys)
                    keys.Add(key);
                foreach (uint key in Person.Keys)
                    keys.Add(key);
                foreach (uint key in NatureSymptom.Keys)
                    keys.Add(key);

                foreach (uint key in keys)
                {
                    if (ConditionSymptom.ContainsKey(key))
                    {
                        if (ConditionSymptom[key].value_a <= -0.5 + double.Epsilon)
                            values.Add(.0);

                        if (ConditionSymptom[key].value_a >= 0.5 - double.Epsilon)
                        {
                            if (NatureSymptom.ContainsKey(key) && checker.CheckValue_a(NatureSymptom[key].value_a))
                                values.Add(checker.GetCheckedValue_a(NatureSymptom[key].value_a));

                            if (Person.ContainsKey(key) && checker.CheckValue_a(Person[key].value_a))
                                values.Add(checker.GetCheckedValue_a(Person[key].value_a));
                        }
                    }
                    else
                    {
                        if (NatureSymptom.ContainsKey(key) && checker.CheckValue_a(NatureSymptom[key].value_a))
                            values.Add(checker.GetCheckedValue_a(NatureSymptom[key].value_a));

                        if (Person.ContainsKey(key) && checker.CheckValue_a(Person[key].value_a))
                            values.Add(checker.GetCheckedValue_a(Person[key].value_a));
                    }
                }

                if (values.Count > 0)
                {
                    double temp = .0;
                    foreach (double item in values)
                        temp += item;

                    return temp / values.Count;
                }

                if (_value_a != null) return _value_a;
                else return .0;
            }
            set
            {
                CheckerValue checker = new CheckerValue();
                if (value != null)
                    _value_a = checker.GetNewCalcValue_a((double)value);
            }
        }

        public PlexNewBigId CorrectionCurrent { get; private set; }

        /// <summary>
        /// степень доказательности веса
        /// </summary>
        private double? _value_d;

        /// <summary>
        /// степень доказательности веса
        /// </summary>
        public double? Value_D
        {
            get
            {
                if (this.type == 1) return 1;

                CheckerValue checker = new CheckerValue();
                List<double> values = new List<double>();
                HashSet<ulong> keys = new HashSet<ulong>();

                foreach (ulong key in ConditionSymptom.Keys)
                    keys.Add(key);
                foreach (ulong key in Person.Keys)
                    keys.Add(key);
                foreach (ulong key in NatureSymptom.Keys)
                    keys.Add(key);

                foreach (ulong key in keys)
                {
                    if (ConditionSymptom.ContainsKey(key))
                    {
                        if (ConditionSymptom[key].value_a >= 0.5 - double.Epsilon)
                        {
                            if (NatureSymptom.ContainsKey(key) && checker.CheckValue_a(NatureSymptom[key].value_a) && checker.CheckValue_d(NatureSymptom[key].value_d))
                                values.Add(checker.GetCheckedValue_d(NatureSymptom[key].value_d));

                            if (Person.ContainsKey(key) && checker.CheckValue_a(Person[key].value_a) && checker.CheckValue_d(Person[key].value_d))
                                values.Add(checker.GetCheckedValue_d(Person[key].value_d));
                        }
                    }
                    else
                    {
                        if (NatureSymptom.ContainsKey(key) && checker.CheckValue_a(NatureSymptom[key].value_a) && checker.CheckValue_d(NatureSymptom[key].value_d))
                            values.Add(checker.GetCheckedValue_d(NatureSymptom[key].value_d));

                        if (Person.ContainsKey(key) && checker.CheckValue_a(Person[key].value_a) && checker.CheckValue_d(Person[key].value_d))
                            values.Add(checker.GetCheckedValue_d(Person[key].value_d));
                    }
                }

                if (values.Count > 0)
                {
                    double temp = .0;
                    foreach (double item in values)
                        temp += item;

                    return temp / values.Count;
                }

                if (_value_d != null) return _value_d;
                else return .0;
            }
            set
            {
                CheckerValue checker = new CheckerValue();
                if (value != null)
                    _value_d = checker.GetNewCalcValue_d((double)value);
            }
        }

        /// <summary>
        /// глубина элемента графа
        /// </summary>
        public short deep; // глубина элемента графа

        /// <summary>
        /// индекс левого плеча
        /// </summary>
        public ulong ida; // индекс левого плеча

        /// <summary>
        /// индекс правого плеча
        /// </summary>
        public ulong idb; // индекс правого плеча

        /// <summary>
        /// общий посчитаный вес ребра
        /// </summary>
        private double _calc_value_a;

        /// <summary>
        /// общий посчитаный вес ребра
        /// </summary>
        public double CalcValue_A
        {
            get => _calc_value_a;
            set
            {
                CheckerValue checker = new CheckerValue();
                _calc_value_a = checker.GetNewCalcValue_a(value);
            }
        }

        /// <summary>
        /// общая посчитаная степень доказательности веса
        /// </summary>
        private double _calc_value_d;

        /// <summary>
        /// общая посчитаная степень доказательности веса
        /// </summary>
        [IgnoreDataMember]
        public double CalcValue_D
        {
            get => _calc_value_d;
            set
            {
                CheckerValue checker = new CheckerValue();
                _calc_value_d = checker.GetNewCalcValue_d(value);
            }
        }

        /// <summary>
        /// индекс предиката (связи) или типа концепта
        /// </summary>

        public ushort level; // индекс предиката (связи) или типа концепта

        /// <summary>
        /// направление связи:
        /// 1-слево на право;
        /// 2-справо на лево;
        /// 3-двусторонняя связь;
        /// 0-неориентированный граф (связь без направления).
        /// </summary>

        public short route; // направление связи: 1 - слево на право (ida → idb); 2 - справо на лево (ida ← idb), 3 - двусторонняя связь (id_a <→ id_b)), 0 - неориентированный граф (связь без направления)

        /// <summary>
        /// тип концепта правого плеча
        /// </summary>
        public ushort levelb; // тип концепта правого плеча

        /// <summary>
        /// Кол-во корневых ID (симптомов) сылающиеся на промежуточных данных нозологии
        /// </summary>
        public Dictionary<ulong, string> countRootID;

        /// <summary>
        /// имя концепта правого плеча
        /// </summary>
        public string levelbName;

        /// <summary>
        /// Характер симптома.
        /// </summary>
        public Dictionary<ulong, PlexNewBigId> NatureSymptom;

        /// <summary>
        /// Персонализация отношений.
        /// </summary>
        public Dictionary<ulong, PlexNewBigId> Person;

        /// <summary>
        /// Условия при которых симптом проявляется.
        /// </summary>
        public Dictionary<ulong, PlexNewBigId> ConditionSymptom;

        /// <summary>
        /// Условия при которых сенсор проявляется(Вкл).
        /// </summary>
        public Dictionary<ulong, PlexNewBigId> SensorTurnOn;

        /// <summary>
        /// Логические условия.
        /// </summary>
        public Dictionary<ulong, HashSet<ulong>> LogicSensorTurnOn;

        /// <summary>
        /// Уточнение параметров значений.
        /// </summary>
        public Dictionary<ulong, Dictionary<ulong, PlexNewBigId>> Correction;

        /// <summary>
        /// Заболеваемость в долях по IDB плечу.
        /// </summary>
        public double Prevalence;

        internal ETItem Copy()
        {
            ETItem temp = new ETItem(
                this.MasterPlex.Copy(),
                this.MasterPlex.id,//
                this.name,
                this.type,
                this._value_a,//
                this._value_d,
                this.deep,//
                this.MasterPlex.ida,//
                this.MasterPlex.idb,//
                this._calc_value_a,
                this._calc_value_d,
                this.level,//
                this.route,
                this.levelb,
                this.name_ida,
                this.nameDebugAdd,
                new Dictionary<ulong, string>(),
                this.levelbName,
                new Dictionary<ulong, PlexNewBigId>(),
                new Dictionary<ulong, PlexNewBigId>(),
                new Dictionary<ulong, PlexNewBigId>(),
                new Dictionary<ulong, PlexNewBigId>(),
                this.Prevalence,
                new Dictionary<ulong, HashSet<ulong>>(), new Dictionary<ulong, Dictionary<ulong, PlexNewBigId>>()
);

            foreach (ulong key in this.countRootID.Keys)
                temp.countRootID.Add(key, this.countRootID[key]);

            foreach (uint key in this.NatureSymptom.Keys)
                temp.NatureSymptom.Add(key, this.NatureSymptom[key].Copy());

            foreach (uint key in this.Person.Keys)
                temp.Person.Add(key, this.Person[key].Copy());

            foreach (uint key in this.ConditionSymptom.Keys)
                temp.ConditionSymptom.Add(key, this.ConditionSymptom[key].Copy());

            foreach (ulong key in this.SensorTurnOn.Keys)
                temp.SensorTurnOn.Add(key, this.SensorTurnOn[key].Copy());

            foreach (ulong key in this.LogicSensorTurnOn.Keys)
            {
                HashSet<ulong> tempLocal = new HashSet<ulong>();

                foreach (ulong keyLocal in this.LogicSensorTurnOn[key])
                    tempLocal.Add(keyLocal);

                temp.LogicSensorTurnOn.Add(key, tempLocal);
            }

            foreach (ulong key in this.Correction.Keys)
            {
                if (!temp.Correction.ContainsKey(key))
                    temp.Correction.Add(key, new Dictionary<ulong, PlexNewBigId>());

                foreach (var c in this.Correction[key])
                    temp.Correction[key].Add(c.Key, c.Value.Copy());
            }

            return temp;
        }

        public ETItem()
        {
            this.id = new uint();
            this.name = String.Empty;
            this.type = new short();
            this._value_a = null;
            this._value_d = null;
            this.deep = new short();
            this.ida = new uint();
            this.idb = new uint();
            this._calc_value_a = new double();
            this._calc_value_d = new double();
            this.level = new ushort();
            this.route = new short();
            this.levelb = new ushort();
            this.name_ida = String.Empty;
            this.nameDebugAdd = String.Empty;
            this.countRootID = new Dictionary<ulong, string>();
            this.levelbName = String.Empty;
            this.NatureSymptom = new Dictionary<ulong, PlexNewBigId>();
            this.Person = new Dictionary<ulong, PlexNewBigId>();
            this.ConditionSymptom = new Dictionary<ulong, PlexNewBigId>();
            this.SensorTurnOn = new Dictionary<ulong, PlexNewBigId>();
            this.Prevalence = new double();
            this.MasterPlex = new PlexNewBigId();
            this.ContainMasterPlex = false;
            this.LogicSensorTurnOn = new Dictionary<ulong, HashSet<ulong>>();
            this.Correction = new Dictionary<ulong, Dictionary<ulong, PlexNewBigId>>();
        }

        public ETItem(PlexNewBigId PlexNewBigId)
        {
            this.id = new uint();//PlexNewBigId
            this.deep = new short();
            this.ida = new uint();
            this.idb = new uint();
            this.level = new ushort();
            //levela
            this.levelb = new ushort();
            //page
            this.route = new short();
            //sort
            this.type = new short();
            this._value_a = null;//value_a 
            this._value_d = null;//value_b
            //value_c
            //value_d

            this.name = String.Empty;
            this._calc_value_a = new double();
            this._calc_value_d = new double();



            this.name_ida = String.Empty;
            this.nameDebugAdd = String.Empty;
            this.countRootID = new Dictionary<ulong, string>();
            this.levelbName = String.Empty;
            this.NatureSymptom = new Dictionary<ulong, PlexNewBigId>();
            this.Person = new Dictionary<ulong, PlexNewBigId>();
            this.ConditionSymptom = new Dictionary<ulong, PlexNewBigId>();
            this.SensorTurnOn = new Dictionary<ulong, PlexNewBigId>();
            this.Prevalence = new double();
            this.MasterPlex = PlexNewBigId;
            this.ContainMasterPlex = true;
            this.LogicSensorTurnOn = new Dictionary<ulong, HashSet<ulong>>();
            this.Correction = new Dictionary<ulong, Dictionary<ulong, PlexNewBigId>>();
        }

        public ETItem(
            PlexNewBigId PlexNewBigId,
            ulong id,
            string name,
            short type,
            double? _value_a,
            double? _value_d,
            short deep,
            ulong ida,
            ulong idb,
            double _calc_value_a,
            double _calc_value_d,
            ushort level,
            short route,
            ushort levelb,
            string name_ida,
            string nameDebugAdd,
            Dictionary<ulong, string> countRootID,
            string levelbName,
            Dictionary<ulong, PlexNewBigId> NatureSymptom,
            Dictionary<ulong, PlexNewBigId> Person,
            Dictionary<ulong, PlexNewBigId> ConditionSymptom,
            Dictionary<ulong, PlexNewBigId> SensorTurnOn,
            double Prevalence,
            Dictionary<ulong, HashSet<ulong>> LogicSensorTurnOn,
            Dictionary<ulong, Dictionary<ulong, PlexNewBigId>> Correction)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this._value_a = _value_a;
            this._value_d = _value_d;
            this.deep = deep;
            this.ida = ida;
            this.idb = idb;
            this._calc_value_a = _calc_value_a;
            this._calc_value_d = _calc_value_d;
            this.level = level;
            this.route = route;
            this.levelb = levelb;
            this.name_ida = name_ida;
            this.nameDebugAdd = nameDebugAdd;
            this.countRootID = countRootID;
            this.levelbName = levelbName;
            this.NatureSymptom = NatureSymptom;
            this.Person = Person;
            this.ConditionSymptom = ConditionSymptom;
            this.SensorTurnOn = SensorTurnOn;
            this.Prevalence = Prevalence;
            this.MasterPlex = PlexNewBigId;
            this.ContainMasterPlex = true;
            this.LogicSensorTurnOn = LogicSensorTurnOn;
            this.Correction = Correction;
        }

        internal bool TryAddNatureSymptom(PlexNewBigId PlexNewBigId)
        {
            if (PlexNewBigId.ida == this.MasterPlex.id && Constants.LevelNatureSymptom.Contains(PlexNewBigId.level) && !this.NatureSymptom.ContainsKey(PlexNewBigId.idb))
            {
                this.NatureSymptom.Add(PlexNewBigId.idb, PlexNewBigId);
                return true;
            }
            return false;
        }

        internal bool TryAddPerson(PlexNewBigId PlexNewBigId)
        {
            if (PlexNewBigId.ida == this.MasterPlex.id && Constants.LevelPerson.Contains(PlexNewBigId.level) && !this.Person.ContainsKey(PlexNewBigId.idb))
            {
                this.Person.Add(PlexNewBigId.idb, PlexNewBigId);
                return true;
            }
            return false;
        }

        internal bool TryAddConditionSymptom(PlexNewBigId PlexNewBigId)
        {
            if (PlexNewBigId.ida == this.id && Constants.LevelConditionSymptom.Contains(PlexNewBigId.level) && !this.ConditionSymptom.ContainsKey(PlexNewBigId.idb))
            {
                this.ConditionSymptom.Add(PlexNewBigId.idb, PlexNewBigId);
                return true;
            }
            return false;
        }

        internal bool TryAddSensorTurnOn(PlexNewBigId PlexNewBigId)
        {
            if (PlexNewBigId.ida == this.id && Constants.LevelCheckerSensorTurnOn.Contains(PlexNewBigId.level) && !this.SensorTurnOn.ContainsKey(PlexNewBigId.id))
            {
                this.SensorTurnOn.Add(PlexNewBigId.id, PlexNewBigId);
                return true;
            }
            return false;
        }

        internal bool TryAddLogicSensorTurnOn(PlexNewBigId PlexNewBigId)
        {
            if (this.SensorTurnOn.ContainsKey(PlexNewBigId.ida) && this.SensorTurnOn.ContainsKey(PlexNewBigId.idb))
            {
                if (!this.LogicSensorTurnOn.ContainsKey(PlexNewBigId.idb))
                    this.LogicSensorTurnOn.Add(PlexNewBigId.idb, new HashSet<ulong>());

                this.LogicSensorTurnOn[PlexNewBigId.idb].Add(PlexNewBigId.ida);
                return true;
            }
            return false;
        }

        internal bool TryAddCorrection(PlexNewBigId PlexNewBigId)
        {
            if (PlexNewBigId.ida == this.id && Constants.LevelCorrection.Contains(PlexNewBigId.level) && !this.Correction.ContainsKey(PlexNewBigId.id))
            {
                if (!Correction.ContainsKey(PlexNewBigId.idb))
                    this.Correction.Add(PlexNewBigId.idb, new Dictionary<ulong, PlexNewBigId>());
                if (!this.Correction[PlexNewBigId.idb].ContainsKey(PlexNewBigId.id))
                    this.Correction[PlexNewBigId.idb].Add(PlexNewBigId.id, PlexNewBigId);
                return true;
            }
            return false;
        }

        public bool? IsCorrectionTurnOn(PlexNewBigId PlexNewBigId, bool isTurnOn, double? value)
        {
            if (PlexNewBigId.value_a == null || !isTurnOn)
                return null;

            int caseSwitch = Convert.ToInt32(Math.Round((double)PlexNewBigId.value_a));
            bool flag = value != null;

            switch (caseSwitch)
            {
                case -2:
                    if (flag)
                        if (value <= PlexNewBigId.value_b + double.Epsilon)
                            return true;
                    return false;
                case -1:
                    if (flag)
                        if (value < PlexNewBigId.value_b - double.Epsilon)
                            return true;
                    return false;
                case 0:
                    if (flag)
                        if (value >= PlexNewBigId.value_b - double.Epsilon &&
                            value <= PlexNewBigId.value_c + double.Epsilon)
                            return true;
                    return false;
                case 1:
                    if (flag)
                        if (value > PlexNewBigId.value_b + double.Epsilon)
                            return true;
                    return false;

                case 2:
                    if (flag)
                        if (value >= PlexNewBigId.value_b - double.Epsilon)
                            return true;
                    return false;
                default:
                    return null;
            }
        }


        public ETItem GetReverseID_AB()
        {
            if (this.ContainMasterPlex)
            {
                ETItem eTItem = new ETItem(
                    this.MasterPlex,
                    this.id,
                    this.name_ida,
                    this.type,
                    this._value_a,
                    this._value_d,
                    this.deep,
                    this.idb,
                    this.ida,
                    new double(),
                    new double(),
                    this.level,
                    this.route,
                    new ushort(),
                    this.name,
                    this.nameDebugAdd + "\"REVERSE\"",
                    new Dictionary<ulong, string>(),
                    this.levelbName + "\"REVERSE\"",
                    this.NatureSymptom,
                    this.Person,
                    this.ConditionSymptom,
                    this.SensorTurnOn,
                    new double(),
                    this.LogicSensorTurnOn,
                    this.Correction);

                return eTItem;
            }
            else
            {
                //ETItem eTItem = new ETItem(
                //    this.id,
                //    this.name_ida,
                //    this.type,
                //    this._value_a,
                //    this._value_d,
                //    this.deep,
                //    this.idb,
                //    this.ida,
                //    new double(),
                //    new double(),
                //    this.level,
                //    this.route,
                //    new ushort(),
                //    this.name,
                //    this.nameDebugAdd + "\"REVERSE\"",
                //    new Dictionary<ulong, string>(),
                //    this.levelbName + "\"REVERSE\"",
                //    this.NatureSymptom,
                //    this.Person,
                //    this.ConditionSymptom,
                //    this.SensorTurnOn,
                //    new double(),
                //    this.LogicSensorTurnOn,
                //    this.Correction);

                return null;
            }
        }
    }
}

