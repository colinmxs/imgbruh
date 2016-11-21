using imgbruh.Models.NameGeneration;
using System;
using System.Collections.Generic;

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
        public Img(string url, string name, ApplicationUser user)
        {
            Url = url;
            TimeCreatedUtc = DateTime.UtcNow;
            CodeName = name;
            if (user.IsAuthenticated)
            {
                User = user;
            }else
            {
                throw new Exception("Img can only be create by authenticated user. Use user.Authenticate(IPrincipal p) to set the correct flag");
            }
        }
        #endregion
        #region properties
        public string CodeName { get; private set; }
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
        public void ChangeCodeName(NameGenerator nameGenerator)
        {
            CodeName = nameGenerator.Generate();
        }
        #endregion
    }
}