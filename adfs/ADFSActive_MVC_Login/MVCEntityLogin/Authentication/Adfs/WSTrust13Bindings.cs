using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Security;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net;
using System.IdentityModel;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.IdentityModel.Services;
using System.Threading;
using System.Security.Claims;
using System.Xml;
using System.IO;
using System.IdentityModel.Services.Configuration;
using System.ServiceModel.Description;



namespace MVCEntityLogin.Authentication.Adfs
{
    /*
     *  FROM: http://www.fancy-development.net/wstrust-bindings-for-adfs-2-0-in-net-4-5
     */
    public static class WSTrust13Bindings
    {
        /// <summary>
        /// Binding to talk to kerberosmixed endpoint
        /// </summary>
        public static Binding KerberosMixed
        {
            get { return CreateKerberosBinding(SecurityMode.TransportWithMessageCredential); }
        }

        /// <summary>
        /// Binding to talk to username endpoint
        /// </summary>
        public static Binding Username
        {
            get { return CreateUserNameBinding(SecurityMode.Message); }
        }

        /// <summary>
        /// Binding to talk to usernamebasictransport endpoint
        /// </summary>
        public static Binding UsernameBasicTransport
        {
            get { return CreateUserNameBinding(SecurityMode.Transport); }
        }

        /// <summary>
        /// Binding to talk to usernamemixed endpoint
        /// </summary>
        public static Binding UsernameMixed
        {
            get { return CreateUserNameBinding(SecurityMode.TransportWithMessageCredential); }
        }

        /// <summary>
        /// Creates a username binding with the specified security mode.
        /// </summary>
        private static Binding CreateUserNameBinding(SecurityMode securityMode)
        {
            if (securityMode == SecurityMode.None)
                throw new ArgumentException("securityMode None is not allowed");

            BindingElementCollection bindingElements = new BindingElementCollection();

            // Add securtiy binding element
            if (securityMode == SecurityMode.Message)
                bindingElements.Add(
                    SecurityBindingElement.CreateUserNameForCertificateBindingElement());
            else if (securityMode == SecurityMode.TransportWithMessageCredential)
                bindingElements.Add(
                    SecurityBindingElement.CreateUserNameOverTransportBindingElement());

            // Add encoding binding element
            bindingElements.Add(CreateEncodingBindingElement());

            // Add transport binding element
            HttpTransportBindingElement transportBindingElement =
                CreateTransportBindingElement(securityMode);

            if (securityMode == SecurityMode.Transport)
                transportBindingElement.AuthenticationScheme = AuthenticationSchemes.Basic;
            else
                transportBindingElement.AuthenticationScheme = AuthenticationSchemes.Digest;

            bindingElements.Add(transportBindingElement);

            // Create binding
            return new CustomBinding(bindingElements);
        }

        /// <summary>
        /// Creates a kerberos binding with the specified security mode.
        /// On ADFS 2.0 only TransportWithMessageCredential is available.
        /// </summary>
        private static Binding CreateKerberosBinding(SecurityMode securityMode)
        {
            if (securityMode == SecurityMode.None)
                throw new ArgumentException("securityMode None is not allowed");

            BindingElementCollection bindingElements = new BindingElementCollection();

            // Add securtiy binding element
            if (securityMode == SecurityMode.Message)
                bindingElements.Add(
                    SecurityBindingElement.CreateKerberosBindingElement());
            else if (securityMode == SecurityMode.TransportWithMessageCredential)
                bindingElements.Add(
                    SecurityBindingElement.CreateKerberosOverTransportBindingElement());

            // Add encoding binding element
            bindingElements.Add(CreateEncodingBindingElement());

            // Add transport binding element
            HttpTransportBindingElement transportBindingElement =
                CreateTransportBindingElement(securityMode);

            transportBindingElement.AuthenticationScheme = AuthenticationSchemes.Negotiate;
            bindingElements.Add(transportBindingElement);

            // Create binding
            return new CustomBinding(bindingElements);
        }

        private static HttpTransportBindingElement CreateTransportBindingElement(SecurityMode securityMode)
        {
            // Create transport binding element
            if (securityMode == SecurityMode.Message)
                return new HttpTransportBindingElement();
            else
                return new HttpsTransportBindingElement();
        }

        private static TextMessageEncodingBindingElement CreateEncodingBindingElement()
        {
            // Create encoding binding element
            TextMessageEncodingBindingElement encodingBindingElement =
                new TextMessageEncodingBindingElement();

            encodingBindingElement.ReaderQuotas.MaxArrayLength = 2097152;
            encodingBindingElement.ReaderQuotas.MaxStringContentLength = 2097152;

            return encodingBindingElement;
        }
    }
}
