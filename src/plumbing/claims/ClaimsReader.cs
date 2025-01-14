namespace SampleApi.Plumbing.Claims
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Newtonsoft.Json.Linq;
    using SampleApi.Plumbing.Configuration;
    using SampleApi.Plumbing.Errors;

    /*
     * A simple utility class to read claims into objects
     */
    public static class ClaimsReader
    {
        /*
         * Read the claims from an access token
         */
        public static IEnumerable<Claim> AccessTokenClaims(string json, OAuthConfiguration configuration)
        {
            var claimsSet = JObject.Parse(json);
            var claims = new List<Claim>();
            claims.Add(ReadClaim(claimsSet, StandardClaimNames.Issuer));

            if (!string.IsNullOrWhiteSpace(configuration.Audience))
            {
                claims.Add(ReadClaim(claimsSet, StandardClaimNames.Audience));
            }

            claims.Add(ReadClaim(claimsSet, StandardClaimNames.Subject));
            claims.Add(ReadClaim(claimsSet, StandardClaimNames.Scope));
            claims.Add(ReadClaim(claimsSet, StandardClaimNames.Exp));
            claims.Add(ReadClaim(claimsSet, StandardClaimNames.Exp));
            return claims;
        }

        /*
         * Return user info claims from a JSON object received in an HTTP response
         */
        public static IEnumerable<Claim> UserInfoClaims(string json)
        {
            var claimsSet = JObject.Parse(json);
            var claims = new List<Claim>();
            claims.Add(ReadClaim(claimsSet, StandardClaimNames.GivenName));
            claims.Add(ReadClaim(claimsSet, StandardClaimNames.FamilyName));
            claims.Add(ReadClaim(claimsSet, StandardClaimNames.Email));
            return claims;
        }

        /*
         * Read a claim from a ClaimsPrincipal and report missing claims clearly
         */
        public static Claim ReadClaim(ClaimsPrincipal principal, string name)
        {
            var found = principal.Claims.FirstOrDefault(c => c.Type == name);
            if (found == null)
            {
                throw ErrorUtils.FromMissingClaim(name);
            }

            return found;
        }

        /*
         * Read a claim from JSON and report missing claims clearly
         */
        private static Claim ReadClaim(JObject claimsSet, string name)
        {
            var value = claimsSet.GetValue(name);
            if (value == null)
            {
                throw ErrorUtils.FromMissingClaim(name);
            }

            return new Claim(name, value.ToString());
        }
    }
}
