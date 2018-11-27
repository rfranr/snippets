using MVCEntityLogin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services.Configuration;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Web;
using System.Xml;

namespace MVCEntityLogin.Authentication.Adfs
{
    public static class Login 
    {
        public static IEnumerable<Claim> Validate (string user, string password)
        {
            string serviceTokenUrl = ConfigurationManager.AppSettings["ADFS_ServiceTokenUrl_usernamemixed"];
            string relyingPartyURL = ConfigurationManager.AppSettings["ADFS_RelyingPartyURL"];


            WSTrustChannelFactory factory = new WSTrustChannelFactory(
                            WSTrust13Bindings.UsernameMixed,
                            new EndpointAddress(serviceTokenUrl));

            factory.TrustVersion = TrustVersion.WSTrust13;

            RequestSecurityToken rst = new RequestSecurityToken
            {
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Bearer,
                TokenType = "urn:oasis:names:tc:SAML:2.0:assertion",
                AppliesTo = new EndpointReference(relyingPartyURL)
            };


            factory.Credentials.UserName.UserName = user;       // "YOUR_USER_NAME";
            factory.Credentials.UserName.Password = password;   // "***********";

            RequestSecurityTokenResponse rstrr = null;
            SecurityToken issuedToken = factory.CreateChannel().Issue(rst, out rstrr);


            // Validate the token and get the claims
            RequestSecurityTokenResponse r = factory.CreateChannel().Validate(rst);


            // ??
            var genericToken = issuedToken as GenericXmlSecurityToken;

            FederationConfiguration f = new FederationConfiguration(true);
            var handlers = f.IdentityConfiguration.SecurityTokenHandlerCollectionManager.SecurityTokenHandlerCollections.First();

            var cToken = handlers.ReadToken(new XmlTextReader(new StringReader(genericToken.TokenXml.OuterXml)));
            var identity = handlers.ValidateToken(cToken).First();
            var userIdenity = new ClaimsPrincipal(identity);

            Console.WriteLine(userIdenity.Identity.Name);
            Console.WriteLine(userIdenity.Identity.IsAuthenticated);
            foreach (Claim claim in userIdenity.Claims)
            {
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("Type =" + claim.Type);
                Console.WriteLine("Value =" + claim.Value);
                Console.WriteLine("ValueType =" + claim.ValueType);
                Console.WriteLine("Issuer =" + claim.Issuer);
                Console.WriteLine("OriginalIssuer =" + claim.OriginalIssuer);
                Console.WriteLine("Properties =" + claim.Properties);
                Console.WriteLine("Subject =" + claim.Subject);
            }

            return userIdenity.Claims;

        }

    }
}
