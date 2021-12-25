using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace downloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1 || !Uri.TryCreate(args[0], UriKind.Absolute, out var downloadUri))
            {
                Console.WriteLine("Usage: downloader <download-uri>");
                return;
            }

            var sw = Stopwatch.StartNew();
            var p = downloadUri.Segments.LastOrDefault() ?? "download.bin";
            var path = Path.GetInvalidFileNameChars().Aggregate(p, (path, c) => path.Replace(c, '_'));

            using var client = new HttpClient();
            using var stream = await client.GetStreamAsync(downloadUri);
            using var filestream = File.Create(path);

            await stream.CopyToAsync(filestream);
            Console.WriteLine($"File downloaded to '{path}' in {sw.Elapsed.TotalMilliseconds:#,#} ms");
        }
    }
}
