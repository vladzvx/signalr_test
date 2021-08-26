using IASK.Common.Models;
using IASK.DataHub.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IASK.DataHub.Interfaces
{
    public interface IAuthProvider<T> where T : BaseMessage
    {
        public Task<bool> TryAuth(BaseModel baseModel);
    }
}
