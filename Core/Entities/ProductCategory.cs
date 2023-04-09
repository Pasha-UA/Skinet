using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public Photo Photo { get; set; }

        public void AddPhoto(string pictureUrl, string fileName, bool isMain = false)
        {
            var photo = new Photo
            {
                FileName = fileName,
                PictureUrl = pictureUrl
            };

            this.Photo = photo;
        }

        public void RemovePhoto(string id)
        {
            this.Photo = null;
        }

    }
}