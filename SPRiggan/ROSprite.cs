using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SPRiggan
{
    class ROSprite
    {
        public byte[] SP;
        public byte[] version;
        public UInt16 num_pal;
        public UInt16 num_rgba;
        public Frame[] frames;
        public Palette palette;

        private string filename;

        public ROSprite(string filename)
        {
            this.filename = filename;
            FileStream file = File.OpenRead(filename);
            BinaryReader reader = new BinaryReader(file);

            SP = reader.ReadBytes(2);
            version = reader.ReadBytes(2);
            num_pal = reader.ReadUInt16();
            num_rgba = reader.ReadUInt16();

            if (num_pal > 0)
            {
                frames = new Frame[num_pal];

                for (int i = 0; i < num_pal; i++)
                {

                    frames[i].width = reader.ReadUInt16();
                    frames[i].height = reader.ReadUInt16();
                    frames[i].data_length = reader.ReadUInt16();

                    byte[] temp_frame_data = reader.ReadBytes(frames[i].data_length);

                    //RLE DECODE

                    frames[i].frame_data = new byte[(UInt16)(frames[i].height * frames[i].width)];
                    int index = 0;
                    for (int j = 0; j < frames[i].data_length; j++)
                    {
                        if (temp_frame_data[j] == 0x00)
                        {
                            for (int k = 0; k < temp_frame_data[j + 1]; k++)
                            {
                                frames[i].frame_data[index] = 0x00;
                                index++;
                            }
                            j++;
                        }
                        else
                        {
                            frames[i].frame_data[index] = temp_frame_data[j];
                            index++;
                        }
                    }
                }
            }

            if (num_rgba > 0)
            {
                frames = new Frame[num_rgba];

                for (int i = 0; i < num_rgba; i++)
                {
                    frames[i].width = reader.ReadUInt16();
                    frames[i].height = reader.ReadUInt16();
                    frames[i].data_length = (UInt16)(frames[i].height * frames[i].width * 4);
                    frames[i].frame_data = new byte[frames[i].data_length];
                    frames[i].frame_data = reader.ReadBytes(frames[i].data_length);
                }
            }

            palette = new Palette();
            palette = MarshalSerializer.Deserialize<Palette>(reader.ReadBytes(1024), 0);

            reader.Close();
            reader.Dispose();
            file.Close();
            file.Dispose();
        }

        public void SavePNGFiles()
        {
            Console.WriteLine("Saving individual frames from SPR file.");

            
            string dir_name;

            if (Path.IsPathRooted(filename))
            {
                dir_name = Path.GetDirectoryName(filename);
            }
            else
            {
                dir_name = Path.GetDirectoryName(Path.GetFullPath(filename));
            }

            string basename = Path.GetFileNameWithoutExtension(filename);
            dir_name = dir_name + "\\" + basename;

            if (!Directory.Exists(dir_name)) Directory.CreateDirectory(dir_name);

            for (int i = 0; i < num_pal; i++)
            {

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(frames[i].width, frames[i].height);

                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {

                        byte rawpixel = frames[i].frame_data[y * bmp.Width + x];
                        Color pixelcolor;

                        if (rawpixel != 0x00)
                        {
                            pixelcolor = Color.FromArgb(palette.data[rawpixel].r, palette.data[rawpixel].g, palette.data[rawpixel].b);
                        }
                        else
                        {
                            pixelcolor = Color.Transparent;
                        }

                        bmp.SetPixel(x, y, pixelcolor);

                    }
                }

                Console.Write("Saving " + basename + i + ".png...");
                bmp.Save(dir_name + "\\" + basename + i + ".png", System.Drawing.Imaging.ImageFormat.Png);
                Console.WriteLine("DONE.");
            }
        }
    }
}
