﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TypealizR.SourceGenerators.StringLocalizer;

internal class ExtensionClassInfo
{
    private static string generatorName = typeof(SourceGenerator).FullName;
    private static Version generatorVersion = typeof(SourceGenerator).Assembly.GetName().Version;

    private readonly string targetNamespace;
    private readonly string targetTypeName;

    private readonly string members;

    public IEnumerable<ExtensionMethodInfo> Methods { get; }

    public ExtensionClassInfo(string targetNamespace, string targetTypeName, IEnumerable<ExtensionMethodInfo> methods)
    {
        this.targetNamespace = targetNamespace;
        this.targetTypeName = targetTypeName;

        Methods = methods;

        members = string.Join("\r", methods
            .Select(x => x.Declaration)
            .ToArray()
        );
    }

    public string FileName => $"IStringLocalizerExtensions_{targetTypeName}";

    public string Body => $@"
// <auto-generated/>
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Localization;
using {targetNamespace};
namespace Microsoft.Extensions.Localization;
[
    GeneratedCode(""{generatorName}"", ""{generatorVersion}""),
    DebuggerStepThrough,
    ExcludeFromCodeCoverage(Justification = ""generated code"")
]
internal static partial class IStringLocalizerExtensions_{targetTypeName}
{{
{members}
}}
";
}