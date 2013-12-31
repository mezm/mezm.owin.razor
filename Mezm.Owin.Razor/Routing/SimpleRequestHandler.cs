using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Owin.FileSystems;

using Owin.Types;

namespace Mezm.Owin.Razor.Routing
{
    public class SimpleRequestHandler : IRequestHandler
    {
        private readonly IFileInfo file;

        private readonly Func<OwinRequest, object> modelProvider;

        public SimpleRequestHandler(IFileInfo file, Func<OwinRequest, object> modelProvider)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }
            if (modelProvider == null)
            {
                throw new ArgumentNullException("modelProvider");
            }
            this.file = file;
            this.modelProvider = modelProvider;
        }

        public async Task<string> GetTemplate(OwinRequest request)
        {
            using (var reader = new StreamReader(file.CreateReadStream()))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public Task<object> GetModel(OwinRequest request)
        {
            return Task.Run(() => modelProvider(request));
        }
    }
}