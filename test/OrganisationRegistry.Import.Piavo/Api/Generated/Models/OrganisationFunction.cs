// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace OrganisationRegistry.Import.Piavo.Models
{
    public partial class OrganisationFunction
    {
        /// <summary>
        /// Initializes a new instance of the OrganisationFunction class.
        /// </summary>
        public OrganisationFunction() { }

        /// <summary>
        /// Initializes a new instance of the OrganisationFunction class.
        /// </summary>
        public OrganisationFunction(System.Guid? organisationFunctionId = default(System.Guid?), System.Guid? functionId = default(System.Guid?), string functionName = default(string), System.Guid? personId = default(System.Guid?), string personName = default(string), System.Collections.Generic.IList<Contact> contacts = default(System.Collections.Generic.IList<Contact>), Period validity = default(Period))
        {
            OrganisationFunctionId = organisationFunctionId;
            FunctionId = functionId;
            FunctionName = functionName;
            PersonId = personId;
            PersonName = personName;
            Contacts = contacts;
            Validity = validity;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "organisationFunctionId")]
        public System.Guid? OrganisationFunctionId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "functionId")]
        public System.Guid? FunctionId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "functionName")]
        public string FunctionName { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "personId")]
        public System.Guid? PersonId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "personName")]
        public string PersonName { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "contacts")]
        public System.Collections.Generic.IList<Contact> Contacts { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "validity")]
        public Period Validity { get; set; }

    }
}
