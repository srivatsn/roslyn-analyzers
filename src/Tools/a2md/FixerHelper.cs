using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;

namespace a2md
{
    public static class FixerHelper
    {
        public static ImmutableArray<CodeFixProvider> GetFixers(this AnalyzerFileReference analyzerFileReference)
        {
            if (analyzerFileReference == null)
            {
                return ImmutableArray<CodeFixProvider>.Empty;
            }

            IEnumerable<TypeInfo> typeInfos = null;
            ImmutableArray<CodeFixProvider>.Builder builder = null;

            try
            {
                Assembly analyzerAssembly = analyzerFileReference.GetAssembly();
                typeInfos = analyzerAssembly.DefinedTypes;

                foreach (var typeInfo in typeInfos)
                {
                    if (typeInfo.IsSubclassOf(typeof(CodeFixProvider)))
                    {
                        try
                        {
                            var attribute = typeInfo.GetCustomAttribute<ExportCodeFixProviderAttribute>();
                            if (attribute != null)
                            {
                                builder = builder ?? ImmutableArray.CreateBuilder<CodeFixProvider>();
                                builder.Add((CodeFixProvider)Activator.CreateInstance(typeInfo.AsType()));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }

            return builder != null ? builder.ToImmutable() : ImmutableArray<CodeFixProvider>.Empty;
        }
    }

}
}
