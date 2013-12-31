using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Owin.FileSystems;

using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public class SingleFileRoute : IRoute
    {
        private readonly string urlPath;

        private readonly string filename;

        private readonly IFileSystem fileSystem;

        public SingleFileRoute(IFileSystem fileSystem, string name, string urlPath, string filename)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(urlPath))
            {
                throw new ArgumentNullException("urlPath");
            }
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException("filename");
            }
            this.urlPath = urlPath;
            this.filename = filename;
            this.fileSystem = fileSystem;
            Name = name;
        }

        public string Name { get; private set; }

        public bool CanRoute(OwinRequest request)
        {
            return string.Equals(request.Path, urlPath, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> GetTemplate(OwinRequest request)
        {
            IFileInfo fileInfo;
            if (!fileSystem.TryGetFileInfo(filename, out fileInfo))
            {
                throw new IOException(string.Format(CultureInfo.CurrentCulture, "File '{0}' was not found.", filename));
            }

            using (var reader = new StreamReader(fileInfo.CreateReadStream()))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}