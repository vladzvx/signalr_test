using IASK.Common.Models;
using IASK.InterviewerEngine.Models.Output;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UMKBNeedStuff.DataContainer.Billing;
using UMKBRequests;
using UMKBRequests.Models.API.Semantic;

namespace IASK.Common
{

    public static class UMKBWorker
    {
        private static HttpClient httpClient = new HttpClient();
        public static bool TryGetSemantic(ulong id,string key, out InterfaceUnit interfaceUnit)
        {
            interfaceUnit = new InterfaceUnit
            {
                Type = InterfaceUnit.UnitType.LIST
            };
            RequestSemantic rs = new RequestSemantic()
            {
                libid = new List<string>() { id.ToString() },
                deep = 1,
                key = key,
                lib = new List<ushort>() { 25 },
                level = new List<ushort>() { 1102 },
                route = 2
            };
            GetSemanticNewBigId getSemanticNewBigId11 = new GetSemanticNewBigId();
            var q = getSemanticNewBigId11.Get(rs);
            if (q.alerts.code == 200)
            {
                foreach (PlexNewBigId plex in q.graph)
                {
                    if (plex.deep == 0) continue;
                    if (q.names.TryGetValue(plex.id, out string name))
                    {
                        InterfaceUnit iu = new InterfaceUnit();
                        iu.Type = InterfaceUnit.UnitType.TEXT;
                        iu.Id = plex.id.ToString();
                        iu.Idb = plex.idb.ToString();
                        iu.Label = name;
                        interfaceUnit.Units.Add(iu);
                    }
                }
                return true;
            }
            else return false;
            
        }

        public static bool TryGetSemantic(List<ulong> ids,List<ushort> libs, List<ushort> levels, string key,short route, out InterfaceUnit interfaceUnit)
        {
            interfaceUnit = new InterfaceUnit
            {
                Type = InterfaceUnit.UnitType.LIST
            };
            RequestSemantic rs = new RequestSemantic()
            {
                libid = ids.Select((id)=>id.ToString()),
                deep = 1,
                key = key,
                lib = libs,
                level = levels,
                route = route
            };
            GetSemanticNewBigId getSemanticNewBigId11 = new GetSemanticNewBigId();
            var q = getSemanticNewBigId11.Get(rs);
            if (q.alerts.code == 200)
            {
                foreach (PlexNewBigId plex in q.graph)
                {
                    if (plex.deep == 0) continue;
                    if (q.names.TryGetValue(plex.id, out string name))
                    {
                        InterfaceUnit iu = new InterfaceUnit();
                        iu.Type = InterfaceUnit.UnitType.TEXT;
                        iu.Id = plex.id.ToString();
                        iu.Idb = plex.idb.ToString();
                        iu.Label = name;
                        interfaceUnit.Units.Add(iu);
                    }
                }
                return true;
            }
            else return false;

        }

        public static async Task<int> TryGetUserId(BaseModel baseModel)
        {
            AuthGetUsertId auth = new AuthGetUsertId();
            UMKBRequests.Models.API.Auth.CheckAuthGetUserId userauth = auth.Get(new UMKBRequests.Models.API.Semantic.RequestAuth()
            {
                authkey = baseModel.Permit.authkey,
                gateway = null,
                login = null,
                application = "0"
            });
            if (userauth.alert.code == 200)
                return userauth.userid;
            else
            {
                throw new System.Exception("Auth failed!");
            }
        }

        public static async Task<AuthResult> TryGetUserId(string authKey)
        {
            AuthGetUsertId auth = new AuthGetUsertId();
            UMKBRequests.Models.API.Auth.CheckAuthGetUserId userauth = auth.Get(new UMKBRequests.Models.API.Semantic.RequestAuth()
            {
                authkey = authKey,
                gateway = null,
                login = null,
                application = "0"
            });
            if (userauth.alert.code == 200)
                return new AuthResult() { UserId = userauth.userid, Success = true };
            else
            {
                return new AuthResult() { UserId = -1, Success = false };
            }
        }

        public static async Task<List<ulong>> GetSimpleSemanticAsync(ulong startId, ushort lib, ushort level, string key, short route)
        {
            List<ulong> forReturn = new List<ulong>();
            RequestSemantic rs = new RequestSemantic()
            {
                libid = new string[1] {startId.ToString() },
                deep = 1,
                key = key,
                lib = new ushort[1] { lib },
                level = new ushort[1] { level },
                route = route
            };

            StringContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(rs), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://cs.socmedica.com/api/umkb/getsemanticnewbigid", content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string result = await response.Content.ReadAsStringAsync();
                ResultSemanticNewBigId res = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultSemanticNewBigId>(result);
                if (res.alerts.code == 200)
                {
                    foreach (PlexNewBigId plex in res.graph)
                    {
                        if (plex.deep == 0) continue;
                        forReturn.Add(plex.idb);
                    }
                    return forReturn;
                }
                throw new System.Exception(Newtonsoft.Json.JsonConvert.SerializeObject(res.alerts));
            }
            throw new System.Exception(response.StatusCode.ToString());

        }


        public static async Task<bool> TryAuth(BaseModel  baseModel)
        {
            return true;
        }


        public static List<Content> Search(string text, List<string> levels, string libs, out UMKBRequests.Models.API.Semantic.Alert alert)
        {
            List<Content> result = new List<Content>();
            UMKBRequests.GetSearch getSearch = new GetSearch();
            ResultSearch resultSearch = getSearch.Get(new RequestSearch()
            {
                key = IASK.Common.Secrets.SphinxSearch,
                text = text,
                chain = 0,
                lib = libs
            });
            alert = resultSearch.alert;
            if (resultSearch.concepts != null)
            {
                foreach (var res in resultSearch.concepts)
                {
                    if (levels != null && levels.Count > 0)
                    {
                        if (levels.Contains(res.idlevel.ToString()))
                        {
                            result.Add(new Content(Id: IdParser.ParseBigIdToNewBigId(res.id).ToString(), Value: res.name, Type: Content.ContentType.search_result));
                        }
                    }
                    else
                    {
                        result.Add(new Content(Id: IdParser.ParseBigIdToNewBigId(res.id).ToString(), Value: res.name, Type: Content.ContentType.search_result));
                    }
                }
            }
            return result;
        }
        public static bool TryAuth(BaseModel container, string service)
        {
            AuthGetUsertId auth = new AuthGetUsertId();
            UMKBRequests.Models.API.Auth.CheckAuthGetUserId userauth = auth.Get(new UMKBRequests.Models.API.Semantic.RequestAuth()
            {
                authkey = container.Permit.authkey,
                gateway = null,
                login = null,
                application = service
            });

            container.SetAlert(userauth.alert);
            return userauth.alert.code == 200;
        }
        public static bool TryAuth(string authkey, string service, out UMKBRequests.Alert alert)
        {
            alert = new UMKBRequests.Alert();
            alert.Ok();
            AuthGetUsertId auth = new AuthGetUsertId();
            UMKBRequests.Models.API.Auth.CheckAuthGetUserId userauth = auth.Get(new UMKBRequests.Models.API.Semantic.RequestAuth()
            {
                authkey = authkey,
                gateway = null,
                login = null,
                application = service
            });
            if (userauth.alert.code == 200)
                return true;
            else
            {
                alert = userauth.alert;
                return false;
            }
        }
    }
}
