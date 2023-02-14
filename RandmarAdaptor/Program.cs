using System;
using System.Linq;
using System.Threading.Tasks;

namespace RandmarAdaptor
{
  class Program
  {
    static async Task Main()
    {
      var randmarResellerAdaptor = new RandmarResellerAdaptor();
      var resellerInformation = await randmarResellerAdaptor.GetProfile();

      Console.WriteLine($"Reseller Id: {(string)resellerInformation.ResellerId}");
      Console.WriteLine($"Reseller Name: {(string)resellerInformation.Name}");

      var manufacturerProfile = await randmarResellerAdaptor.GetManufacturer(2010);
      Console.WriteLine($"Manufacturer Name: {(string)manufacturerProfile.PublicName}");

      var manufacturers = await randmarResellerAdaptor.GetManufacturers();
      Console.WriteLine($"Manufacturers count: {manufacturers.Count()}");

      var receivings = await randmarResellerAdaptor.GetReceivings();
      Console.WriteLine($"Receivings count: {receivings.Count()}");
      
      var instantRebates = await randmarResellerAdaptor.GetInstantRebates();
      Console.WriteLine($"Instant Rebates count: {instantRebates.Count()}");

      var orders = await randmarResellerAdaptor.GetOrders();
      Console.WriteLine($"Order count: {orders.Count()}");

      var shipments = await randmarResellerAdaptor.GetShipments();
      Console.WriteLine($"Shipment count: {shipments.Count()}");

      var invoices = await randmarResellerAdaptor.GetInvoices();
      Console.WriteLine($"Invoice count: {invoices.Count()}");

      var credits = await randmarResellerAdaptor.GetCredits();
      Console.WriteLine($"Credit count: {credits.Count()}");

      var returns = await randmarResellerAdaptor.GetReturns();
      Console.WriteLine($"Return count: {returns.Count()}");

      const string searchQuery = "TN450";
      var tn450s = await randmarResellerAdaptor.SearchProducts(searchQuery);
      Console.WriteLine($"Search result count for {searchQuery}: {tn450s.Count()}");

      foreach(var product in tn450s)
      {
        var tn450 = await randmarResellerAdaptor.GetProduct((string)product.Content.MPN);
        Console.WriteLine($"UPC for {(string)tn450.MPN} is  {(string)tn450.UPC}");
      }

    }
  }
}