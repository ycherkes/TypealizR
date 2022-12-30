﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using TypealizR.Core;using TypealizR.Diagnostics;

namespace TypealizR.Builder;
internal partial class ExtensionClassBuilder
{
	private readonly TypeModel markerType;
	private readonly string rootNamespace;

	public ExtensionClassBuilder(TypeModel markerType, string rootNamespace)
	{
		this.markerType = markerType;
		this.rootNamespace = rootNamespace;
	}    private DeduplicatingCollection<ExtensionMethodModel> methods = new();

	public ExtensionClassBuilder WithExtensionMethod(string key, string value, DiagnosticsCollector diagnostics)
	{
		var builder = new ExtensionMethodBuilder(markerType, key, value, diagnostics);
		var model = builder.Build();
        methods.Add(model, diagnostics);
		return this;
	}

	public ExtensionClassModel Build()
	{
		return new(markerType, rootNamespace, methods.Items);
    }
}
