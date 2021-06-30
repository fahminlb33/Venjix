using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;

namespace Venjix.Infrastructure.Helpers
{
    public static class CommonHelpers
    {
        // ReSharper disable once InconsistentNaming
        public static readonly CultureInfo USCulture = new CultureInfo("en-US");

        public static async Task<Stream> SerializeCsvRecords<T>(IEnumerable<T> records)
        {
            var ms = new MemoryStream();
            var streamWriter = new StreamWriter(ms);
            var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            
            await csvWriter.WriteRecordsAsync(records);
            await csvWriter.FlushAsync();
            await streamWriter.FlushAsync();
            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }
    }
}
