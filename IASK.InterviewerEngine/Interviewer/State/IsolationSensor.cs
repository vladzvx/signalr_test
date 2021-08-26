namespace IASK.InterviewerEngine
{
    public class IsolationSensor
    {
        internal readonly ulong Idb;//id сенсора триггер-сенсор
        internal ulong Id;//id ребра-связи триггер-сенсор
        internal ulong TriggerId;
        internal readonly ulong LinkId;//id ребра-связи чекер-триггер
        internal bool Required = false;
        internal readonly string Name;
        internal bool Forced = false;
        internal InterviewerItem sensor;
        internal bool IsTurnOn
        {
            get
            {
                if (this.ContainValueD || this.ContainValueB || this.ContainValueS)
                    return true;
                return false;
            }
        }

        internal bool ContainValueD { get; private set; } = false;
        private double valueD;
        internal double? ValueD
        {
            get
            {
                if (this.ContainValueD) return this.valueD;
                return null;
            }
            set
            {
                if (!this.ContainValueS && !this.ContainValueB && value != null)
                {
                    this.ContainValueD = true;
                    this.valueD = (double)value;
                }
            }
        }

        internal bool ContainValueS { get; private set; } = false;
        private string valueS;
        internal string ValueS
        {
            get
            {
                if (this.ContainValueS) return this.valueS;
                return null;
            }
            set
            {
                if (!this.ContainValueD && !this.ContainValueB && value != null)
                {
                    this.ContainValueS = true;
                    this.valueS = value;
                }
            }
        }
        internal bool ContainValueB { get; private set; } = false;
        private bool valueB;
        internal bool? ValueB
        {
            get
            {
                if (this.ContainValueB) return this.valueB;
                return null;
            }
            set
            {
                if (!this.ContainValueD && !this.ContainValueS && value != null)
                {
                    this.ContainValueB = true;
                    this.valueB = (bool)value;
                }
            }
        }

        internal IsolationSensor(ulong idb, ulong id, ulong Trigger_id, ulong link_id, string name, bool required)
        {
            this.Idb = idb;
            this.Id = id;
            this.TriggerId = Trigger_id;
            this.LinkId = link_id;
            this.Required = required;
            this.Name = name;
        }
    }
}
