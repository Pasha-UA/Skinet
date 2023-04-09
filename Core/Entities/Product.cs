using Core.Entities.PriceListAggregate;
using Core.Interfaces;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string ExternalId { get; set; } // id in external db. used for import
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } // TODO: change type to 'Price'
        // public string PriceItemId { get; set; }
        // private readonly List<PriceItem> _prices = new List<PriceItem>();
        // public virtual IList<ProductPrice> ProductsPrices {get; set;}
        public IList<Price> Prices {get; set;}

        // public PriceItem BulkPrice { get; set; }
        public ProductType ProductType { get; set; }
        public string ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public string ProductBrandId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ProductCategoryId { get; set; }
        private readonly List<Photo> _photos = new List<Photo>();
        public IReadOnlyList<Photo> Photos => _photos.AsReadOnly();
        public int Stock { get; set; } = 0;
        public string BarCode { get; set; }
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false; // product is deleted, db to remove it after some term

        public void AddPhoto(string pictureUrl, string fileName, bool isMain = false)
        {
            var photo = new Photo
            {
                FileName = fileName,
                PictureUrl = pictureUrl
            };

            if (_photos.Count == 0) photo.IsMain = true;

            _photos.Add(photo);
        }

        public void RemovePhoto(string id)
        {
            var photo = _photos.Find(x => x.Id == id);
            _photos.Remove(photo);
        }

        public void SetMainPhoto(string id)
        {
            var currentMain = _photos.SingleOrDefault(item => item.IsMain);
            foreach (var item in _photos.Where(item => item.IsMain))
            {
                item.IsMain = false;
            }

            var photo = _photos.Find(x => x.Id == id);
            if (photo != null)
            {
                photo.IsMain = true;
                if (currentMain != null) currentMain.IsMain = false;
            }
        }

        // new properties
        //        public string ShortDescription { get; set; }
        //        public string BasicUnit { get; set; }
        //        public List<AdditionalPropertie> AdditionalProperties {get;set;}

        public static bool operator ==(Product a, Product b)
        {
            if ((object)a == null || (object)b == null)
                return false;

            return (string.Compare(a.Name, b.Name) == 0
                && string.Compare(a.ProductCategoryId, b.ProductCategoryId) == 0);
        }

        public static bool operator !=(Product a, Product b)
        {
            if ((object)a == null || (object)b == null)
                return false;

            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            var other = (Product)obj;

            return (
                string.Compare(this.Name, other.Name) == 0
                && string.Compare(this.ProductCategoryId, other.ProductCategoryId) == 0
                && string.Compare(this.Id, other.Id) == 0
            );
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }



    }
}