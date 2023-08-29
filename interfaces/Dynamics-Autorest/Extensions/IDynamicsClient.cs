namespace Gov.Pssg.Css.Interfaces.DynamicsAutorest
{
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
