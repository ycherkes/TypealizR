﻿using System;using System.Collections.Generic;using System.Linq;using System.Text;using Microsoft.CodeAnalysis.CSharp.Syntax;using TypealizR.Extensions;namespace TypealizR.Core;internal class MemberName{    //refactor me    private readonly string name;    public MemberName(string raw)    {        var value = new string(raw.SkipWhile(x => !x.IsValidInIdentifier(true)).ToArray());                value = value.RemoveAndReplaceDuplicatesOf(" ", "@");        value = value.Replace(".", "");        value = new string(            value                .Trim('_')                .Select((x, i) => x.IsValidInIdentifier(i == 0) ? x : ' ')                .ToArray()        );        value = string.Join(" ",                value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)                    .Where(x => !string.IsNullOrEmpty(x))                )                .Replace("___", "__")                .ReplaceInvalidForMemberNameWith('_');        name = value.Trim('_');    }    public static implicit operator string (MemberName that) => that.name;    public override string ToString() => name;}