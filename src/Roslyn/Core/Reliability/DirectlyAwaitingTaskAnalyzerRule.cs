﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;

namespace Roslyn.Diagnostics.Analyzers
{
    internal static class DirectlyAwaitingTaskAnalyzerRule
    {
        private static readonly LocalizableString s_localizableTitle = new LocalizableResourceString(nameof(RoslynDiagnosticsResources.DirectlyAwaitingTaskDescription), RoslynDiagnosticsResources.ResourceManager, typeof(RoslynDiagnosticsResources));
        private static readonly LocalizableString s_localizableMessage = new LocalizableResourceString(nameof(RoslynDiagnosticsResources.DirectlyAwaitingTaskMessage), RoslynDiagnosticsResources.ResourceManager, typeof(RoslynDiagnosticsResources));

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            RoslynDiagnosticIds.DirectlyAwaitingTaskAnalyzerRuleId,
            s_localizableTitle,
            s_localizableMessage,
            "Reliability",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            customTags: WellKnownDiagnosticTags.Telemetry);
    }
}
