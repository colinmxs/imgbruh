using System;
using System.Collections.Generic;
using System.Fabric;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric.Description;
using System.Text;

namespace imgbruh.services.upload
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class upload : StatelessService
    {
        public upload(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener(context => this.CreateInputListener(context)) };
        }

        private ICommunicationListener CreateInputListener(ServiceContext context)
        {
            // Service instance's URL is the node's IP & desired port
            EndpointResourceDescription inputEndpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");

            // This is the public-facing URL that HTTP clients, e.g., web browsers, can connect to.
            // The "alphabetpartitions" path is a unique URL prefix for this service so that other
            // services that might be hosted on the same node can also use this port with their own unique URL prefix.
            string uriPrefix = String.Format("{0}://+:{1}/upload/", inputEndpoint.Protocol, inputEndpoint.Port);

            // The published URL is slightly different from the listening URL prefix.
            // The listening URL is given to HttpListener.
            // The published URL is the URL that is published to the Service Fabric Naming Service,
            // which is used for service discovery. Clients will ask for this address through that discovery service.
            // The address that clients get needs to have the actual IP or FQDN of the node in order to connect,
            // so we need to replace '+' with the node's IP or FQDN.
            string uriPublished = uriPrefix.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);

            return new HttpCommunicationListener(uriPrefix, uriPublished, this.ProcessInputRequest);
        }

        private async Task ProcessInputRequest(HttpListenerContext context, CancellationToken cancelRequest)
        {
            String output = "Tubular!";

            

            try
            {
                var x = 2 + 2;
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }

            using (HttpListenerResponse response = context.Response)
            {
                if (output != null)
                {
                    response.ContentType = "text/html";

                    byte[] outBytes = Encoding.UTF8.GetBytes(output);
                    response.OutputStream.Write(outBytes, 0, outBytes.Length);
                }
            }
        }
    }
}
