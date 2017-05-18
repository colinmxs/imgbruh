using imgbruh.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace imgbruh.Models
{
    public class Img : Entity
    {
        #region constructors
        //private constructor for entity framework
        public Img() { }
        public Img(string url, string codeName, string artistName, string contentType, string fileName, string lookupId) : this()
        {

            Url = url;
            TimeCreatedUtc = DateTime.UtcNow;
            ArtistName = artistName;
            ContentType = contentType;
            FileName = fileName;
            LookupId = lookupId;
            CodeName = codeName;
        }
        #endregion
        #region properties
        public string Url
        {
            get; private set;
        }
        public string CodeName { get; private set; }
        public DateTime TimeCreatedUtc { get; private set; }        
        public string UserId { get; private set; }  
        public string ArtistName { get; private set; }  
        public string ContentType { get; private set; }   
        public string FileName { get; private set; }
        public string LookupId { get; private set; }
        #endregion        

        public static async Task<Img> CreateAsync(HttpPostedFileBase image, string codeName, string artistName, FileStorage fs, ImgbruhContext db)
        {
            var lookupId = Guid.NewGuid().ToString().Substring(0, 8);
            var url = await fs.UploadBlobAsync(image.InputStream, lookupId);
            var img = new Img(url, codeName, artistName, image.ContentType, image.FileName, lookupId);
            db.Imgs.Add(img);
            return img;
        }
    }
}