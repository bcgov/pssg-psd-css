namespace Gov.Jag.Spice.Interfaces
{
    using Microsoft.Rest;
    using Models;
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

        string GetCreatedRecord(OdataerrorException odee, string errorMessage);

        
        Task<MicrosoftDynamicsCRMaccount> GetAccountById(Guid id);
        Task<MicrosoftDynamicsCRMcontact> GetContactById(Guid id);
        Task<MicrosoftDynamicsCRMincident> GetScreeningById(Guid id);
        MicrosoftDynamicsCRMcontact GetContactByExternalId(string externalId);

    }
}
