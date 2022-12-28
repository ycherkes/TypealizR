﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using TypealizR.Extensions;

namespace TypealizR.Builder;
internal class StringTypealizRClassModel
{
    public IEnumerable<string> Usings => usings;
    public string Visibility => Target.Visibility.ToString().ToLower();
	public string TypeName => $"StringTypealizR_{Target.FullNameForClassName}";

    public readonly TypeModel Target;

    private readonly HashSet<string> usings = new()
    {
		"TypealizR.Abstractions"
	};

    public IEnumerable<Diagnostic> Diagnostics { get; }

	public StringTypealizRClassModel(TypeModel target, string rootNamespace)
    {
		Target = target;
        usings.Add(rootNamespace);

        Diagnostics = Enumerable.Empty<Diagnostic>();
    }

	public string FileName => $"StringTypealizR_{Target.FullName}.g.cs";

	public string ToCSharp(Type generatorType) => $$"""
        // <auto-generated/>
        {{Usings.Select(x => $"using {x};").ToMultiline(false)}}
        namespace {{Target.Namespace}}.TypealizR
        {

            {{generatorType.GeneratedCodeAttribute()}}
            {{Visibility}} partial class {{TypeName}} : IStringTypealizR<{{Target.Name}}>
            {
                private readonly IStringLocalizer<{{Target.Name}}> that;
            }
        }
        """;
}