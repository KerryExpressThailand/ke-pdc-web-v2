using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace KE_PDC.Services
{
    public class LineNotify
    {
        public virtual string Token { get; set; }
        private object _ratelimit = new {
            request = new {
                limit = 0,
                remain = 0
            },
            image = new {
                limit = 0,
                remain = 0
            },
            reset = ""
        };

        public LineNotify(string token)
        {
            this.Token = token;
        }

        private HttpResponseMessage Request(string endpoint)
        {
            byte[] _imageFile = null;

            LineSticker sticker = new LineSticker
            {
                StickerPackageId = 0,
                StickerId = 0
            };
            return Request(endpoint, null, _imageFile, sticker);
        }

        private HttpResponseMessage Request(string endpoint, string message)
        {
            byte[] _imageFile = null;

            LineSticker sticker = new LineSticker
            {
                StickerPackageId = 0,
                StickerId = 0
            };
            return Request(endpoint, message, _imageFile, sticker);
        }

        private HttpResponseMessage Request(string endpoint, string message, byte[] imageFile, LineSticker sticker)
        {
            string _endpoint = $"https://notify-api.line.me/api/{endpoint}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.Token}");

                if (endpoint.Equals("status"))
                {
                    MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                }

                //StringContent contentData = new StringContent(message, Encoding.UTF8, "application/x-www-form-urlencoded");

                FormUrlEncodedContent contentData = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("", "login")
                });
                
                var requestContent = new MultipartFormDataContent();
                //    here you can specify boundary if you need---^
                var imageContent = new ByteArrayContent(imageFile);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                requestContent.Add(imageContent, "imageFile", "image.jpg");

                return endpoint.Equals("status")
                    ? client.GetAsync(_endpoint).Result
                    : client.PostAsync(_endpoint, contentData).Result;
            }
        }

        public HttpResponseMessage Status()
        {
            return Request("status");
        }

        public HttpResponseMessage Revoke()
        {
            return Request("revoke");
        }

        public HttpResponseMessage Notify(string message)
        {
            return Request("notify", message);
        }

        public HttpResponseMessage Notify(string message, int stickerPackageId, int stickerId)
        {
            LineSticker sticker = new LineSticker {
                StickerPackageId = stickerPackageId,
                StickerId = stickerId
            };
            return Notify(message, sticker);
        }

        public HttpResponseMessage Notify(string message, LineSticker sticker)
        {
            byte[] _imageFile = null;
            return Request("notify", message, _imageFile, sticker);
        }

        //public HttpResponseMessage Notify(string message, string imageThumbnail, string imageFullsize)
        //{
        //    LineImage _images = new LineImage {
        //        ImageFullsize = new Uri(imageFullsize),
        //        ImageThumbnail = new Uri(imageThumbnail)
        //    };
        //    return Request("notify", _images);
        //}

        //public HttpResponseMessage Notify(string message, Uri imageThumbnail, Uri imageFullsize)
        //{
        //    LineImage _images = new LineImage
        //    {
        //        ImageFullsize = imageFullsize,
        //        ImageThumbnail = imageThumbnail
        //    };
        //    return Request("notify", _images);
        //}

        //public HttpResponseMessage Request(string message, LineImage images)
        //{
        //    byte[] _imageFile = null;
        //    return Request("notify", message, null, _imageFile, images);
        //}

        public HttpResponseMessage Notify(string message, IFormFile imageFile)
        {
            return Notify(message, imageFile.OpenReadStream());
        }

        public HttpResponseMessage Notify(string message, FileStream imageFile)
        {
            byte[] _imageFile;
            using (var br = new BinaryReader(imageFile))
            {
                _imageFile = br.ReadBytes((int)imageFile.Length);
            }

            return Notify(message, _imageFile);
        }

        public HttpResponseMessage Notify(string message, Stream imageFile)
        {
            byte[] _imageFile;
            using (var br = new BinaryReader(imageFile))
            {
                _imageFile = br.ReadBytes((int)imageFile.Length);
            }

            return Notify(message, _imageFile);
        }

        public HttpResponseMessage Notify(string message, byte[] imageFile)
        {
            return Request("notify", message, imageFile, null);
        }
    }

    class LineNotifyRateLimit
    {

    }

    public class LineImage
    {
        public Uri ImageThumbnail { get; set; }
        public Uri ImageFullsize { get; set; }
    }

    public class LineSticker
    {
        public int StickerPackageId { get; set; }
        public int StickerId { get; set; }
    }
}
