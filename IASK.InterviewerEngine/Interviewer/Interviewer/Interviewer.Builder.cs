using IASK.InterviewerEngine.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UMKBRequests;
using UMKBRequests.Models.API.Semantic;
using fieldsDescriptions = UMKBNeedStuff.Models.FieldsDescriptions;

namespace IASK.InterviewerEngine
{
    public partial class Interviewer
    {
        internal class Builder
        {
            #region fields
            internal InterviewerType Type { get;private set; } = InterviewerType.Questioning;
            internal bool OnePageMode = true;
            internal ulong Id { get; private set; }
            internal Dictionary<ulong, TriggerBuilder> Triggers;
            internal Dictionary<ulong, CheckerItemBuilder> ChekerTriggerIDs;
            internal Dictionary<ulong, List<Content>> Contents;
            internal Dictionary<ulong, ComplexDescription.Builder> Descriptions;
            internal Dictionary<ulong, Name.Builder> Names;

            #endregion

            public void SetType(ushort value)
            {
                if (Enum.IsDefined(typeof(InterviewerType), value))
                {
                    this.Type = (InterviewerType)value;
                }
                else if (Constants.PageLevels.Contains(value))
                {
                    this.Type = InterviewerType.SinglePage;
                }
                else throw new InvalidCastException("Can't cast interviwer root to InterviewerType!");
            }

            public Builder(ulong CheckerID)
            {
                this.Id = CheckerID;
                Triggers = new Dictionary<ulong, TriggerBuilder>();
                ChekerTriggerIDs = new Dictionary<ulong, CheckerItemBuilder>();
                Contents = new Dictionary<ulong, List<Content>>();
                Descriptions = new Dictionary<ulong, ComplexDescription.Builder>();
                Names = new Dictionary<ulong, Name.Builder>();
            }

            #region methods
            internal void AddContentIfNeed(ulong id, Content content, bool overwrite=false)
            {
                if (Contents.ContainsKey(id))
                {
                    if (Contents[id].FindIndex(item => item.Equals(content)) < 0)
                    {
                        if (overwrite)
                        {
                            Contents[id] = new List<Content>() { content };
                        }
                        else
                        {
                            Contents[id].Add(content);
                        }                           
                    }
                }
                else
                {
                    Contents.Add(id, new List<Content>() { content });
                }
            }
            internal bool TryAddCheckerTriggerIDs(CheckerItemBuilder eTItem, bool forceAdd = false)
            {
                if (this.Id == eTItem.MasterPlex.ida &&
                    !this.ChekerTriggerIDs.ContainsKey(eTItem.MasterPlex.idb) &&
                    eTItem.MasterPlex.value_a != null &&
                    Math.Round((double)eTItem.MasterPlex.value_a) >0)
                {
                    this.ChekerTriggerIDs.Add(eTItem.MasterPlex.idb, eTItem);
                    return true;
                }
                return false;
            }

            internal bool TryAddTriggerId(CheckerItemBuilder eTItem)
            {
                if (!this.ChekerTriggerIDs.ContainsKey(eTItem.MasterPlex.id))
                {
                    this.ChekerTriggerIDs.Add(eTItem.MasterPlex.id, eTItem);
                    return true;
                }
                return false;
            }

