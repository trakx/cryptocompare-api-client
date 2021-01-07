using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Trakx.Utils.Attributes;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.CryptoCompare.ApiClient.Tests.Unit.Configuration
{
    public class EnvironmentReadmeUnitTest
    {
        private readonly ITestOutputHelper _output;

        public EnvironmentReadmeUnitTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void UpdateReadmeTest()
        {
            var directory = GetAssemblyDirectory();
            var readmeFilePath = Path.Combine(directory.Parent.Parent.Parent.Parent.Parent.ToString(), "Environment.md");
            var configProperties = GetConfigurationProperties(directory);

            var mdLines = new List<string>();

            AddHeaderLines(mdLines);

            DumpVarNamesByAssembly(mdLines, configProperties);

            AddFullSampleIntro(mdLines);

            BuildSampleEnvFile(mdLines, configProperties);

            AddClosingLines(mdLines);

            File.WriteAllLines(readmeFilePath, mdLines);
        }

        private void BuildSampleEnvFile(List<string> mdLines, Dictionary<string, List<string>> configProperties)
        {
            configProperties
                .Values
                .SelectMany(x => x)
                .Distinct()
                .ToList()
                .ForEach(x =>
                {
                    mdLines.Add($"\t{x}");
                });
            mdLines.Add(string.Empty);
        }

        private void DumpVarNamesByAssembly(List<string> mdLines, Dictionary<string, List<string>> configProperties)
        {
            configProperties
                .Keys
                .ToList()
                .ForEach(key =>
                {
                    mdLines.Add($"### {key}");
                    mdLines.Add(string.Empty);

                    configProperties[key]
                    .ToList()
                    .ForEach(propertyName =>
                    {
                        mdLines.Add($"\t{propertyName}");
                    });

                });

            mdLines.Add(string.Empty);
        }

        private void AddHeaderLines(IList<string> lines)
        {
            lines.Add("## Avoid committing you secrets and keys ");
            lines.Add("In order to be able to run some integration tests, you should create a .env file with the following items based on the Configuration classes you instanciate: ");
            lines.Add(string.Empty);
        }

        private void AddClosingLines(IList<string> lines)
        {
            lines.Add("### You should update the path to your .env file in src/Trakx.Tests/Tools/Secrets.cs");
        }

        private void AddFullSampleIntro(IList<string> lines)
        {
            lines.Add("### Complete .env file sample");
            lines.Add(string.Empty);

        }

        private DirectoryInfo GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            return new DirectoryInfo(codeBase).Parent;
        }

        private Dictionary<string, List<string>> GetConfigurationProperties(DirectoryInfo dirInfo)
        {
            var result = new Dictionary<string, List<string>>();

            var files = dirInfo.GetFiles("Trakx.*.dll");

            files
                .ToList()
                .ForEach(file =>
                {
                    var currentAssembly = Assembly.LoadFrom(file.FullName);

                    var assemblyTypes = currentAssembly.GetTypes();

                    assemblyTypes.ToList().ForEach(x =>
                    {
                        var typename = x.FullName;

                        if (!string.IsNullOrEmpty(typename) &&
                        typename.EndsWith("Configuration"))
                        {
                            var varNames = new List<string>();

                            var typeProperties = x.GetProperties();

                            if (typeProperties.Count() > 0)
                            {
                                foreach (var property in typeProperties)
                                {
                                    if (property.GetCustomAttribute(typeof(ReadmeDocumentAttribute)) is ReadmeDocumentAttribute attribute)
                                    {
                                        varNames.Add(attribute.VarName);
                                    }
                                }

                                if (varNames.Count > 0)
                                {
                                    result.Add(typename, varNames);
                                }
                            }
                        }
                    });
                });

            return result;
        }
    }
}
