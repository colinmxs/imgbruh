
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
        public Img(string url, string codeName, ApplicationUser user, string contentType, string fileName, string lookupId) : this()
        {

            Url = url;
            TimeCreatedUtc = DateTime.UtcNow;
            User = user;
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
        public virtual ApplicationUser User { get; private set; }  
        public string ContentType { get; private set; }   
        public string FileName { get; private set; }
        public string LookupId { get; private set; }
        #endregion        

        public async static Task CreateAsync(HttpPostedFileBase image, string codeName, ApplicationUser user, FileStorage fs, ImgbruhContext db)
        {
            var @event = new ImgCreated(fs, image, codeName, user, db);
            await ApplyAsync(@event);
            //var url = await fs.UploadBlobAsync(image.InputStream, lookupId);
            //var img = new Img(url, codeName, user, image.ContentType, image.FileName, lookupId);
            //db.Imgs.Add(img);
            //return img;
            
        }

        

        public async static Task ApplyAsync(AsyncEvent @event)
        {
            var handlers = @event._handlers;
            var tasks = new List<Task>();
            foreach (var handler in handlers)
            {
                tasks.Add(handler.ExecuteAsync());
            }

            await Task.WhenAll(tasks);
        }

        class ImgCreated : AsyncEvent
        {
            internal ImgCreated(FileStorage fs, HttpPostedFileBase image, string codeName, ApplicationUser user, ImgbruhContext db)
            {
                var lookupId = Guid.NewGuid().ToString().Substring(0, 8);
                var url = "http://127.0.0.1:10000/devstoreaccount1/default/" + lookupId;
                var img = new Img(url, codeName, user, image.ContentType, image.FileName, lookupId);
                _handlers.Add(new BlobUploader(fs, image.InputStream, lookupId));
                _handlers.Add(new DbInserter(db, img));
            }
            class DbInserter : AsyncHandler
            {
                private readonly ImgbruhContext _db;
                private readonly Img _img;

                internal DbInserter(ImgbruhContext db, Img img)
                {
                    _db = db;
                    _img = img;
                }

                public async override Task ExecuteAsync()
                {
                    _db.Imgs.Add(_img);
                    await _db.SaveChangesAsync();                    
                }
            }
            class BlobUploader : AsyncHandler
            {
                private readonly FileStorage _fs;
                private readonly Stream _stream;
                private readonly string _lookupId;


                internal BlobUploader(FileStorage fs, Stream stream, string lookupId)
                {
                    _fs = fs;
                    _stream = stream;
                    _lookupId = lookupId;
                }

                public async override Task ExecuteAsync()
                {
                    await _fs.UploadBlobAsync(_stream, _lookupId);
                }
            }
        }
    }

    public abstract class AsyncEvent
    {
        public readonly List<AsyncHandler> _handlers;
        public AsyncEvent()
        {
            _handlers = new List<AsyncHandler>();
        }
    }

    public abstract class AsyncHandler
    {
        public abstract Task ExecuteAsync();
    }

}