            internal void LoadContents(string GraphPass)
            {
                foreach (TriggerBuilder Trigger in Triggers.Values)
                {
                    foreach (CheckerItemBuilder sensor in Trigger.Sensors.Values)
                    {
                        if (sensor.MasterPlex.value_a != null)
                        {
                            int val = Interviewer.ConvertValueToCase((double)sensor.MasterPlex.value_a);
                            switch (val)
                            {
                                case 13:
                                    {
                                        GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
                                        RequestSemantic settingGraph = new RequestSemantic()
                                        {
                                            key = GraphPass,
                                            level = Constants.LevelToParent,
                                            route = Constants.RouteChild,
                                            lib = new List<ushort>() { IdParser.ParseNewBigId(sensor.MasterPlex.idb).lib },
                                            libid = new List<string>() { sensor.MasterPlex.idb.ToString() },
                                            deep = 5,
                                        };

                                        var res = getSemantic.Get(settingGraph);
                                        if (res.graph != null)
                                        {
                                            var ids = res.graph.Where(item => Constants.HospitalsLevel.Contains(item.levelb)).Select((item) => item.idb);
                                            foreach (var id in ids)
                                            {
                                                AddContentIfNeed(sensor.MasterPlex.idb,
                                                    new Content(Value: res.names[id], Id: id.ToString(), Type: Content.ContentType.concept));
                                            }
                                        }
                                        break;
                                    }
                                case 18:
                                case 8:
                                    {
                                        var parsed = IdParser.ParseNewBigId(sensor.MasterPlex.idb);
                                        if (parsed.lib == Constants.LibOfLibs)
                                        {
                                            AddContentIfNeed(sensor.MasterPlex.idb,
                                                new Content(Type: Content.ContentType.lib, Value: parsed.id.ToString()));
                                        }
                                        else
                                        {
                                            AddContentIfNeed(sensor.MasterPlex.idb,
                                                new Content(Type: Content.ContentType.lib, Value: parsed.lib.ToString()));
                                            if (!Constants.SearchLevelbExceptions.Contains(sensor.MasterPlex.levelb))
                                                AddContentIfNeed(sensor.MasterPlex.idb,
                                                    new Content(Type: Content.ContentType.level, Value: sensor.MasterPlex.levelb.ToString()));
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            internal bool TryAddDescription(ulong bigID, fieldsDescriptions fieldsDescriptions)
            {
                if (fieldsDescriptions == null)
                    return false;

                if (this.Descriptions.ContainsKey(bigID))
                {
                    this.Descriptions[bigID].TryAddDescriptions(fieldsDescriptions);
                }
                else
                {
                    ComplexDescription.Builder desc = new ComplexDescription.Builder();
                    desc.TryAddDescriptions(fieldsDescriptions);
                    this.Descriptions.Add(bigID, desc);
                }
                return false;
            }
            internal bool TryAddName(ulong NewBigID, UMKBRequests.Models.API.Satellite.Name name)
            {
                bool result = false;
                if (Names.ContainsKey(NewBigID))
                {
                    result = Names[NewBigID].TryAddName(name);
                }
                else
                {
                    Name.Builder builder = new Name.Builder();
                    if (builder.TryAddName(name))
                    {
                        Names.Add(NewBigID, builder);
                        result = true;
                    }
                }
                return result;
            }
            internal void AddRangesContent(ulong id, string value)
            {
                double start, end, step, def = default;
                start = end = step = def;
                char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
                value.Replace('.', separator);
                value.Replace(',', separator);

                var regex = new Regex(@"(?<=min\=)\w+(?=\;)");
                var match = regex.Matches(value);
                if (match.Count > 0)
                    double.TryParse(match[0].Value, out start);

                regex = new Regex(@"(?<=max\=)\w+(?=\;)");
                match = regex.Matches(value);
                if (match.Count > 0)
                    double.TryParse(match[0].Value, out end);

                regex = new Regex(@"(?<=step\=)\w+(?=\;)");
                match = regex.Matches(value);
                if (match.Count > 0)
                    double.TryParse(match[0].Value, out step);

                regex = new Regex(@"(?<=def\=)\w+(?=\;)");
                match = regex.Matches(value);
                if (match.Count > 0)
                    double.TryParse(match[0].Value, out _);

                regex = new Regex(@"(?<=unit\=)\w+(?=\;)");
                match = regex.Matches(value);
                if (match.Count > 0)
                    _ = match[0].Value;
                else
                {
                    regex = new Regex(@"(?<=unit\=)\w+");
                    match = regex.Matches(value);
                    if (match.Count > 0)
                        _ = match[0].Value;
                }

                if (start <= end && step > 0)
                {
                    List<double> values = new List<double>();
                    List<int> int_values = new List<int>();
                    double current = start;
                    while (current <= end)
                    {
                        values.Add(Math.Round(current, 3));
                        int_values.Add((int)Math.Round(current, 0));
                        current += step;
                    }
                    if (values.Count == int_values.Count)
                    {
                        foreach (var ival in int_values)
                        {
                            this.AddContentIfNeed(id, new Content(Value: ival.ToString(), Type: Content.ContentType.parameter));
                        }
                    }
                    else
                    {
                        foreach (var dval in values)
                        {
                            this.AddContentIfNeed(id, new Content(Value: dval.ToString(), Type: Content.ContentType.parameter));
                        }
                    }
                }

            }
            #endregion
        }
    }

}