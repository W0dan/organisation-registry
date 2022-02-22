// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace OrganisationRegistry.Import.Piavo.Models
{
    public partial class BodyInfoResponse
    {
        /// <summary>
        /// Initializes a new instance of the BodyInfoResponse class.
        /// </summary>
        public BodyInfoResponse() { }

        /// <summary>
        /// Initializes a new instance of the BodyInfoResponse class.
        /// </summary>
        public BodyInfoResponse(System.Guid? id = default(System.Guid?), string bodyNumber = default(string), string name = default(string), string shortName = default(string), string description = default(string))
        {
            Id = id;
            BodyNumber = bodyNumber;
            Name = name;
            ShortName = shortName;
            Description = description;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public System.Guid? Id { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "bodyNumber")]
        public string BodyNumber { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "shortName")]
        public string ShortName { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "description")]
        public string Description { get; private set; }

    }
}
