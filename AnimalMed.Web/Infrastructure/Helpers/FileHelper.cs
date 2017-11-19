using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AnimalMed.DTO.Infrastructure.Helpers
{
    public class FileHelper
    {
        public static bool SaveImageFromStream(Stream stream, string filePath)
        {
            var img = System.Drawing.Image.FromStream(stream);
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                if (stream == null)
                {
                    return false;
                }

                img.Save(filePath);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                img.Dispose();
                stream.Dispose();
                GC.Collect();
            }

            return true;
        }

        public static bool SaveFileFromStream(Stream stream, string filePath)
        {
            FileStream fileStream = null;
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                if (stream == null)
                {
                    return false;
                }

                fileStream = File.Create(filePath, (int)stream.Length);
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, bytesInStream.Length);
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                fileStream.Dispose();
                stream.Dispose();
                GC.Collect();
            }

            return true;
        }

        public static bool DeleteImage(string filePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
