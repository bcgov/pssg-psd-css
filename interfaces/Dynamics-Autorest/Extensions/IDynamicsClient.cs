namespace Gov.Jag.Pssg.Csa.Interfaces
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using System;

    /// <summary>
    /// Auto Generated
    /// </summary>
    public partial interface IDynamicsClient : System.IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        System.Uri NativeBaseUri { get; set; }

        string GetEntityURI(string entityType, string id);
        

    }
}
