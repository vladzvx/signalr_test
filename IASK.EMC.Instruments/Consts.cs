using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using UMKBRequests;

namespace IASK.EMC.Instruments
{
    public static class Consts
    {
        internal static readonly ImmutableList<ushort> EMCHeadersLibs = ImmutableList.CreateRange(new ushort[] { 81});
        internal static readonly ImmutableList<string> GroupsRepoStarts = ImmutableList.CreateRange(new string[] { IdParser.GetNewBigId(86,9790).ToString(), IdParser.GetNewBigId(86, 9781).ToString() });
    }
}
