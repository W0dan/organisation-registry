// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace OrganisationRegistry.Import.Piavo.Models
{
    public partial class AddOrganisationOrganisationClassificationRequest
    {
        /// <summary>
        /// Initializes a new instance of the
        /// AddOrganisationOrganisationClassificationRequest class.
        /// </summary>
        public AddOrganisationOrganisationClassificationRequest() { }

        /// <summary>
        /// Initializes a new instance of the
        /// AddOrganisationOrganisationClassificationRequest class.
        /// </summary>
        public AddOrganisationOrganisationClassificationRequest(System.Guid? organisationOrganisationClassificationId = default(System.Guid?), System.Guid? organisationClassificationTypeId = default(System.Guid?), System.Guid? organisationClassificationId = default(System.Guid?), System.DateTime? validFrom = default(System.DateTime?), System.DateTime? validTo = default(System.DateTime?))
        {
            OrganisationOrganisationClassificationId = organisationOrganisationClassificationId;
            OrganisationClassificationTypeId = organisationClassificationTypeId;
            OrganisationClassificationId = organisationClassificationId;
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "organisationOrganisationClassificationId")]
        public System.Guid? OrganisationOrganisationClassificationId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "organisationClassificationTypeId")]
        public System.Guid? OrganisationClassificationTypeId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "organisationClassificationId")]
        public System.Guid? OrganisationClassificationId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "validFrom")]
        public System.DateTime? ValidFrom { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "validTo")]
        public System.DateTime? ValidTo { get; set; }

    }
}
