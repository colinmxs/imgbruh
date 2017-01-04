using imgbruh.Infrastructure;
using imgbruh.Models.NameGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace imgbruh.Models
{
    public class Img : Entity
    {
        #region fields
        private ICollection<Rating> _ratings;
        private ICollection<Comment> _comments;
        private string _url;
        private ApplicationUser _user;
        private string _userId;
        #endregion
        #region constructors
        //private constructor for entity framework
        private Img() { }
        private Img(string url, string codeName, ApplicationUser user, string contentType, string fileName, string lookupId)
        {
            Url = url;
            TimeCreatedUtc = DateTime.UtcNow;
            User = user;
            ContentType = contentType;
            FileName = fileName;
            LookupId = lookupId;
            CodeName = codeName;
        }
        
        public async static Task<Img> CreateAsync(HttpPostedFileBase image, string codeName, ApplicationUser user, FileStorage fs, imgbruhContext db)
        {
            var lookupId = Guid.NewGuid().ToString().Substring(0, 10);
            if (!user.IsAuthenticated)
            {
                throw new Exception("Img can only be create by authenticated user. Use user.Authenticate(IPrincipal p) to set the correct flag");
            }
            var url = await fs.UploadBlobAsync(image.InputStream, lookupId);
            var img = new Img(url, codeName, user, image.ContentType, image.FileName, lookupId);
            db.Imgs.Add(img);
            return img;
        }
        #endregion
        #region properties
        public string Url
        {
            get
            {
                return _url;
            }
            private set
            {
                if (Uri.IsWellFormedUriString(value, UriKind.Absolute) && (value.EndsWith(".gif") || value.EndsWith(".gifv") || value.EndsWith(".png") || value.EndsWith(".jpg")))
                    _url = value;
                else
                    throw new Exception("Invalid Url string: " + value);
            }
        }
        public string CodeName { get; private set; }
        public DateTime TimeCreatedUtc { get; private set; }
        public virtual ICollection<Rating> Ratings
        {
            get
            {
                return this._ratings ?? (this._ratings = new HashSet<Rating>());
            }
        }
        public virtual ICollection<Comment> Comments
        {
            get
            {
                return this._comments ?? (this._comments = new HashSet<Comment>());
            }
        }
        public string UserId { get { return _userId; } private set { _userId = value; } }  
        public virtual ApplicationUser User { get { return _user; } private set
            {
                UserId = value.Id;
                _user = value;
            }
        }  
        public string ContentType { get; private set; }   
        public string FileName { get; private set; }
        public string LookupId { get; private set; }

        #endregion
        #region methods
        public void AddRating(Rating rating)
        {
            this._ratings.Add(rating);
        }
        public void AddComment(Comment comment)
        {
            this._comments.Add(comment);
        }        
        #endregion
    }
}