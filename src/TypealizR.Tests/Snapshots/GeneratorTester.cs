﻿using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace TypealizR.Tests.Snapshots;

internal class GeneratorTester : IVerifiable
{
    private readonly GeneratorDriver driver;
	private readonly string snapshotDirectory;

	public GeneratorTester(GeneratorDriver driver, string snapshotDirectory)
    {
        this.driver = driver;
		this.snapshotDirectory = snapshotDirectory;
	}

    public Task Verify([CallerMemberName]string caller = "") => Verifier
		.Verify(driver)
		.UseFileName(caller)
		.UseDirectory(snapshotDirectory)
	;
}
