 using System;

namespace imgbruh.Models
{
    public enum Emotion
    {
        Happy,
        Sad,
        Excited,
        Mad,
        Scared,
        Loving
    }

    public class Rating : Entity
    {
        private ApplicationUser _user;
        private string _userId;

        //private constructor for EF
        private Rating()
        {

        }

        public Rating(Emotion emotion, ApplicationUser user)
        {            
                //user            
                Emotion = emotion;
                TimeCreatedUtc = DateTime.UtcNow;
                User = user;
        }

        public Emotion Emotion { get; private set; }
        public DateTime TimeCreatedUtc {get;private set;}
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
        public void AddToImg(Img img)
        {
            img.AddRating(this);
        }           
    }
}