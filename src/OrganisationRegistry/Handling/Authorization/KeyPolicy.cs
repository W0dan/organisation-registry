namespace OrganisationRegistry.Handling.Authorization
{
    using System;
    using System.Linq;
    using Configuration;
    using Infrastructure.Authorization;
    using Organisation.Exceptions;

    public class KeyPolicy : ISecurityPolicy
    {
        private readonly string _ovoNumber;
        private readonly bool _underVlimpersManagement;
        private readonly Guid[] _keyTypeIds;
        private readonly IOrganisationRegistryConfiguration _configuration;

        public KeyPolicy(string ovoNumber,
            bool underVlimpersManagement,
            IOrganisationRegistryConfiguration configuration,
            params Guid[] keyTypeIds)
        {
            _ovoNumber = ovoNumber;
            _underVlimpersManagement = underVlimpersManagement;
            _keyTypeIds = keyTypeIds;
            _configuration = configuration;
        }

        public AuthorizationResult Check(IUser user)
        {
            if(user.IsInRole(Role.OrganisationRegistryBeheerder))
                return AuthorizationResult.Success();

            var containsVlimpersKey = ContainsVlimpersKey(_keyTypeIds);
            var containsOrafinKey = ContainsOrafinKey(_keyTypeIds);

            if(containsOrafinKey
               && !user.IsInRole(Role.Orafin))
                return AuthorizationResult.Fail(new InsufficientRights());

            if (containsVlimpersKey &&
                _underVlimpersManagement &&
                user.IsInRole(Role.VlimpersBeheerder))
                return AuthorizationResult.Success();

            if(!_underVlimpersManagement &&
               user.IsOrganisatieBeheerderFor(_ovoNumber))
                return AuthorizationResult.Success();

            return AuthorizationResult.Fail(new InsufficientRights());
        }

        private bool ContainsOrafinKey(Guid[] keyTypeIds)
        {
            var keyTypeIdsAllowedByVlimpers = _configuration.Authorization.KeyIdsAllowedOnlyForOrafin;
            return keyTypeIds.Any(labelTypeId =>
                keyTypeIdsAllowedByVlimpers.Contains(labelTypeId));
        }

        private bool ContainsVlimpersKey(Guid[] keyTypeIds)
        {
            var keyTypeIdsAllowedByVlimpers = _configuration.Authorization.KeyIdsAllowedOnlyForOrafin;
            return keyTypeIds.Any(labelTypeId =>
                keyTypeIdsAllowedByVlimpers.Contains(labelTypeId));
        }
    }
}
