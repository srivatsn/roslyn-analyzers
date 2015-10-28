using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace a2md
{
    /// <summary>
    /// Represents a diagnostic in the RoslynAnalyzers projects
    /// </summary>
    public class RoslynAnalyzersDiagnosticInfo
    {
        /// <summary>
        /// The Id of the diagnostic including the prefix 'SA' or 'SX'
        /// </summary>
        /// <value>
        /// The Id of the diagnostic including the prefix 'SA' or 'SX'
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// The short name if the diagnostic that is used in the class name.
        /// </summary>
        /// <value>
        /// The short name if the diagnostic that is used in the class name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Whether or not the diagnostic is implemented in C#.
        /// </summary>
        /// <value>
        /// Whether or not the diagnostic is implemented in C#.
        /// </value>
        public bool HasCSharpImplementation { get; set; }

        /// <summary>
        /// Whether or not the diagnostic is implemented in VB.
        /// </summary>
        /// <value>
        /// Whether or not the diagnostic is implemented in VB.
        /// </value>
        public bool HasVBImplementation { get; set; }

        /// <summary>
        /// Represents if the diagnostic is enabled or not. This can indicate if the
        /// diagnostic is enabled by default or not, or if it is disabled because
        /// there are no tests for the diagnostic.
        /// </summary>
        /// <value>
        /// Represents if the diagnostic is enabled or not. This can indicate if the
        /// diagnostic is enabled by default or not, or if it is disabled because
        /// there are no tests for the diagnostic.
        /// </value>
        public string IsEnabledByDefault { get; set; }

        /// <summary>
        /// Represents whether or not there is a code fix for the diagnostic.
        /// </summary>
        /// <value>
        /// Represents whether or not there is a code fix for the diagnostic.
        /// </value>
        public bool HasCodeFix { get; set; }

        /// <summary>
        /// Returns the title of this diagnostic
        /// no reason.
        /// </summary>
        /// <value>
        /// Returns the title of this diagnostic
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Returns the category of this diagnostic
        /// no reason.
        /// </summary>
        /// <value>
        /// Returns the category of this diagnostic
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Returns help link for this diagnostic
        /// </summary>
        /// <value>
        /// Returns help link for this diagnostic
        /// </value>
        public string HelpLink { get; set; }

        /// <summary>
        /// Returns the analyzer package to which this diagnostic belongs.
        /// </summary>
        /// <value>
        /// Returns the analyzer package to which this diagnostic belongs.
        /// </value>
        public string AnalyzerPackage { get; set; }

        /// <summary>
        /// Returns a string representing this diagnostic
        /// </summary>
        /// <returns>
        /// The string contains the diagnostic id and the short name.
        /// </returns>
        public override string ToString()
        {
            return this.Id + " " + this.Name;
        }

        /// <summary>
        /// Returns a json representation of this diagnostic
        /// </summary>
        /// <returns>A json string representing this diagnostic</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Creates an instance of the <see cref="RoslynAnalyzersDiagnosticInfo"/> class
        /// that is populated with the data stored in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">A json representing a <see cref="RoslynAnalyzersDiagnosticInfo"/></param>
        /// <returns>A <see cref="RoslynAnalyzersDiagnosticInfo"/> that is populated with the data stored in <paramref name="value"/>.</returns>
        public static RoslynAnalyzersDiagnosticInfo FromJson(string value)
        {
            return JsonConvert.DeserializeObject<RoslynAnalyzersDiagnosticInfo>(value);
        }
    }
}
