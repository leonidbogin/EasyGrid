using EasyGrid.Core.Models.Gpx;
using EasyGrid.Core.Services;
using Nito.AsyncEx;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EasyGrid.Core.Providers
{
    public class SaveGpxProvider : ProgressService
    {
        private readonly XmlWriterSettings xmlWriterSettings;

        public SaveGpxProvider(bool indent = true)
        {
            xmlWriterSettings = new XmlWriterSettings { Indent = indent };
        }

        public async Task SaveAsync(Gpx gpx, string path, CancellationToken cancellationToken)
        {
            LogProgress(0);
            LogStatus("Saving grid to file");

            if (cancellationToken.IsCancellationRequested) return;

            try
            {
                var task = Task.Factory.StartNew(Save, (gpx, path), cancellationToken);
                await task.WaitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            
            LogProgress(100);
        }

        private void Save(object state)
        {
            var st = ((Gpx gpx, string path))state;

            using var writer = XmlWriter.Create(st.path, xmlWriterSettings);
            var serializer = new XmlSerializer(typeof(Gpx));
            serializer.Serialize(writer, st.gpx);
        }
    }
}
