using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPost {
    public class ShipmentList : Resource {
        public List<Shipment> shipments { get; set; }
        public bool has_more { get; set; }

        public Dictionary<string, object> filters { get; set; }

        /// <summary>
        /// Get the next page of shipments based on the original parameters passed to Shipment.List().
        /// </summary>
        /// <returns>A new EasyPost.ShipmentList instance.</returns>
        public async Task<ShipmentList> Next() {
            filters = filters ?? new Dictionary<string, object>();
            filters["before_id"] = shipments.Last().id;

            return await Shipment.List(filters);
        }
    }
}