using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venly.Model
{
    public partial class NonFungible
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("backgroundColor")]
        public object BackgroundColor { get; set; }

        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("imagePreviewUrl")]
        public Uri ImagePreviewUrl { get; set; }

        [JsonProperty("imageThumbnailUrl")]
        public Uri ImageThumbnailUrl { get; set; }

        [JsonProperty("animationUrl")]
        public object AnimationUrl { get; set; }

        [JsonProperty("animationUrls")]
        public List<object> AnimationUrls { get; set; }

        [JsonProperty("fungible")]
        public bool Fungible { get; set; }

        [JsonProperty("maxSupply")]
        public long? MaxSupply { get; set; }

        [JsonProperty("contract")]
        public Contract Contract { get; set; }

        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }

        [JsonProperty("balance")]
        public long Balance { get; set; }
    }

    public partial class Attribute
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("displayType")]
        public object DisplayType { get; set; }

        [JsonProperty("traitCount")]
        public object TraitCount { get; set; }

        [JsonProperty("maxValue")]
        public object MaxValue { get; set; }
    }

    public partial class Contract
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("media")]
        public List<Media> Media { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("premium")]
        public bool Premium { get; set; }

        [JsonProperty("categories")]
        public List<object> Categories { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public Uri Value { get; set; }
    }
}
