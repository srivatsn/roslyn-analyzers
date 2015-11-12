// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace System.Security.Cryptography.Hashing.Algorithms.Analyzers.Common
{
    public static class DiagnosticHelpers
    {
        public static bool MatchMemberDerived(this ISymbol member, INamedTypeSymbol type, string name)
        {
            return member != null && member.ContainingType.IsDerivedFrom(type) && member.MetadataName == name;
        }

        public static bool MatchMethodDerived(this ISymbol member, INamedTypeSymbol type, string name)
        {
            return member != null && member.Kind == SymbolKind.Method && member.MatchMemberDerived(type, name);
        }

        public static bool MatchPropertyDerived(this ISymbol member, INamedTypeSymbol type, string name)
        {
            return member != null && member.Kind == SymbolKind.Property && member.MatchMemberDerived(type, name);
        }

        public static bool MatchFieldDerived(this ISymbol member, INamedTypeSymbol type, string name)
        {
            return member != null && member.Kind == SymbolKind.Field && member.MatchMemberDerived(type, name);
        }

        public static bool IsDerivedFrom(this ITypeSymbol typeSymbol, ITypeSymbol baseSymbol, bool baseTypesOnly = false)
        {
            if (baseSymbol == null)
            {
                return false;
            }

            if (!baseTypesOnly && typeSymbol.AllInterfaces.Contains(baseSymbol))
            {
                return true;
            }

            for (ITypeSymbol baseType = typeSymbol; baseType != null; baseType = baseType.BaseType)
            {
                if (baseType == baseSymbol)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool MatchMember(this ISymbol member, INamedTypeSymbol type, string name)
        {
            return member != null && member.ContainingType == type && member.MetadataName == name;
        }

        public static bool MatchMethod(this ISymbol member, INamedTypeSymbol type, string name)
        {
            return member != null && member.Kind == SymbolKind.Method && member.MatchMember(type, name);
        }

        public static bool MatchProperty(this ISymbol member, INamedTypeSymbol type, string name)
        {
            return member != null && member.Kind == SymbolKind.Property && member.MatchMember(type, name);
        }

        public static bool MatchField(this ISymbol member, INamedTypeSymbol type, string name)
        {
            return member != null && member.Kind == SymbolKind.Field && member.MatchMember(type, name);
        }

        public static ITypeSymbol GetVariableSymbolType(this ISymbol symbol)
        {
            if (symbol == null)
            {
                return null;
            }
            SymbolKind kind = symbol.Kind;
            switch (kind)
            {
                case SymbolKind.Field:
                    return ((IFieldSymbol)symbol).Type;
                case SymbolKind.Local:
                    return ((ILocalSymbol)symbol).Type;
                case SymbolKind.Parameter:
                    return ((IParameterSymbol)symbol).Type;
                case SymbolKind.Property:
                    return ((IPropertySymbol)symbol).Type;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Checks if a symbol is visible outside of an assembly.
        /// </summary>
        /// <param name="symbol">The symbol whose access shall be checked.</param>
        /// <returns>true if the symbol is visible outside its assembly; otherwise, false.</returns>
        public static bool IsVisibleOutsideAssembly(this ISymbol symbol)
        {
            if (symbol == null)
            {
                return false;
            }

            for (ISymbol containingType = symbol; containingType != null; containingType = containingType.ContainingType)
            {
                if (IsInvisibleOutsideAssemblyAtSymbolLevel(containingType))
                {
                    return false;
                }
            }

            return true;
        }

        public static LocalizableResourceString GetLocalizableResourceString(string resourceName)
        {
            return new LocalizableResourceString(resourceName, SystemSecurityCryptographyHashingAlgorithmsAnalyzersResources.ResourceManager, typeof(SystemSecurityCryptographyHashingAlgorithmsAnalyzersResources));
        }

        private static bool IsInvisibleOutsideAssemblyAtSymbolLevel(ISymbol symbol)
        {
            return SymbolIsPrivateOrInternal(symbol)
                || SymbolIsProtectedInSealed(symbol);
        }

        private static bool SymbolIsPrivateOrInternal(ISymbol symbol)
        {
            var access = symbol.DeclaredAccessibility;
            return access == Accessibility.Private
                || access == Accessibility.Internal
                || access == Accessibility.ProtectedAndInternal
                || access == Accessibility.NotApplicable;
        }

        private static bool SymbolIsProtectedInSealed(ISymbol symbol)
        {
            var containgType = symbol.ContainingType;
            if (containgType != null && containgType.IsSealed)
            {
                var access = symbol.DeclaredAccessibility;
                return access == Accessibility.Protected
                    || access == Accessibility.ProtectedOrInternal;
            }

            return false;
        }
    }
}
