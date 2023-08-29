using Newtonsoft.Json;

namespace Gov.Pssg.Css.Interfaces.DynamicsAutorest.Models
{
    public partial class MicrosoftDynamicsCRMcsuSubjectofcomplaint
    {

        [JsonProperty(PropertyName = "csu_Complaint@odata.bind")]
        public string ComplaintODataBind { get; set; }
    }
}
