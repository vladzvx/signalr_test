using IASK.Common.Interfaces;
using System;
using UMKBRequests.Models.API.Semantic;

namespace IASK.Common.Services
{
    public class UrlFactory : IUrlFactory
    {
        public string GetUrl<T>()//EnvironmentType dtype)
        {
            Type type = typeof(T);            
            if (type == typeof(ResponseGetBelWhere))
            {
                return "https://cs.socmedica.com/api/umkb/GetBelWhere";
            }
            else if (type == typeof(ResultVarCharList))
            {
                return "https://cs.socmedica.com/api/umkb/getVarcharList";
            }
            else if (type == typeof(ResultSemanticNewBigId))
            {
                return "https://cs.socmedica.com/api/umkb/getsemanticnewbigid";
            }
            throw new NotImplementedException();
        }
    }
}
