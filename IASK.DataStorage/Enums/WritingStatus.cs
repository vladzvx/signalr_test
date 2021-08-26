using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.DataStorage
{
    public enum WritingStatus
    {
        Recieved = 0,
        Waiting = 1,
        Writing = 2,
        Writed = 3,
        Failed = 4
    }
}
