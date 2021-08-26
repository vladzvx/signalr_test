using EMCCore.Enums;
using EMCCore.Interfaces;
using EMCCore.Models;
using IASK.DataStorage.Interfaces;
using IASK.DataStorage.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IASK.EMC.Core.Main
{
    public class EMCCommonWriter<T> : IEMCCommonWriter<T> where T: class
    {
        public readonly ICommonWriter<T> commonWriter;

        public EMCCommonWriter(ICommonWriter<T> commonWriter)
        {
            this.commonWriter = commonWriter;
        }
        public async Task<EMCWritingStatus> Write(IEnumerable<T> data)
        {
            MultipleDataWrapper<T> multipleDataWrapper = new MultipleDataWrapper<T>(data);
            commonWriter.PutData(multipleDataWrapper);
            var status = await multipleDataWrapper.waiting;
            WritingStatus writingStatus = ConvertStatus(status);
            return new EMCWritingStatus(new string[1] { string.Empty }, writingStatus);
        }

        private static WritingStatus ConvertStatus(DataStorage.WritingStatus status)
        {
            if (status == DataStorage.WritingStatus.Failed)
            {
                return WritingStatus.Failed; 
            }
            else if (status == DataStorage.WritingStatus.Writed)
            {
                return WritingStatus.Successful;
            }
            throw new InvalidCastException("Cannot convert DataStorage.WritingStatus to EMCCore.Enums.WritingStatus!");
        }
    }
}
