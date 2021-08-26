using IASK.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.DataStorage.Interfaces
{
    public interface IDataBaseSettings
    {
        public TimeSpan ReconnectionPause { get => new TimeSpan(0, 1, 0); }
        public double StartWritingInterval { get => 5000; }
        public double PoolingTimeout { get => 30000; }
        public double DBPeriodicAction { get => 60000; }
        public string ConnectionString1 { get => Secrets.ConnectionString1; }
        public int TrasactionSize { get => 100000; }
        public int ConnectionPoolMaxSize { get => 70; }
        public int ConnectionPoolHotReserve { get => 2; }
        public int IdPoolMaxSize { get => 200; }
        public int IdPoolMinSize { get => 100; }
        public double ArchivingPeriodMinuts { get => 30; }
        public string SequenceName1 { get ; }
    }
}
