using EMCCore.Interfaces;
using EMCCore.Models;
using EMCCore.Models.Protocol;
using IASK.Common.Services;
using IASK.DataStorage.Interfaces;
using IASK.InterviewerEngine.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IASK.EMC.Instruments
{
    public class InterfaceUnitConverter : IEMCDataConverter<InterfaceUnit>
    {
        private readonly NamesCache namesCache;
        private double ArchivingPeriodMinuts;
        public InterfaceUnitConverter(NamesCache namesCache, IDataBaseSettings settings)
        {
            this.namesCache = namesCache;
            this.ArchivingPeriodMinuts = settings.ArchivingPeriodMinuts;
        }

        [Obsolete("Not implemeted! Throws NotImplementedException")]
        public bool TryConvert(IEnumerable<InterfaceUnit> data, out IEnumerable<EMCProtoM> protocols)
        {
            throw new NotImplementedException();
        }

        
        public bool TryConvert(IEnumerable<EMCProtoM> protocols, out IEnumerable<InterfaceUnit> data)
        {
            try
            {
                data = ConvertProtocols(protocols.ToList());
                return true;
            }
            catch (Exception ex)
            {
                data = null;
                return false;
            }
        }
        public List<InterfaceUnit> ConvertProtocols(List<EMCProtoM> protcols)
        {
            List<InterfaceUnit> result = new List<InterfaceUnit>();
            foreach (EMCProtoM protocol in protcols)
            {
                bool isEmpty = false;
                InterfaceUnit prot = new InterfaceUnit();
                if (protocol.header != "0")
                {
                    prot.Type = InterfaceUnit.UnitType.PROTOCOL;
                }
                else
                {
                    prot.Type = InterfaceUnit.UnitType.HEAD_PROTOCOL;
                }
                result.Add(prot);
                if (namesCache.TryGetNames(new List<ulong>() { protocol.id }, out List<string> data))
                {
                    prot.Label = data[0];
                    prot.Id = protocol._id;
                    prot.Idb = protocol.id.ToString();
                }
                if (protocol.subjects != null)
                {
                    foreach (EMCSubject sub in protocol.subjects)
                    {
                        ConvertSubject(sub, prot);
                    }
                }
                else
                {
                    isEmpty = true;
                }


                prot.Content = new List<Content>();
                prot.Content.Add(new Content(null, null, protocol.time.ToString(), null, Content.ContentType.timestamp));
                prot.Content.Add(new Content(null, null, "Доктор Иванов", null, Content.ContentType.doctor));

                if (DateTime.UtcNow.Subtract(protocol.time).TotalMinutes < ArchivingPeriodMinuts)
                {
                    prot.Content.Add(new Content(null, null, "true", null, Content.ContentType.mutable));
                }
                else
                {
                    prot.Content.Add(new Content(null, null, "false", null, Content.ContentType.mutable));
                }
                if (protocol.values != null && protocol.values.Count > 0)
                {
                    foreach (var t in protocol.values.Values)
                    {
                        if (t != null)
                            foreach (var t2 in t)
                                prot.Content.Add(new Content(null, null, t2, null, Content.ContentType.text));
                    }
                    isEmpty = false;
                }
                else
                {
                    isEmpty = isEmpty && true;
                }
            }
            return result;
        }

        public InterfaceUnit ConvertProtocol(EMCProtoM protocol)
        {
            List<InterfaceUnit> result = new List<InterfaceUnit>();
            bool isEmpty = false;
            InterfaceUnit prot = new InterfaceUnit();
            prot.Type = InterfaceUnit.UnitType.PROTOCOL;
            result.Add(prot);
            if (namesCache.TryGetNames(new List<ulong>() { protocol.id }, out List<string> data))
            {
                prot.Label = data[0];
                prot.Id = protocol._id;
                prot.Idb = protocol.id.ToString();
            }
            if (protocol.subjects != null)
            {
                foreach (EMCSubject sub in protocol.subjects)
                {
                    ConvertSubject(sub, prot);
                }
            }
            else
            {
                isEmpty = true;
            }
            prot.Content = new List<Content>();
            prot.Content.Add(new Content(null, null, protocol.time.ToString(), null, Content.ContentType.timestamp));
            //prot.Content.Add(new Content(null, null, protocol.time.ToString(), null, Content.ContentType.));
            if (protocol.values != null && protocol.values.Count > 0)
            {
                foreach (var t in protocol.values.Values)
                {
                    if (t != null)
                        foreach (var t2 in t)
                            prot.Content.Add(new Content(null, null, t2, null, Content.ContentType.text));
                }
                isEmpty = false;
            }
            else
            {
                isEmpty = isEmpty && true;
            }
            
            return prot;
        }
        private void ConvertSubject(EMCSubject subject, InterfaceUnit wrapper)
        {
            InterfaceUnit result = new InterfaceUnit();
            wrapper.Units.Add(result);
            result.Type = InterfaceUnit.UnitType.PROTOCOL_DATA;
            if (namesCache.TryGetNames(new List<ulong>() { subject.id }, out List<string> data))
            {
                result.Label = data[0];
                result.Idb = subject.id.ToString();
            }
            if (subject.valueTable != null)
                ConvertTabValues(subject.valueTable, result);
            if (subject.facts != null)
            {
                foreach (EMCFact sfact in subject.facts)
                {
                    ConvertFact(sfact, result);
                }
            }
        }
        private void ConvertFact(EMCFact fact, InterfaceUnit wrapper)
        {

            InterfaceUnit result = new InterfaceUnit();
            wrapper.Units.Add(result);
            result.Type = InterfaceUnit.UnitType.PROTOCOL_DATA;
            if (namesCache.TryGetNames(new List<ulong>() { fact.id }, out List<string> data))
            {
                result.Idb = fact.id.ToString();
                result.Label = data[0];
            }
            if (fact.valueTable != null)
                ConvertTabValues(fact.valueTable, result);
            if (fact.subFacts != null)
            {
                foreach (EMCFact sfact in fact.subFacts)
                {
                    ConvertFact(sfact, result);
                }
            }
        }
        private void ConvertTabValues(List<EMCValTab> tabs, InterfaceUnit wrapper)
        {
            string label = string.Empty;
            string label2 = string.Empty;
            if (tabs != null && tabs.Count == 1)
            {
                if (tabs[0].value_1 != null)
                {
                    label = tabs[0].value_1.ToString();
                }
                if (tabs[0].unit_1 != null)
                {
                    if (namesCache.TryGetNames(new List<ulong>() { (ulong)tabs[0].unit_1 }, out List<string> data))
                    {
                        label += " " + data[0];
                    }
                }

                if (tabs[0].value_2 != null)
                {
                    label2 = tabs[0].value_2.ToString();
                }
                if (tabs[0].unit_2 != null)
                {
                    if (namesCache.TryGetNames(new List<ulong>() { (ulong)tabs[0].unit_2 }, out List<string> data))
                    {
                        label2 += " " + data[0];
                    }
                }

                Content cont = new Content(null, null, label, label2, Content.ContentType.data);
                if (wrapper.Content == null) wrapper.Content = new List<Content>() { cont };
                else wrapper.Content.Add(cont);
            }
        }
    }
}
