﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Xml;
using Microsoft.CodeAnalysis;

namespace TypealizR.SourceGenerators.StringLocalizer;

internal class ClassModel
{
    private static string generatorName = typeof(SourceGenerator).FullName;
    private static Version generatorVersion = typeof(SourceGenerator).Assembly.GetName().Version;

    private TypeInfo target;

    private readonly string members;

    public IEnumerable<MethodModel> Methods { get; }
    public IEnumerable<Diagnostic> Warnings { get; }

	public ClassModel(TypeInfo target, IEnumerable<MethodModel> methods, IEnumerable<Diagnostic> warnings)
    {
        this.target = target;
        Methods = methods;
		Warnings = warnings;
		members = string.Join("\r", methods
            .Select(x => x.Declaration)
            .ToArray()
        );
    }

    public string FileName => $"IStringLocalizerExtensions_{target.FullName}";

    public string Body => $@"
// <auto-generated/>
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Localization;
using {target.Namespace};
namespace Microsoft.Extensions.Localization;
[
    GeneratedCode(""{generatorName}"", ""{generatorVersion}""),
    DebuggerStepThrough,
    ExcludeFromCodeCoverage(Justification = ""generated code"")
]
internal static partial class IStringLocalizerExtensions_{target.Name}
{{
{members}
}}
";

}