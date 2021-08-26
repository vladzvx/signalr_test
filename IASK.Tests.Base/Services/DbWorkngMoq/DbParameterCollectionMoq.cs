using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace IASK.Tests.Base.Services.DbWorkngMoq
{
    public class DbParameterCollectionMoq : DbParameterCollection
    {
        private Dictionary<string, DbParameter> parameters = new Dictionary<string, DbParameter>();
        public new DbParameter this[string name]
        {
            get => parameters[name];
            set => parameters.Add(name,value);
        }
        public override int Count => throw new NotImplementedException();

        public override object SyncRoot => throw new NotImplementedException();

        public override int Add(object value)
        {
            if (value is DbParameter p)
            {
                parameters.Add(p.ParameterName,p);
                return 1;
            }
            return 0;
        }

        public override void AddRange(Array values)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(string value)
        {
            return parameters.ContainsKey(value);
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(int index)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return parameters[parameterName];
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            throw new NotImplementedException();
        }
    }
}
