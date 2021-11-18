using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandmarAdaptor
{
  class RandmarResellerAdaptor
  {
    private readonly string resellerId;

    public RandmarResellerAdaptor()
    {
      resellerId = RandmarApiHandler.ResellerId;
    }

    public async Task<bool> CartExists(string cartName)
    {
      return (await GetCart(cartName)) != null;
    }

    public async Task<dynamic> GetCart(string cartName)
    {
      return await RandmarApiHandler.Get<dynamic>($"Reseller/{resellerId}/Cart/{cartName}");
    }

    public async Task<dynamic> GetProduct(string manufacturerPartNumberOrRandmarSKU)
    {
      return await RandmarApiHandler.Get<dynamic>($"Reseller/{resellerId}/Product/{manufacturerPartNumberOrRandmarSKU}");
    }

    public async Task<IEnumerable<dynamic>> SearchProducts(string query)
    {
      return await RandmarApiHandler.Get<IEnumerable<dynamic>>($"Reseller/{resellerId}/Search?s={query}");
    }

    public async Task<bool> InsertProductToCart(string cartName, string randmarSKU, int quantityOrdered, string bidNumber = null)
    {
      string bidQuery = string.IsNullOrEmpty(bidNumber) ? string.Empty : string.Format("&bidNumber={0}", bidNumber);
      return await RandmarApiHandler.Post<bool>($"Reseller/{resellerId}/Carts/AddItem/{cartName}/{randmarSKU}?quantity={quantityOrdered}{bidQuery}");
    }

    public async Task<string> ProcessCart(string cartName,
                                            string poNumber,
                                            string shipToName,
                                            string shipToAddress1,
                                            string shipToAddress2,
                                            string shipToCity,
                                            string shipToState,
                                            string shiptToPostalCode,
                                            string enduserPO = null,
                                            string comment = null,
                                            string shippingslipComment = null,
                                            string contactName = null,
                                            string contactPhone = null,
                                            string shipVia = null,
                                            bool allowPartialShipment = true)
    {
      var contentBody = new
      {
        Name = shipToName,
        Street1 = shipToAddress1,
        Street2 = shipToAddress2,
        City = shipToCity,
        Province = shipToState,
        PostalCode = shiptToPostalCode,
        PO = poNumber,
        CustomerPO = enduserPO ?? string.Empty,
        Comment = comment ?? string.Empty,
        ShippingSlipComment = shippingslipComment ?? string.Empty,
        ContactName = contactName ?? string.Empty,
        ContactPhone = contactPhone ?? string.Empty,
        AllowPartialShipment = allowPartialShipment,
        ShippingMethodId = shipVia ?? string.Empty
      };

      dynamic obj = await RandmarApiHandler.Post<dynamic>($"Reseller/{resellerId}/Carts/Process/{cartName}", contentBody);

      var orderNumber = (string)obj.OrderNumber;
      if (orderNumber.StartsWith("OW")) return orderNumber;

      throw new InvalidOperationException(obj);
    }

    public async Task<IEnumerable<dynamic>> GetOrders()
    {
      return await RandmarApiHandler.Get<IEnumerable<dynamic>>($"Reseller/{resellerId}/Orders");
    }

    public async Task<IEnumerable<dynamic>> GetShipments()
    {
      return await RandmarApiHandler.Get<IEnumerable<dynamic>>($"Reseller/{resellerId}/Orders/Shipments");
    }

    public async Task<IEnumerable<dynamic>> GetInvoices()
    {
      return await RandmarApiHandler.Get<IEnumerable<dynamic>>($"Reseller/{resellerId}/Billing/Invoices");
    }

    public async Task<IEnumerable<dynamic>> GetCredits()
    {
      return await RandmarApiHandler.Get<IEnumerable<dynamic>>($"Reseller/{resellerId}/Billing/Credits");
    }

    public async Task<IEnumerable<dynamic>> GetReturns()
    {
      return await RandmarApiHandler.Get<IEnumerable<dynamic>>($"Reseller/{resellerId}/Returns");
    }

    public async Task<dynamic> GetProfile()
    {
      return await RandmarApiHandler.Get<dynamic>($"Reseller/{resellerId}/Account/General");
    }
  }
}