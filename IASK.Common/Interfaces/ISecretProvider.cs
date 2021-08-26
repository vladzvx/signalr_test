using IASK.InterviewerEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Common.Interfaces
{
    public interface ISecretProvider :IKeyProvider
    {
        public string ConnectionString1 { get; }
    }
}
