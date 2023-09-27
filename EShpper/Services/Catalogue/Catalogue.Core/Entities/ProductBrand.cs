using MongoDB.Bson.Serialization.Attributes;

namespace Catalogue.Core.Entities
{
    public class ProductBrand : BaseEntity
    {
        [BsonElement("Name")]
        public string Name { get; set; }
    }
}
