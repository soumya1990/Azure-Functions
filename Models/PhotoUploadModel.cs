using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Photos.Models
{
    public class PhotoUploadModel
    {

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("tags")]
        public String Tags { get; set; }

        [JsonProperty("photo")]
        public String Photo { get; set; }

    }
}
