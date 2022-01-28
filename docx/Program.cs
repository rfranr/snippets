using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace docx
{
    class Program
    {

        static XmlNode SetText(XmlDocument document, string source, string dest)
        {
            var xmlNamespaceManager = new XmlNamespaceManager(document.NameTable);
            xmlNamespaceManager.AddNamespace("w", @"http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            XmlNode xmlNode = document.SelectSingleNode($"//w:body/w:p/*/w:t[text()=\"{source}\"]", xmlNamespaceManager);

            if( xmlNode != null)
            {
                xmlNode.InnerText = dest;
            }
            else
            {
                throw new ApplicationException($"{source} not present in {document.LocalName}");
            }


            return xmlNode;
        }


        static MemoryStream InnerFile_Word_Document_xml(Stream stream, string track1, string track2, string track3)
        {
            String templateTrack1 = "222222";
            String templateTrack2 = "4444444";
            String templateTrack3 = "38888888";

            XmlDocument document = new XmlDocument();
            document.Load(stream);

            Program.SetText(document, templateTrack1, track1);
            Program.SetText(document, templateTrack2, track2);
            Program.SetText(document, templateTrack3, track3);

            MemoryStream xmlStream = new MemoryStream();
            document.Save(xmlStream);
            xmlStream.Position = 0;
            return xmlStream;
        }

        static MemoryStream InnerFile_word_media_image1_png(Image sourceImage , string employeeFullName, string companyName, Image profileImage, Image companyLogoImage, Image companyBottomRibbonImage)
        {
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            Bitmap cardImage = new Bitmap(sourceImage);

            using (var graphics = Graphics.FromImage(cardImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic; 
                //graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                // profile
                graphics.DrawImage(profileImage, 523, 2, 209, 269); // pos,  dimensions
                // logo
                graphics.DrawImage(companyLogoImage, 1, 1, 298 +30, 163 + 30); // pos,  dimensions  

                // employeeName
                var font = new Font("Arial", 32);
                SizeF stringSize = new SizeF();
                stringSize = graphics.MeasureString(employeeFullName, font);
                int xx = cardImage.Width / 2;
                int yy = cardImage.Height - companyBottomRibbonImage.Height - 20; 
                graphics.DrawString(employeeFullName,  font, new SolidBrush(Color.Black), xx - stringSize.Width/2, yy - stringSize.Height/ 2, new StringFormat()); 

            };

            MemoryStream pngStream = new MemoryStream();

            cardImage.Save(pngStream, ImageFormat.Png);
            pngStream.Position = 0;
            return pngStream;
        }

        static void Main(string[] args)
        {
            MemoryStream memoryStream = new MemoryStream(); 
            ZipArchive destDocx = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);

            string path = "c:\\temp\\targeta_con_texto_pipes.docx";

            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (ZipArchive zip = new ZipArchive(fs))
            {
                foreach ( var entry in zip.Entries)
                {
                    Console.WriteLine(entry.FullName);
                    Stream sourceStream;
                    
                    if (entry.FullName == "word/document.xml")
                    {
                        sourceStream = InnerFile_Word_Document_xml(entry.Open(), "track1", "track2", "track3" );
                    }
                    else if (entry.FullName == "word/media/image1.png")
                    {
                        // todo
                        Image profileImage = Image.FromStream(new FileStream("c:\\Temp\\DefaultProfileCardImage.bmp", FileMode.Open));
                        Image companyLogoImage = Image.FromStream(new FileStream("c:\\Temp\\DefaultCompanyLogo.bmp", FileMode.Open));
                        Image companyBottomRibbonImage = Image.FromStream(new FileStream("c:\\Temp\\DefaultCompanyBottomRibon.bmp", FileMode.Open)); 

                        // source document Image
                        Image documentImage = Image.FromStream(entry.Open());

                        
                        sourceStream = InnerFile_word_media_image1_png(documentImage, "Abarca Vilchez, Luís", "Empresa del Rio Asociados, S.L.", profileImage, companyLogoImage, companyBottomRibbonImage);
                    }
                    else
                    {
                        sourceStream = entry.Open();
                    }

                    // READ ZIP CONTENTS PARSE AND WRITE TO ANOTHER ZIP
                    ZipArchiveEntry zipEntry  = destDocx.CreateEntry(entry.FullName);
                    var zipEntryStream = zipEntry.Open();
                    sourceStream.CopyTo(zipEntryStream);
                    zipEntryStream.Close();
                }
            }
            memoryStream.Flush();
            destDocx.Dispose();


            // write to disk
            using (var fileStream = new FileStream(@"C:\Temp\test2.zip.docx", FileMode.Create))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(fileStream);
            }


        }
    }
}
