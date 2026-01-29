using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WH_Panel
{
    internal class PartApiResponse
    {
        [JsonProperty("@odata.context")]
        public string Context { get; set; }

        [JsonProperty("value")]
        public List<PartEntry> value { get; set; }
    }

    internal class PartEntry
    {
        [JsonProperty("PARTNAME")]
        public string PARTNAME { get; set; }

        // This matches the $expand=PARTMNF_SUBFORM in your URL
        [JsonProperty("PARTMNF_SUBFORM")]
        public List<PartMnfSubform> PARTMNF_SUBFORM { get; set; }
    }

    internal class PartMnfSubform
    {
        [JsonProperty("MNFPARTNAME")]
        public string MNFPARTNAME { get; set; }

        [JsonProperty("MNFPARTDES")]
        public string MNFPARTDES { get; set; }

        [JsonProperty("MNFNAME")]
        public string MNFNAME { get; set; }
    }
}