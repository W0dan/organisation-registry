// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace OrganisationRegistry.Import.Piavo.Models
{
    public partial class OrganisationParent
    {
        /// <summary>
        /// Initializes a new instance of the OrganisationParent class.
        /// </summary>
        public OrganisationParent() { }

        /// <summary>
        /// Initializes a new instance of the OrganisationParent class.
        /// </summary>
        public OrganisationParent(System.Guid? organisationOrganisationParentId = default(System.Guid?), System.Guid? parentOrganisationId = default(System.Guid?), string parentOrganisationName = default(string), Period validity = default(Period))
        {
            OrganisationOrganisationParentId = organisationOrganisationParentId;
            ParentOrganisationId = parentOrganisationId;
            ParentOrganisationName = parentOrganisationName;
            Validity = validity;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "organisationOrganisationParentId")]
        public System.Guid? OrganisationOrganisationParentId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "parentOrganisationId")]
        public System.Guid? ParentOrganisationId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "parentOrganisationName")]
        public string ParentOrganisationName { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "validity")]
        public Period Validity { get; set; }

    }
}
