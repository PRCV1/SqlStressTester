using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SqlStressTester.Utils.IO
{
    public static class Json
    {

        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            WriteIndented = true
        };

        public static async Task<T> LoadFromFileAsync<T>(string filePath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }
            using Stream stream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<T>(stream, _serializerOptions, cancellationToken);
        }

        public static async Task SaveToFileAsync<T>(T type, string filePath, CancellationToken cancellationToken = default)
        {
            using Stream stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            await JsonSerializer.SerializeAsync<T>(stream, type, _serializerOptions, cancellationToken);
        }

    }
}
