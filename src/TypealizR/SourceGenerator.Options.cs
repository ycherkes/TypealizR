﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using TypealizR.Diagnostics;

namespace TypealizR;

public partial class SourceGenerator
{
	internal sealed class Options
    {
		public const string MSBUILD_PROJECT_DIRECTORY = "build_property.msbuildprojectdirectory";
		public const string PROJECT_DIR = "build_property.projectdir";
		public const string ROOT_NAMESPACE = "build_property.rootnamespace";

		public Options(string? projectDirectory, string? rootNamespace, IDictionary<string, DiagnosticSeverity> severityConfig)
        {
            RootNamespace = rootNamespace ?? "";
			SeverityConfig = severityConfig;
			ProjectDirectory = new DirectoryInfo(projectDirectory);
        }

        public DirectoryInfo ProjectDirectory { get; }
        public string RootNamespace { get; }
		public IDictionary<string, DiagnosticSeverity> SeverityConfig { get; }

		public static Options From(AnalyzerConfigOptions options)
		{
			if (!options.TryGetValue(MSBUILD_PROJECT_DIRECTORY, out var projectDirectory))
			{
				options.TryGetValue(PROJECT_DIR, out projectDirectory);
			}

			options.TryGetValue(ROOT_NAMESPACE, out var rootNamespace);

			var severityConfig = ReadSeverityConfig(options);

			return new(
				projectDirectory: projectDirectory ?? Guid.NewGuid().ToString(),
				rootNamespace: rootNamespace ?? Guid.NewGuid().ToString(),
				severityConfig: severityConfig
			);
		}
		private static IDictionary<string, DiagnosticSeverity> ReadSeverityConfig(AnalyzerConfigOptions options)
		{
			var severityConfig = new Dictionary<string, DiagnosticSeverity>();

			var availableDiagnostics = Enum.GetValues(typeof(DiagnosticsId))
				.OfType<DiagnosticsId>()
				.Select(x => x.ToString()
			);

			foreach (var diagnostic in availableDiagnostics)
			{
				var key = $"dotnet_diagnostic_{diagnostic.ToLower()}_severity";

				if (options.TryGetValue(key, out var rawValue))
				{
					if (Enum.TryParse<DiagnosticSeverity>(rawValue, true, out var severity))
					{
						severityConfig[diagnostic] = severity;
					}
					else
					{
						throw new InvalidOperationException($"'{key}' has invalid value '{rawValue}'");
					}
				}
			}

            return severityConfig;
		}
	}
}
