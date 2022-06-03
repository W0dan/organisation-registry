﻿namespace OrganisationRegistry.Api.HostedServices.ProcessImportedFiles.Validators;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Be.Vlaanderen.Basisregisters.Api.Search.Helpers;
using SqlServer.Organisation;

public static class ParentValidityExpired
{
    public static ValidationIssue? Validate(
        ImmutableList<OrganisationListItem> organisationsCache,
        DateOnly today,
        int rowNumber,
        DeserializedRecord record)
        => ValidationIssuesFactory.Create(
            rowNumber,
            CheckParent(organisationsCache, today, record).ToList(),
            FormatMessage);

    private static IEnumerable<string> CheckParent(
        ImmutableList<OrganisationListItem> organisationsCache,
        DateOnly today,
        DeserializedRecord record)
    {
        if (record.Parent.Value is not { } parent || parent.IsNullOrWhiteSpace()) yield break;

        var parents = organisationsCache.Where(
            org => org.OvoNumber.Equals(
                parent,
                StringComparison.InvariantCultureIgnoreCase))
            .ToImmutableList();

        if (!parents.Any()) yield break;

        if (parents.Any(org => org.ValidTo is not { } validTo || validTo > today.ToDateTime(new TimeOnly())))
            yield break;

        yield return parent;
    }

    public static string FormatMessage(string parent)
        => $"Parent met Ovo nummer {parent} is reeds afgesloten.";
}
