using fieldsDescriptions = UMKBNeedStuff.Models.FieldsDescriptions;

namespace IASK.InterviewerEngine
{
    internal class Description
    {
        public readonly ushort lib;
        public readonly int id;
        public readonly int master_id;
        public readonly string text;
        public readonly byte? weight;
        public readonly uint? source_id;

        public Description(fieldsDescriptions desc)
        {
            lib = desc.lib;
            id = desc.id;
            master_id = desc.master_id;
            text = desc.text;
            weight = desc.weight;
            source_id = desc.source_id;
        }
        public static implicit operator Description(fieldsDescriptions desc)
        {
            return new Description(desc);
        }
    }
}
