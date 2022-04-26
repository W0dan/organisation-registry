﻿namespace OrganisationRegistry.Api.Backoffice.Organisation.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Handling.Authorization;
    using Infrastructure.Search;
    using Infrastructure.Search.Filtering;
    using Infrastructure.Search.Sorting;
    using OrganisationRegistry.Infrastructure.AppSpecific;
    using OrganisationRegistry.Infrastructure.Authorization;
    using SqlServer.Infrastructure;
    using SqlServer.Organisation;

    public class OrganisationRegulationListQueryResult
    {
        public Guid OrganisationRegulationId { get; set; }
        public string? RegulationThemeName { get; set; }
        public string? RegulationSubThemeName { get; }
        public string? Name { get; set; }
        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public bool IsActive { get; }

        public bool IsEditable { get; }

        public OrganisationRegulationListQueryResult(
            Guid organisationRegulationId,
            string? regulationThemeName,
            string? regulationSubThemeName,
            string? name,
            DateTime? validFrom,
            DateTime? validTo,
            string ovoNumber,
            IUser user)
        {
            OrganisationRegulationId = organisationRegulationId;
            RegulationThemeName = regulationThemeName;
            RegulationSubThemeName = regulationSubThemeName;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Name = name;

            IsActive = new Period(new ValidFrom(validFrom), new ValidTo(validTo)).OverlapsWith(DateTime.Today);

            IsEditable = new RegulationPolicy().Check(user).IsSuccessful;
        }
    }

    public class OrganisationRegulationListQuery : Query<OrganisationRegulationListItem, OrganisationRegulationListItemFilter, OrganisationRegulationListQueryResult>
    {
        private readonly OrganisationRegistryContext _context;
        private readonly IMemoryCaches _memoryCaches;
        private readonly IUser _user;
        private readonly Guid _organisationId;

        protected override ISorting Sorting => new OrganisationRegulationListSorting();

        protected override Expression<Func<OrganisationRegulationListItem, OrganisationRegulationListQueryResult>> Transformation =>
            x => new OrganisationRegulationListQueryResult(
                x.OrganisationRegulationId,
                x.RegulationThemeName,
                x.RegulationSubThemeName,
                x.Name,
                x.ValidFrom,
                x.ValidTo,
                _memoryCaches.OvoNumbers[x.OrganisationId],
                _user);

        public OrganisationRegulationListQuery(
            OrganisationRegistryContext context,
            IMemoryCaches memoryCaches,
            IUser user,
            Guid organisationId)
        {
            _context = context;
            _memoryCaches = memoryCaches;
            _user = user;
            _organisationId = organisationId;
        }

        protected override IQueryable<OrganisationRegulationListItem> Filter(FilteringHeader<OrganisationRegulationListItemFilter> filtering)
        {
            var organisationRegulations = _context.OrganisationRegulationList
                .AsQueryable()
                .Where(x => x.OrganisationId == _organisationId).AsQueryable();

            if (filtering.Filter is not { } filter)
                return organisationRegulations;

            if (filter.ActiveOnly)
                organisationRegulations = organisationRegulations.Where(x =>
                    (!x.ValidFrom.HasValue || x.ValidFrom <= DateTime.Today) &&
                    (!x.ValidTo.HasValue || x.ValidTo >= DateTime.Today));

            return organisationRegulations;
        }

        private class OrganisationRegulationListSorting : ISorting
        {
            public IEnumerable<string> SortableFields { get; } = new[]
            {
                nameof(OrganisationRegulationListItem.RegulationThemeName),
                nameof(OrganisationRegulationListItem.Url),
                nameof(OrganisationRegulationListItem.ValidFrom),
                nameof(OrganisationRegulationListItem.ValidTo)
            };

            public SortingHeader DefaultSortingHeader { get; } =
                new SortingHeader(nameof(OrganisationRegulationListItem.RegulationThemeName), SortOrder.Ascending);
        }
    }

    public class OrganisationRegulationListItemFilter
    {
        public bool ActiveOnly { get; set; }
    }
}
