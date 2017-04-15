using Newtonsoft.Json;

namespace HedgehogSoft.TeamFortressOutpostApi.Models
{
    internal class OffsetResponse
    {
        [JsonProperty(PropertyName = "response")]
        internal OffsetParameters OffsetParameters { get; set; }
    }
}
