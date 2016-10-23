using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBloggerPublic.Lib
{
    class jImage
    {

        public static bool CutImage(string sPic, string dPic, int beginX, int beginY, int endX, int endY, string fileExt)
        {
            try
            {
                Bitmap sBitmap = new Bitmap(sPic);//原图
                int dWidth = sBitmap.Width - beginX - endX;
                int dHeight = sBitmap.Height - beginY - endY;
                if (dWidth > 0 && dHeight > 0)
                {
                    Bitmap dBitmap = new Bitmap(dWidth, dHeight);//目标图
                    Rectangle destRect = new Rectangle(0, 0, dWidth, dHeight);//矩形容器
                    Rectangle srcRect = new Rectangle(beginX, beginY, dWidth, dHeight);
                    Graphics gs = Graphics.FromImage(dBitmap);
                    gs.DrawImage(sBitmap, destRect, srcRect, GraphicsUnit.Pixel);
                    ImageFormat format = ImageFormat.Png;
                    switch (fileExt.ToLower())
                    {
                        case "jpg":
                        case "jpeg":
                            format = ImageFormat.Jpeg;
                            break;
                        case "png":
                            format = ImageFormat.Png;
                            break;
                        case "bmp":
                            format = ImageFormat.Bmp;
                            break;
                        case "gif":
                            format = ImageFormat.Gif;
                            break;
                    }
                    dBitmap.Save(dPic, format);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool is_pic(string sPic) {
            try
            {
                Image img = Image.FromFile(sPic);
                if (img.RawFormat.Equals(ImageFormat.Jpeg) || img.RawFormat.Equals(ImageFormat.Png) || img.RawFormat.Equals(ImageFormat.Gif) || img.RawFormat.Equals(ImageFormat.Bmp))
                {
                    return true;
                }else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// 添加图片水印
        /// </summary>
        /// <param name="oldpath">原图片绝对地址</param>
        /// <param name="newpath">新图片放置的绝对地址</param>
        public static bool addWaterMark(string oldpath, string newpath, string watertext="", string waterpic = "", string waterposition= "BOTTOM_RIGHT", string type="WM_TEXT")

        {
            try
            {

                Image image = Image.FromFile(oldpath);

                Bitmap b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(b);
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.High;

                g.DrawImage(image, 0, 0, image.Width, image.Height);

                switch (type)
                {
                    //是图片的话               
                    case "WM_IMAGE":
                        addWatermarkImage(g, waterpic, waterposition, image.Width, image.Height);
                        break;
                    //如果是文字                    
                    case "WM_TEXT":
                        addWatermarkText(g, watertext, waterposition
                            , image.Width, image.Height);
                        break;
                }

                b.Save(newpath);
                b.Dispose();
                image.Dispose();
                return true;
            }
            catch
            {
                if (File.Exists(oldpath))
                {
                    File.Delete(oldpath);
                }
                return false;
            }
            
        }


        /// <summary>
        ///  加水印文字
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="_watermarkText">水印文字内容</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private static void addWatermarkText(Graphics picture, string _watermarkText, string _watermarkPosition, int _width, int _height)
        {
            int[] sizes = new int[] { 24,20,18,16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 7; i++)
            {
                crFont = new Font("arial", sizes[i], FontStyle.Bold);
                crSize = picture.MeasureString(_watermarkText, crFont);

                if ((ushort)crSize.Width < (ushort)_width)
                    break;
            }
            float xpos = 0;
            float ypos = 0;
            switch (_watermarkPosition)
            {
                case "TOP_LEFT":
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case "TOP_RIGHT":
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case "BOTTOM_RIGHT":
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
                case "BOTTOM_LEFT":
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
            }

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            picture.DrawString(_watermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
            picture.DrawString(_watermarkText, crFont, semiTransBrush, xpos, ypos, StrFormat);

            semiTransBrush2.Dispose();
            semiTransBrush.Dispose();

        }

        /// <summary>
        ///  加水印图片
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="WaterMarkPicPath">水印图片的地址</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private static void addWatermarkImage(Graphics picture, string WaterMarkPicPath, string _watermarkPosition, int _width, int _height)
        {
            Image watermark = new Bitmap(WaterMarkPicPath);

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;
            int WatermarkWidth = 0;
            int WatermarkHeight = 0;
            double bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((_width > watermark.Width * 4) && (_height > watermark.Height * 4))
            {
                bl = 1;
            }
            else if ((_width > watermark.Width * 4) && (_height < watermark.Height * 4))
            {
                bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);

            }
            else

            if ((_width < watermark.Width * 4) && (_height > watermark.Height * 4))
            {
                bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);
            }
            else
            {
                if ((_width * watermark.Height) > (_height * watermark.Width))
                {
                    bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);

                }
                else
                {
                    bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);

                }

            }

            WatermarkWidth = Convert.ToInt32(watermark.Width * bl);
            WatermarkHeight = Convert.ToInt32(watermark.Height * bl);



            switch (_watermarkPosition)
            {
                case "WM_TOP_LEFT":
                    xpos = 10;
                    ypos = 10;
                    break;
                case "WM_TOP_RIGHT":
                    xpos = _width - WatermarkWidth - 10;
                    ypos = 10;
                    break;
                case "WM_BOTTOM_RIGHT":
                    xpos = _width - WatermarkWidth - 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
                case "WM_BOTTOM_LEFT":
                    xpos = 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
            }

            picture.DrawImage(watermark, new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);


            watermark.Dispose();
            imageAttributes.Dispose();
        }

    }
}
