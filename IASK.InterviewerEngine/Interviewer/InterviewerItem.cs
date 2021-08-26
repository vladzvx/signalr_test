using System.Collections.Generic;
using System.Collections.Immutable;
using UMKBRequests.Models.API.Semantic;

namespace IASK.InterviewerEngine
{
    internal class CheckerItemBuilder
    {
        internal PlexNewBigId MasterPlex;
        internal ResultSemanticNewBigId plexus;
        public CheckerItemBuilder(PlexNewBigId plex, ResultSemanticNewBigId plexus)
        {
            MasterPlex = plex;
            this.plexus = plexus;
        }

        /// <summary>
        /// Условия при которых сенсор проявляется(Вкл).
        /// </summary>
        public Dictionary<ulong, PlexNewBigId> SensorTurnOn = new Dictionary<ulong, PlexNewBigId>();

        /// <summary>
        /// Логические условия.
        /// </summary>
        public Dictionary<ulong, HashSet<ulong>> LogicSensorTurnOn = new Dictionary<ulong, HashSet<ulong>>();

        /// <summary>
        /// Дочерние элементы группировки
        /// </summary>
        public Dictionary<ulong, PlexNewBigId> OrGroupedItems = new Dictionary<ulong, PlexNewBigId>();
        internal bool TryAddGrouppedChild(PlexNewBigId PlexNewBigId)
        {
            if (this.MasterPlex.idb == Constants.ID_OR&& MasterPlex.id== PlexNewBigId.parent_id)
            {
                return this.OrGroupedItems.TryAdd(PlexNewBigId.id, PlexNewBigId);
            }
            return false;
        }
        internal bool TryAddSensorTurnOn(PlexNewBigId PlexNewBigId)
        {
            if (PlexNewBigId.ida == this.MasterPlex.id && Constants.LevelCheckerSensorTurnOn.Contains(PlexNewBigId.level) && !this.SensorTurnOn.ContainsKey(PlexNewBigId.id))
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

    }
    internal class InterviewerItem
    {
        #region ids

        /// <summary>
        /// Индекс элемента графа
        /// </summary>
        public readonly ulong id; // индекс элемента графа

        /// <summary>
        /// индекс левого плеча
        /// </summary>
        public readonly ulong ida;

        /// <summary>
        /// индекс правого плеча
        /// </summary>
        public readonly ulong idb;

        /// <summary>
        /// индекс предиката (связи) или типа концепта
        /// </summary>
        public readonly ushort level;

        /// <summary>
        /// тип концепта правого плеча
        /// </summary>
        public readonly ushort levelb;
        #endregion

        #region names

        /// <summary>
        /// Название концепта/связи
        /// </summary>
        public readonly string name_idb;

        /// <summary>
        /// Название концепта/связи левого плеча
        /// </summary>
        public readonly string name_ida;

        /// <summary>
        /// Название концепта/связи ребра
        /// </summary>
        public readonly string name;

        /// <summary>
        /// имя концепта правого плеча
        /// </summary>
        public readonly string levelbName;
        #endregion

        #region other

        public readonly double? value_a;
        public readonly double? value_c;
        public readonly double? value_b;
        public readonly double? value_d;
        public readonly ulong? parent_id;
        /// <summary>
        /// направление связи:
        /// 1-слево на право;
        /// 2-справо на лево;
        /// 3-двусторонняя связь;
        /// 0-неориентированный граф (связь без направления).
        /// </summary>
        public readonly short route; // направление связи: 1 - слево на право (ida → idb); 2 - справо на лево (ida ← idb), 3 - двусторонняя связь (id_a <→ id_b)), 0 - неориентированный граф (связь без направления)


        /// <summary>
        /// глубина элемента графа
        /// </summary>
        public readonly short deep; // глубина элемента графа


        /// <summary>
        /// Тип элемента графа:
        /// 0 - ребро;
        /// 1 - вершина;
        /// 2 - промежуточный признак (ребро в центре вершина).
        /// </summary>
        public readonly short type; // тип элемента графа: 0 - ребро; 1 - вершина; 2 - промежуточный признак (ребро в центре вершина)


        #endregion 

        /// <summary>
        /// Условия при которых сенсор проявляется(Вкл).
        /// </summary>
        public readonly ImmutableDictionary<ulong, InterviewerItem> SensorTurnOn;

        /// <summary>
        /// Логические условия.
        /// </summary>
        public readonly ImmutableDictionary<ulong, ImmutableHashSet<ulong>> LogicSensorTurnOn;


        /// <summary>
        /// Вложенные элементы данного сенсора (для группировки по принципу или)
        /// </summary>
        public readonly ImmutableDictionary<ulong, InterviewerItem> OrGroupedItems;


