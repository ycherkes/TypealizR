﻿using System.Collections.Generic;
using System.Linq;
using TypealizR.Diagnostics;

namespace TypealizR.Builder;
internal sealed class MemberBuilderContext<TBuilder> where TBuilder : IMemberBuilder
{
	public MemberBuilderContext(TBuilder builder, DiagnosticsCollector diagnostics)
	{
		Builder = builder;
		Diagnostics = diagnostics;
	}

	public TBuilder Builder { get; }
	public DiagnosticsCollector Diagnostics { get; }
}

internal static class MethodModelContextExtensions
{
	internal static IEnumerable<IMemberModel> Deduplicate(this MemberModelContext[] that)
	{
		var groupByMethodName = that.GroupBy(x => x.Model.Name);
		var deduplicatedMethods = new List<MemberModelContext>(that.Count());

		foreach (var methodGroup in groupByMethodName)
		{
			if (methodGroup.Count() == 1)
			{
				deduplicatedMethods.Add(methodGroup.Single());
				continue;
			}

			int discriminator = 1;
			foreach (var duplicate in methodGroup.Skip(1))
			{
				duplicate.Diagnostics.Add(fac => fac.AmbigiousRessourceKey_0002(duplicate.Model.Name));
				duplicate.Model.DeduplicateWith(discriminator++);
			}

			deduplicatedMethods.AddRange(methodGroup);
		}

		return deduplicatedMethods
			.Select(x => x.Model)
			.ToArray();

	}
}
