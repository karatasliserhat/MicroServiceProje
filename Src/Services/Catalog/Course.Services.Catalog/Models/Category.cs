using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Course.Services.Catalog.Models
{
    public class Category:BaseModel
    {

        public string Name { get; set; }

    }
}
