// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace OrganisationRegistry.Import.Piavo.Models
{
    public partial class BuildingOrganisation
    {
        /// <summary>
        /// Initializes a new instance of the BuildingOrganisation class.
        /// </summary>
        public BuildingOrganisation() { }

        /// <summary>
        /// Initializes a new instance of the BuildingOrganisation class.
        /// </summary>
        public BuildingOrganisation(System.Guid? parentOrganisationId = default(System.Guid?), string parentOrganisationName = default(string), System.Guid? organisationId = default(System.Guid?), string organisationName = default(string), string organisationShortName = default(string), string organisationOvoNumber = default(string), string dataVlaanderenOrganisationUri = default(string), string legalForm = default(string), string policyDomain = default(string), string responsibleMinister = default(string))
        {
            ParentOrganisationId = parentOrganisationId;
            ParentOrganisationName = parentOrganisationName;
            OrganisationId = organisationId;
            OrganisationName = organisationName;
            OrganisationShortName = organisationShortName;
            OrganisationOvoNumber = organisationOvoNumber;
            DataVlaanderenOrganisationUri = dataVlaanderenOrganisationUri;
            LegalForm = legalForm;
            PolicyDomain = policyDomain;
            ResponsibleMinister = responsibleMinister;
        }

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
        [Newtonsoft.Json.JsonProperty(PropertyName = "organisationId")]
        public System.Guid? OrganisationId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "organisationName")]
        public string OrganisationName { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "organisationShortName")]
        public string OrganisationShortName { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "organisationOvoNumber")]
        public string OrganisationOvoNumber { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "dataVlaanderenOrganisationUri")]
        public string DataVlaanderenOrganisationUri { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "legalForm")]
        public string LegalForm { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "policyDomain")]
        public string PolicyDomain { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "responsibleMinister")]
        public string ResponsibleMinister { get; set; }

    }
}
