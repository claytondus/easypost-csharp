using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp.Portable;


namespace EasyPost {
    public class Webhook : Resource {
        public string id { get; set; }
        public string mode { get; set; }
        public string url { get; set; }
        public DateTime? disabled_at { get; set; }

        /// <summary>
        /// Get a list of scan forms.
        /// </summary>
        /// <returns>List of EasyPost.Webhook insteances.</returns>
        public static async Task<List<Webhook>> List(Dictionary<string, object> parameters = null) {
            Request request = new Request("webhooks");

            WebhookList webhookList = await request.Execute<WebhookList>();
            return webhookList.webhooks;
        }

        /// <summary>
        /// Retrieve a Webhook from its id.
        /// </summary>
        /// <param name="id">String representing a webhook. Starts with "hook_".</param>
        /// <returns>EasyPost.User instance.</returns>
        public static async Task<Webhook> Retrieve(string id) {
            Request request = new Request("webhooks/{id}");
            request.AddUrlSegment("id", id);

            return await request.Execute<Webhook>();
        }

        /// <summary>
        /// Create a Webhook.
        /// </summary>
        /// <param name="parameters">
        /// Dictionary containing parameters to create the carrier account with. Valid pairs:
        ///   * { "url", string } Url of the webhook that events will be sent to.
        /// All invalid keys will be ignored.
        /// </param>
        /// <returns>EasyPost.Webhook instance.</returns>
        public static async Task<Webhook> Create(Dictionary<string, object> parameters) {
            Request request = new Request("webhooks", Method.POST);
            request.AddBody(parameters, "webhook");

            return await request.Execute<Webhook>();
        }

        /// <summary>
        /// Enable a Webhook that has been disabled previously.
        /// </summary>
        public async Task Update() {
            Request request = new Request("webhooks/{id}", Method.PUT);
            request.AddUrlSegment("id", id);

            Merge(await request.Execute<Webhook>());
        }

        public async Task Destroy() {
            Request request = new Request("webhooks/{id}", Method.DELETE);
            request.AddUrlSegment("id", id);
            await request.Execute();
        }
    }
}
