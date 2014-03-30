using System;
using System.IO;
using Microsoft.Owin.FileSystems;
using RazorEngine.Templating;

namespace Mezm.Owin.Razor.Rendering
{
    public class FileSystemTempleteResolver : ITemplateResolver
    {
        private readonly IFileSystem fileSystem;

        public FileSystemTempleteResolver(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public string Resolve(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            IFileInfo fileInfo;
            if (!fileSystem.TryGetFileInfo("/" + name, out fileInfo))
            {
                throw new FileNotFoundException(string.Format("Cannot resolve file '{0}'", name));
            }

            using (var stream = new StreamReader(fileInfo.CreateReadStream()))
            {
                return stream.ReadToEnd();
            }
        }
    }
}