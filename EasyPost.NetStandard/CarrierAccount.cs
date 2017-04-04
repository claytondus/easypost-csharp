using RestSharp.Portable;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPost {
    public class CarrierAccount : Resource {
        public string id { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string reference { get; set; }
        public string readable { get; set; }
        public Dictionary<string, object> credentials { get; set; }
        public Dictionary<string, object> test_credentials { get; set; }

        public static async Task<List<CarrierAccount>> List() {
            Request request = new Request("carrier_accounts");
            return await request.Execute<List<CarrierAccount>>();
        }

        /// <summary>
        /// Retrieve a CarrierAccount from its id.
        /// </summary>
        /// <param name="id">String representing a carrier account. Starts with "ca_".</param>
        /// <returns>EasyPost.CarrierAccount instance.</returns>
        public static async Task<CarrierAccount> Retrieve(string id) {
            Request request = new Request("carrier_accounts/{id}");
            request.AddUrlSegment("id", id);

            return await request.Execute<CarrierAccount>();
        }

        /// <summary>
        /// Create a CarrierAccount.
        /// </summary>
        /// <param name="parameters">
        /// Optional dictionary containing parameters to create the carrier account with. Valid pairs:
        ///   * {"type", string} Required (e.g. EndiciaAccount, UPSAccount, etc.).
        ///   * {"reference", string} External reference for carrier account.
        ///   * {"description", string} Description of carrier account.
        ///   * {"credentials", Dictionary<string, string>}
        ///   * {"test_credentials", Dictionary<string, string>}
        /// All invalid keys will be ignored.
        /// </param>
        /// <returns>EasyPost.CarrierAccount instance.</returns>
        public static async Task<CarrierAccount> Create(Dictionary<string, object> parameters) {
            Request request = new Request("carrier_accounts", Method.POST);
            request.AddBody(parameters, "carrier_account");

            return await request.Execute<CarrierAccount>();
        }

        /// <summary>
        /// Remove this CarrierAccount from your account.
        /// </summary>
        public async Task Destroy() {
            Request request = new Request("carrier_accounts/{id}", Method.DELETE);
            request.AddUrlSegment("id", id);

            await request.Execute();
        }

        /// <summary>
        /// Update this CarrierAccount.
        /// </summary>
        /// <param name="parameters">See CarrierAccount.Create for more details.</param>
        public async Task Update(Dictionary<string, object> parameters) {
            Request request = new Request("carrier_accounts/{id}", Method.PUT);
            request.AddUrlSegment("id", id);
            request.AddBody(parameters, "carrier_account");

            Merge(await request.Execute<CarrierAccount>());
        }
    }
}
