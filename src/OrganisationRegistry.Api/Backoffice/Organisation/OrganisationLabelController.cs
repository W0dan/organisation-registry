﻿namespace OrganisationRegistry.Api.Backoffice.Organisation
{
    using System;
    using System.Threading.Tasks;
    using Infrastructure;
    using Infrastructure.Search.Filtering;
    using Infrastructure.Search.Pagination;
    using Infrastructure.Search.Sorting;
    using Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using OrganisationRegistry.Infrastructure.Authorization;
    using OrganisationRegistry.Infrastructure.Commands;
    using Queries;
    using Requests;
    using SqlServer.Infrastructure;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [OrganisationRegistryRoute("organisations/{organisationId}/labels")]
    public class OrganisationLabelController : OrganisationRegistryController
    {
        public OrganisationLabelController(ICommandSender commandSender)
            : base(commandSender)
        {
        }

        /// <summary>Get a list of available labels for an organisation.</summary>
        [HttpGet]
        [OrganisationRegistryAuthorize]
        [AllowAnonymous]
        public async Task<IActionResult> Get(
            [FromServices] OrganisationRegistryContext context,
            [FromServices] ISecurityService securityService,
            [FromRoute] Guid organisationId)
        {
            var filtering = Request.ExtractFilteringRequest<OrganisationLabelListItemFilter>();
            var sorting = Request.ExtractSortingRequest();
            var pagination = Request.ExtractPaginationRequest();

            Func<Guid?, bool> canUseLabelType =
                labelTypeId => User.Identity.IsAuthenticated &&
                             securityService.CanUseLabelType(securityService.GetUser(User), labelTypeId.Value);

            var pagedOrganisations = new OrganisationLabelListQuery(context, organisationId, canUseLabelType).Fetch(filtering, sorting, pagination);

            Response.AddPaginationResponse(pagedOrganisations.PaginationInfo);
            Response.AddSortingResponse(sorting.SortBy, sorting.SortOrder);

            return Ok(await pagedOrganisations.Items.ToListAsync());
        }

        /// <summary>Get a label for an organisation.</summary>
        /// <response code="200">If the label is found.</response>
        /// <response code="404">If the label cannot be found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromServices] OrganisationRegistryContext context, [FromRoute] Guid organisationId, [FromRoute] Guid id)
        {
            var organisation = await context.OrganisationLabelList.FirstOrDefaultAsync(x => x.OrganisationLabelId == id);

            if (organisation == null)
                return NotFound();

            return Ok(organisation);
        }

        /// <summary>Create a label for an organisation.</summary>
        /// <response code="201">If the label is created, together with the label.</response>
        /// <response code="400">If the label information does not pass validation.</response>
        [HttpPost]
        [OrganisationRegistryAuthorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromServices] ISecurityService securityService, [FromRoute] Guid organisationId, [FromBody] AddOrganisationLabelRequest message)
        {
            var internalMessage = new AddOrganisationLabelInternalRequest(organisationId, message);

            if (!securityService.CanEditOrganisation(User, internalMessage.OrganisationId))
                ModelState.AddModelError("NotAllowed", "U hebt niet voldoende rechten voor deze organisatie.");

            if (!TryValidateModel(internalMessage))
                return BadRequest(ModelState);

            await CommandSender.Send(AddOrganisationLabelRequestMapping.Map(internalMessage));

            return Created(Url.Action(nameof(Get), new { id = message.OrganisationLabelId }), null);
        }

        /// <summary>Update a label for an organisation.</summary>
        /// <response code="201">If the label is updated, together with the label.</response>
        /// <response code="400">If the label information does not pass validation.</response>
        [HttpPut("{id}")]
        [OrganisationRegistryAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromServices] ISecurityService securityService, [FromRoute] Guid organisationId, [FromBody] UpdateOrganisationLabelRequest message)
        {
            var internalMessage = new UpdateOrganisationLabelInternalRequest(organisationId, message);

            if (!securityService.CanEditOrganisation(User, internalMessage.OrganisationId))
                ModelState.AddModelError("NotAllowed", "U hebt niet voldoende rechten voor deze organisatie.");

            if (!TryValidateModel(internalMessage))
                return BadRequest(ModelState);

            await CommandSender.Send(UpdateOrganisationLabelRequestMapping.Map(internalMessage));

            return Ok();
        }
    }
}
