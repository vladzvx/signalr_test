using IASK.DataStorage.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.DataStorage.Services.Implementations
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string SequenceName1 => "protocolscache_id_seq";
    }
}
