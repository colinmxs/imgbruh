using System;

namespace imgbruh.Models
{
    public class Comment : Entity
    {
        private ApplicationUser _user;
        private string _userId;

        //private constructor for EF
        private Comment() { }

        public Comment(string message, ApplicationUser user)
        {
            if (user.IsAuthenticated)
            {
                Message = message;
                _user = user;
                TimeCreatedUtc = DateTime.UtcNow;
            }
            else
            {
                throw new Exception("User must be authenticated. Use user.Authenticate(IPrincipal princ) to set the proper flag.");
            }
        }

        public string Message { get; private set; }
        public string UserId { get { return _userId; } private set { _userId = value; } }
        public virtual ApplicationUser User
        {
            get { return _user; }
            set
            {
                if (value.IsAuthenticated)
                {
                    _user = value;
                    UserId = value.Id;
                }
                else
                {
                    throw new Exception("Only authenticated users are allowed to create Ratings. Use user.Authenticate(IPrincipal principal) to set the proper flag.");
                }
            }
        }
        public DateTime TimeCreatedUtc { get; private set; }
        public void AddToImg(Img img)
        {
            img.AddComment(this);
        }
    }
}