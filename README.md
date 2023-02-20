# RandmarAdaptor
Randmar Adaptor is an open-source starter project, written as C# console application, to help you get started with Randmar integrations. It allows you to place orders through Randmar's API and retrieve Reseller profile information, orders, shipments, returns, and more.

## Get Started
1. Ensure you have the .NET 5.0 framework installed. If you don't have it downloaded, you can get it [here](https://dotnet.microsoft.com/en-us/download/dotnet/5.0).
2. Clone the GitHub repository and open it in a code editor of your choice (Visual Studio is recommended).
3. Create an API key in the Integrations tab of the [Randmar Partner Dashboard](https://dashboard.randmar.io)

![Screenshot from Partner Dashboard](https://i.imgur.com/i0o5LAW.png)

4. In RandmarApiHandler.cs, replace ClientId with your key name and ClientSecret with your secret key.

![Screenshot from RandmarApiHandler.cs](https://i.imgur.com/7jm2TK6.png)

5. Build and run the project. You should see some example calls run in the console.

## Usage
RandmarResellerAdaptor.cs contains all methods that make calls to Randmar's API. You can see example usage in Program.cs.

