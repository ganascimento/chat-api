using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace GADev.Chat.Application.Util.Implementation
{
    public class ImageStorage : IImageStorage
    {
        private IHostingEnvironment  _hostingEnvironment;

        public ImageStorage(IHostingEnvironment  hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void SaveImage(string nameImage, string avatar)
        {
            var pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, "assets", "images", nameImage);
            byte[] bytes = Convert.FromBase64String(avatar);

            if (!Directory.Exists(Path.Combine(_hostingEnvironment.ContentRootPath, "assets", "images"))) {
                Directory.CreateDirectory(Path.Combine(_hostingEnvironment.ContentRootPath, "assets", "images"));
            }

            using (MemoryStream stream = new MemoryStream(bytes)) {
                Image img = Image.FromStream(stream);
                int MAX_WIDTH = 600;
                int MAX_HEIGHT = 400;
                int width = img.Width;
                int height = img.Height;
                
                if (width > height) {
                    if (width > MAX_WIDTH) {
                        height = MAX_HEIGHT;
                        width = MAX_WIDTH;
                    }
                } else {
                    if (height > MAX_HEIGHT) {
                        width = MAX_HEIGHT;
                        height = MAX_WIDTH;
                    }
                }

                var destRect = new Rectangle(0, 0, width, height);

                using (Bitmap btm = new Bitmap(width, height)) {               
                    btm.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                    using (var graphics = Graphics.FromImage(btm)) {
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.Low;
                        graphics.SmoothingMode = SmoothingMode.HighSpeed;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                        using (var wrapMode = new ImageAttributes())
                        {
                            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                            graphics.DrawImage(img, destRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, wrapMode);
                        }
                    }

                    btm.Save(pathFile, System.Drawing.Imaging.ImageFormat.Png);                    
                }
            }
        }
        
        public void RemoveImage(string nameImage) {
            try {
                string pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, "assets", "images", nameImage);

                if (File.Exists(pathFile)){
                    File.Delete(pathFile);
                }
            } catch {}
        }

        public string GetImage(string nameImage) {
            try {
                string imageBase64 = null;
                string pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, "assets", "images", nameImage);

                using (Image image = Image.FromFile(pathFile)){
                    using (MemoryStream stream = new MemoryStream()){
                        image.Save(stream, image.RawFormat);
                        byte[] bytes = stream.ToArray();
                        
                        imageBase64 = Convert.ToBase64String(bytes);
                    }
                }

                return imageBase64;
            }
            catch {
                return null;
            }
        }
    }
}