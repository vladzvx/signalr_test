using IASK.DataStorage.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace IASK.DataStorage.Interfaces
{
    public interface ICommonWriter<T> where T:class
    {
        public void PutData(T data);

        public void PutData(MultipleDataWrapper<T> data);
    }
}
