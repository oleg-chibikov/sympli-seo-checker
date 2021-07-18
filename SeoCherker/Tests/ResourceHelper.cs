using System;
using System.IO;
using System.Reflection;

namespace OlegChibikov.SympliInterview.SeoChecker.Tests
{
    public static class ResourceHelper
    {
        public static string ReadEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream($"OlegChibikov.SympliInterview.SeoChecker.Tests.TestFiles.{resourceName}") ??
                               throw new InvalidOperationException("Resource does not exist");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}