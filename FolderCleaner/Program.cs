using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            string fc = @"C:\Sandbox\Test";
            string act = "Delete";
            string ptrn = "*.txt";
            bool allf = true;

            var fs = new FileSystem();
            IEnumerable<string> files;

            try
            {
                if (allf)
                {
                    files = fs.Directory.EnumerateFiles(fc, ptrn, SearchOption.AllDirectories);
                }
                else
                {
                    files = fs.Directory.EnumerateFiles(fc, ptrn, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }

            switch (act)
            {
                case "Delete":
                    FileRemover fr = new FileRemover();
                    fr.Remove(files.ToArray());
                    break;
                case "Compress":
                    FileCompressor fcom = new FileCompressor();
                    fcom.Compress(files.ToArray());
                    break;
                default:
                    throw new Exception("Действие не поддерживается.");
            }

            Console.WriteLine("OK");
            Console.ReadKey();

        }
    }

    class FileRemover
    {
        public void Remove(string[] d)
        {
            var fs = new FileSystem();

            foreach (var folder in d)
            {
                fs.File.Delete(folder);
            }
        }
    }

    class FileCompressor
    {
        public void Compress(string[] d)
        {
            foreach (var folder in d)
            {
                string fn = Path.GetFileNameWithoutExtension(folder);
                string dn = Path.GetDirectoryName(folder);

                ZipOutputStream os = new ZipOutputStream(File.Create(dn + "\\" + fn + ".zip"));
                os.SetLevel(9);

                byte[] b = new byte[4096];
                ZipEntry e = new ZipEntry(Path.GetFileName(folder));
                e.DateTime = DateTime.Now;
                os.PutNextEntry(e);

                FileStream fs = File.OpenRead(folder);
                int sb;
                do
                {
                    sb = fs.Read(b, 0, b.Length);
                    os.Write(b, 0, sb);
                } while (sb > 0);

                os.Finish();
                os.Close();

                Console.WriteLine("Архив создан.");
            }
        }
    }
}
