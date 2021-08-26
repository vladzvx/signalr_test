using System.Collections.Generic;
using System.Collections.Immutable;
using UMKBRequests.Models.API.Semantic;

namespace IASK.InterviewerEngine
{
    internal class TriggerBuilder
    {
        public readonly ulong Id;
        public readonly string Name;
        public readonly Dictionary<ulong, CheckerItemBuilder> Sensors;
        public readonly Dictionary<ulong, CheckerItemBuilder> Collectors;
        public readonly Dictionary<ulong, PlexNewBigId> PlexsToSensorGroups;
        public readonly Dictionary<ulong, PlexNewBigId> PlexsToCollectorGroups;


        public TriggerBuilder(ulong id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.Sensors = new Dictionary<ulong, CheckerItemBuilder>();
            this.Collectors = new Dictionary<ulong, CheckerItemBuilder>();
            this.PlexsToSensorGroups = new Dictionary<ulong, PlexNewBigId>();
            this.PlexsToCollectorGroups = new Dictionary<ulong, PlexNewBigId>();
        }

        internal bool TryAddSensor(CheckerItemBuilder eTItem)
        {
            if (eTItem.MasterPlex.ida == this.Id && !this.Sensors.ContainsKey(eTItem.MasterPlex.id))
            {
                this.Sensors.Add(eTItem.MasterPlex.id, eTItem);
                return true;
            }

            return false;
        }

        internal bool TryAddCollector(CheckerItemBuilder eTItem)
        {
            if (eTItem.MasterPlex.ida == this.Id && !this.Collectors.ContainsKey(eTItem.MasterPlex.idb))
            {
                this.Collectors.Add(eTItem.MasterPlex.id, eTItem);
                return true;
            }
            return false;
        }

        internal bool TryAddSensorGroup(PlexNewBigId PlexNewBigId)
        {
            if (this.Sensors.ContainsKey(PlexNewBigId.ida) && this.Sensors.ContainsKey(PlexNewBigId.idb))
            {
                this.PlexsToSensorGroups.Add(PlexNewBigId.ida, PlexNewBigId);
                return true;
            }

            return false;
        }

        internal bool TryAddCollectorGroup(PlexNewBigId PlexNewBigId)
        {
            if (this.Collectors.ContainsKey(PlexNewBigId.ida) && this.Collectors.ContainsKey(PlexNewBigId.idb))
            {
                this.PlexsToCollectorGroups.Add(PlexNewBigId.ida, PlexNewBigId);
                return true;
            }

            return false;
        }
    }

    internal class Trigger
    {
        internal readonly ulong Id;
        internal readonly string Name;
        internal readonly ImmutableDictionary<ulong, InterviewerItem> Sensors;
        internal readonly ImmutableList<ulong> SensorsSequence;
        internal readonly ImmutableDictionary<ulong, InterviewerItem> Collectors;
        internal readonly ImmutableDictionary<ulong, InterviewerItem> PlexsToSensorGroups;
        internal readonly ImmutableDictionary<ulong, InterviewerItem> PlexsToCollectorGroups;
        internal readonly ImmutableList<InterviewerItem> OrGroupsHeads;

        public Trigger(TriggerBuilder checkerTriggerTemplate)
        {
            this.Id = checkerTriggerTemplate.Id;
            this.Name = checkerTriggerTemplate.Name;

            var builderList = ImmutableList.CreateBuilder<InterviewerItem>();
            SensorsSequence = ImmutableList.CreateRange(checkerTriggerTemplate.Sensors.Keys);
            var b1 = ImmutableDictionary.CreateBuilder<ulong, InterviewerItem>();
            foreach (ulong key in checkerTriggerTemplate.Sensors.Keys)
            {
                bool AddOrGroupsHeads = false;
                if (checkerTriggerTemplate.Sensors[key].MasterPlex.idb == Constants.ID_OR)
                {
                    foreach (ulong key2 in checkerTriggerTemplate.Sensors.Keys)
                    {
                        if (checkerTriggerTemplate.Sensors[key2].MasterPlex.idb != Constants.ID_OR)
                        {
                            AddOrGroupsHeads = checkerTriggerTemplate.Sensors[key].TryAddGrouppedChild(checkerTriggerTemplate.Sensors[key2].MasterPlex);             
                        }    
                    }
                }
                var temp = new InterviewerItem(checkerTriggerTemplate.Sensors[key]);
                b1.Add(key, temp);
                if (AddOrGroupsHeads) builderList.Add(temp);
            }
            this.Sensors = b1.ToImmutable();
            OrGroupsHeads = builderList.ToImmutable();
            var b2 = ImmutableDictionary.CreateBuilder<ulong, InterviewerItem>();
            foreach (ulong key in checkerTriggerTemplate.Collectors.Keys)
            {
                b2.Add(key, new InterviewerItem(checkerTriggerTemplate.Collectors[key]));
            }
            this.Collectors = b2.ToImmutable();

            var b3 = ImmutableDictionary.CreateBuilder<ulong, InterviewerItem>();
            foreach (ulong key in checkerTriggerTemplate.PlexsToSensorGroups.Keys)
            {
                b3.Add(key, new InterviewerItem(checkerTriggerTemplate.PlexsToSensorGroups[key]));
            }
            this.PlexsToSensorGroups = b3.ToImmutable();

            var b4 = ImmutableDictionary.CreateBuilder<ulong, InterviewerItem>();
            foreach (ulong key in checkerTriggerTemplate.PlexsToCollectorGroups.Keys)
            {
                b4.Add(key, new InterviewerItem(checkerTriggerTemplate.PlexsToCollectorGroups[key]));
            }
            this.PlexsToCollectorGroups = b4.ToImmutable();
        }
    }
}