        public InterviewerItem(PlexNewBigId plex, ResultSemanticNewBigId plexus)
        {
            id = plex.id;
            deep = plex.deep;
            type = plex.type;
            ida = plex.ida;
            idb = plex.idb;
            level = plex.level;
            levelb = plex.levelb;
            route = plex.route;
            value_a = plex.value_a;
            value_b = plex.value_b;
            value_c = plex.value_c;
            value_d = plex.value_d;
            parent_id = plex.parent_id;

            if (plex.type == 1)
            {
                if (plexus.names.ContainsKey(plex.id))
                {
                    this.name_idb = plexus.names[plex.id];

                    if (plexus.levels.ContainsKey(plex.level))
                        this.levelbName = plexus.levels[plex.level];
                }
                else
                    this.name_idb = plex.id.ToString();
            }
            else if (plex.type == 0)
            {
                if (plexus.levels.ContainsKey(plex.levelb))
                    this.levelbName = plexus.levels[plex.levelb];

                if (plexus.names.ContainsKey(plex.ida))
                    this.name_ida = plexus.names[plex.ida];
                else
                    this.name_ida = plex.ida.ToString();

                if (plexus.names.ContainsKey(plex.idb))
                    this.name_idb = plexus.names[plex.idb];
                else
                    this.name_idb = plex.idb.ToString();

                if (plexus.names.ContainsKey(plex.id))
                    this.name = plexus.names[plex.id];

            }
            else if (plex.type == 2)
            {
                if (plexus.levels.ContainsKey(plex.levelb))
                    this.levelbName = plexus.levels[plex.levelb];

                if (plexus.names.ContainsKey(plex.ida))
                    this.name_ida = plexus.names[plex.ida];
                else
                    this.name_ida = plex.ida.ToString();

                if (plexus.names.ContainsKey(plex.id))
                    this.name_idb = plexus.names[plex.id];
                else
                    this.name_idb = plex.id.ToString();

                if (plexus.names.ContainsKey(plex.id))
                    this.name = plexus.names[plex.id];

            }

        }

        public InterviewerItem(PlexNewBigId plex)
        {
            id = plex.id;
            deep = plex.deep;
            type = plex.type;
            ida = plex.ida;
            idb = plex.idb;
            level = plex.level;
            levelb = plex.levelb;
            route = plex.route;
            value_a = plex.value_a;
            value_b = plex.value_b;
            value_c = plex.value_c;
            value_d = plex.value_d;
            parent_id = plex.parent_id;

        }

        public InterviewerItem(CheckerItemBuilder eTIte2Template)
        {
            var plex = eTIte2Template.MasterPlex;
            var plexus = eTIte2Template.plexus;

            id = plex.id;
            deep = plex.deep;
            type = plex.type;
            ida = plex.ida;
            idb = plex.idb;
            level = plex.level;
            levelb = plex.levelb;
            route = plex.route;
            value_a = plex.value_a;
            value_b = plex.value_b;
            value_c = plex.value_c;
            value_d = plex.value_d;
            parent_id = plex.parent_id;

            if (plex.type == 1)
            {
                if (plexus.names.ContainsKey(plex.id))
                {
                    this.name_idb = plexus.names[plex.id];

                    if (plexus.levels.ContainsKey(plex.level))
                        this.levelbName = plexus.levels[plex.level];
                }
                else
                    this.name_idb = plex.id.ToString();

                if (plexus.names.ContainsKey(plex.id))
                    this.name = plexus.names[plex.id];
            }
            else if (plex.type == 0)
            {
                if (plexus.levels.ContainsKey(plex.levelb))
                    this.levelbName = plexus.levels[plex.levelb];

                if (plexus.names.ContainsKey(plex.ida))
                    this.name_ida = plexus.names[plex.ida];
                else
                    this.name_ida = plex.ida.ToString();

                if (plexus.names.ContainsKey(plex.idb))
                    this.name_idb = plexus.names[plex.idb];
                else
                    this.name_idb = plex.idb.ToString();

                if (plexus.names.ContainsKey(plex.id))
                    this.name = plexus.names[plex.id];
            }
            else if (plex.type == 2)
            {
                if (plexus.levels.ContainsKey(plex.levelb))
                    this.levelbName = plexus.levels[plex.levelb];

                if (plexus.names.ContainsKey(plex.ida))
                    this.name_ida = plexus.names[plex.ida];
                else
                    this.name_ida = plex.ida.ToString();

                if (plexus.names.ContainsKey(plex.id))
                    this.name_idb = plexus.names[plex.id];
                else
                    this.name_idb = plex.id.ToString();

                if (plexus.names.ContainsKey(plex.id))
                    this.name = plexus.names[plex.id];
            }

            var builder = ImmutableDictionary.CreateBuilder<ulong, ImmutableHashSet<ulong>>();
            foreach (ulong key in eTIte2Template.LogicSensorTurnOn.Keys)
            {
                builder.Add(key, ImmutableHashSet.CreateRange(eTIte2Template.LogicSensorTurnOn[key]));
            }
            LogicSensorTurnOn = builder.ToImmutable();

            var builder2 = ImmutableDictionary.CreateBuilder<ulong, InterviewerItem>();
            foreach (ulong key in eTIte2Template.SensorTurnOn.Keys)
            {
                builder2.Add(key, new InterviewerItem(eTIte2Template.SensorTurnOn[key]));
            }

            SensorTurnOn = builder2.ToImmutable();

            var builder3 = ImmutableDictionary.CreateBuilder<ulong, InterviewerItem>();
            foreach (ulong key in eTIte2Template.OrGroupedItems.Keys)
            {
                builder3.Add(key, new InterviewerItem(eTIte2Template.OrGroupedItems[key]));
            }
            OrGroupedItems = builder3.ToImmutable();
        }

        public static implicit operator PlexNewBigId(InterviewerItem eTItem2)
        {
            return new PlexNewBigId()
            {
                id = eTItem2.id,
                deep = eTItem2.deep,
                type = eTItem2.type,
                ida = eTItem2.ida,
                idb = eTItem2.idb,
                level = eTItem2.level,
                levelb = eTItem2.levelb,
                route = eTItem2.route,
                value_a = eTItem2.value_a,
                value_b = eTItem2.value_b,
                value_c = eTItem2.value_c,
                value_d = eTItem2.value_d,
                parent_id = eTItem2.parent_id,
            };
        }
    }
}
