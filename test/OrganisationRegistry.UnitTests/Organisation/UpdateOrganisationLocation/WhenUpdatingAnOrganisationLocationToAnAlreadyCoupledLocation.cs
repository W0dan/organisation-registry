namespace OrganisationRegistry.UnitTests.Organisation.UpdateOrganisationLocation;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Infrastructure.Tests.Extensions.TestHelpers;
using Location;
using OrganisationRegistry.Infrastructure.Events;
using Location.Events;
using Microsoft.Extensions.Logging;
using Moq;
using OrganisationRegistry.Infrastructure.Authorization;
using OrganisationRegistry.Infrastructure.Domain;
using OrganisationRegistry.Organisation;
using OrganisationRegistry.Organisation.Events;
using OrganisationRegistry.Organisation.Exceptions;
using Tests.Shared;
using Xunit;
using Xunit.Abstractions;

public class WhenUpdatingAnOrganisationLocationToAnAlreadyCoupledLocation :
    Specification<UpdateOrganisationLocationCommandHandler, UpdateOrganisationLocation>
{
    private readonly Guid _organisationId;
    private readonly Guid _locationAId;
    private readonly Guid _locationBId;
    private readonly string _ovoNumber;
    private readonly Guid _organisationLocationBId;
    private readonly Guid _organisationLocationAId;

    public WhenUpdatingAnOrganisationLocationToAnAlreadyCoupledLocation(ITestOutputHelper helper) : base(helper)
    {
        _ovoNumber = "OVO000012345";
        _organisationId = Guid.NewGuid();
        _locationAId = Guid.NewGuid();
        _locationBId = Guid.NewGuid();
        _organisationLocationAId = Guid.NewGuid();
        _organisationLocationBId = Guid.NewGuid();
    }

    protected override UpdateOrganisationLocationCommandHandler BuildHandler(ISession session)
        => new(
            new Mock<ILogger<UpdateOrganisationLocationCommandHandler>>().Object,
            session,
            new DateTimeProvider()
        );

    private IUser User
        => new UserBuilder().AddRoles(Role.DecentraalBeheerder).AddOrganisations(_ovoNumber).Build();

    private IEvent[] Events
        => new IEvent[] {
            new OrganisationCreated(
                _organisationId,
                "Kind en Gezin",
                _ovoNumber,
                "K&G",
                Article.None,
                "Kindjes en gezinnetjes",
                new List<Purpose>(),
                false,
                null,
                null,
                null,
                null),
            new LocationCreated(
                _locationAId,
                "12345",
                "Albert 1 laan 32, 1000 Brussel",
                "Albert 1 laan 32",
                "1000",
                "Brussel",
                "Belgie"),
            new LocationCreated(
                _locationBId,
                "12346",
                "Albert 1 laan 34, 1000 Brussel",
                "Albert 1 laan 32",
                "1000",
                "Brussel",
                "Belgie"),
            new OrganisationLocationAdded(
                _organisationId,
                _organisationLocationAId,
                _locationAId,
                "Gebouw A",
                false,
                null,
                "Location Type A",
                null,
                null) { Version = 2 },
            new OrganisationLocationAdded(
                _organisationId,
                _organisationLocationBId,
                _locationBId,
                "Gebouw B",
                false,
                null,
                "Location Type A",
                null,
                null) { Version = 3 }
        };

    private UpdateOrganisationLocation UpdateOrganisationLocationCommand
        => new(
            _organisationLocationBId,
            new OrganisationId(_organisationId),
            new LocationId(_locationAId),
            false,
            null,
            new ValidFrom(),
            new ValidTo(),
            Source.Wegwijs);

    [Fact]
    public async Task PublishesNoEvents()
    {
        await Given(Events).When(UpdateOrganisationLocationCommand, User).ThenItPublishesTheCorrectNumberOfEvents(0);
    }

    [Fact]
    public async Task  ThrowsAnException()
    {
        await Given(Events).When(UpdateOrganisationLocationCommand, User)
            .ThenThrows<LocationAlreadyCoupledToInThisPeriod>()
            .WithMessage("Deze locatie is in deze periode reeds gekoppeld aan de organisatie.");
    }
}
