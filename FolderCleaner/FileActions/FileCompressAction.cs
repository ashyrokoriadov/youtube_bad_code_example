using FolderCleaner.Interfaces;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace FolderCleaner.FileActions
{
    class FileCompressAction : IFileAction
    {
        public void Execute(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                ExtractDirectoryAndFileNameFromPath(path, out string directoryName, out string fileName);
                CreateArchive(path, directoryName, fileName);
                Console.WriteLine("Архив создан.");
            }
        }

        private void ExtractDirectoryAndFileNameFromPath(
            string path,
            out string directoryName,
            out string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(path);
            directoryName = Path.GetDirectoryName(path);
        }

        private void CreateArchive(string path, string directoryName, string fileName)
        {
            using (var outputStream = new ZipOutputStream(File.Create($"{directoryName}\\{fileName}.zip")))
            {
                outputStream.SetLevel(9);

                var entry = new ZipEntry(Path.GetFileName(path))
                {
                    DateTime = DateTime.Now
                };

                outputStream.PutNextEntry(entry);

                WriteDataToArchive(outputStream, path);

                outputStream.Finish();
                outputStream.Close();
            }
        }

        private void WriteDataToArchive(Stream stream, string path)
        {
            using (var fileStream = File.OpenRead(path))
            {
                int sourceBytes;
                do
                {
                    byte[] buffer = new byte[4096];
                    sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                    stream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }
    }
}
