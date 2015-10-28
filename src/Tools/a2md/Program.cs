// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Newtonsoft.Json;

namespace a2md
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Loader loader = new Loader();

            var analyzerReferences = args
                .Select(arg => new AnalyzerFileReference(arg, loader));

            GenerateStatus(analyzerReferences);

            GenerateMarkdown(analyzerReferences);
        }

        private static void GenerateStatus(IEnumerable<AnalyzerFileReference> analyzerReferences)
        {
            DescriptorEqualityComparer comparer = new DescriptorEqualityComparer();

            var allAnalyzers = analyzerReferences
                .Select(analyzerReference => new { AnalyzerPackage = analyzerReference.Display, Analyzers = analyzerReference.GetAnalyzersForAllLanguages() });

            var fixableDiagnosticIds = analyzerReferences
                .SelectMany(analyzerReference => analyzerReference.GetFixers())
                .SelectMany(fixer => fixer.FixableDiagnosticIds)
                .Distinct();

            Dictionary<string, RoslynAnalyzersDiagnosticInfo> diagnosticInfoMap = new Dictionary<string, RoslynAnalyzersDiagnosticInfo>();
            foreach (var group in allAnalyzers)
            {
                foreach (var analyzer in group.Analyzers)
                {
                    bool hasImplementation = true; // TODO: compute this through some heuristic
                    bool hasCSharpImplementation = hasImplementation && analyzer.GetType().GetCustomAttribute<DiagnosticAnalyzerAttribute>().Languages.Contains(LanguageNames.CSharp);
                    bool hasVBImplementation = hasImplementation && analyzer.GetType().GetCustomAttribute<DiagnosticAnalyzerAttribute>().Languages.Contains(LanguageNames.VisualBasic);

                    foreach (var descriptor in analyzer.SupportedDiagnostics.Distinct(comparer))
                    {
                        if (!diagnosticInfoMap.ContainsKey(descriptor.Id))
                        {
                            var hasCodeFix = fixableDiagnosticIds.Contains(descriptor.Id);

                            var analyzerPackage = group.AnalyzerPackage.Replace(".CSharp", string.Empty).Replace(".VisualBasic", string.Empty).Replace(".Common", string.Empty);

                            var diagnosticInfo = new RoslynAnalyzersDiagnosticInfo
                            {
                                Id = descriptor.Id,
                                Category = descriptor.Category,
                                HasCSharpImplementation = hasCSharpImplementation,
                                HasVBImplementation = hasVBImplementation,
                                Name = analyzer.GetType().Name,
                                Title = descriptor.Title.ToString(),
                                HelpLink = descriptor.HelpLinkUri,
                                HasCodeFix = hasCodeFix,
                                IsEnabledByDefault = descriptor.IsEnabledByDefault.ToString(),
                                AnalyzerPackage = analyzerPackage
                            };
                            diagnosticInfoMap.Add(descriptor.Id, diagnosticInfo);
                        }
                        else
                        {
                            var diagnosticInfo = diagnosticInfoMap[descriptor.Id];
                            diagnosticInfo.HasCSharpImplementation |= hasCSharpImplementation;
                            diagnosticInfo.HasVBImplementation |= hasVBImplementation;
                        }
                    }

                }
            }

            Console.WriteLine(JsonConvert.SerializeObject(diagnosticInfoMap.Values));
        }

        private static void GenerateMarkdown(IEnumerable<AnalyzerFileReference> analyzerReferences)
        {
            DescriptorEqualityComparer comparer = new DescriptorEqualityComparer();

            var diagnostics = analyzerReferences
                .SelectMany(analyzerReference => analyzerReference.GetAnalyzersForAllLanguages())
                .SelectMany(analyzer => analyzer.SupportedDiagnostics)
                .Distinct(comparer)
                .OrderBy(descriptor => descriptor.Id);


            var outputMarkdown = diagnostics
                .Select(GenerateDescriptorText)
                .Join(Environment.NewLine + Environment.NewLine);

            Console.Write(outputMarkdown);
        }

        private static string GenerateDescriptorText(DiagnosticDescriptor descriptor)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"### {descriptor.Id}: {descriptor.Title} ###");

            if (!string.IsNullOrWhiteSpace(descriptor.Description.ToString()))
            {
                builder
                    .AppendLine()
                    .AppendLine()
                    .Append(descriptor.Description.ToString());
            }

            builder
                .AppendLine()
                .AppendLine()
                .AppendLine($"Category: {descriptor.Category}")
                .AppendLine()
                .Append($"Severity: {descriptor.DefaultSeverity}");

            if (!string.IsNullOrWhiteSpace(descriptor.HelpLinkUri))
            {
                builder
                    .AppendLine()
                    .AppendLine()
                    .Append($"Help: [{descriptor.HelpLinkUri}]({descriptor.HelpLinkUri})");
            }

            return builder.ToString();
        }
    }
}
