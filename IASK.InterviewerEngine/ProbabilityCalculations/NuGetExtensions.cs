using UMKBRequests.Models.API.Semantic;

namespace IASK.InterviewerEngine
{
    internal static class PlexNewBigExtension
    {
        public static PlexNewBigId Copy(this PlexNewBigId PlexNewBigId)
        {
            return new PlexNewBigId()
            {
                id = PlexNewBigId.id,//
                parent_id = PlexNewBigId.parent_id,//
                deep = PlexNewBigId.deep,//
                ida = PlexNewBigId.ida,//
                idb = PlexNewBigId.idb,//
                level = PlexNewBigId.level,
                levelb = PlexNewBigId.levelb,
                route = PlexNewBigId.route,
                sort = PlexNewBigId.sort,
                type = PlexNewBigId.type,
                value_a = PlexNewBigId.value_a,
                value_b = PlexNewBigId.value_b,
                value_c = PlexNewBigId.value_c,
                value_d = PlexNewBigId.value_d
            };
        }


    }

}
