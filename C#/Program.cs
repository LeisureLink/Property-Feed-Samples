using System.Xml;
using PropertyFeedSampleApp.PropertyFeed;

namespace PropertyFeedSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string userName = "VACATION11";
            const string password = "f33der1!";

            using (var client = new DataReceiverServiceSoapClient())
            {
                var supplierIds = client.GetValidRentalUnitIdsForExtract(new AuthHeader { UserName = userName, Password = password }, string.Empty, string.Empty, true);

                foreach (var supplierId in supplierIds)
                {
                    var rentalUnitIds = client.GetValidRentalUnitIdsForExtract(new AuthHeader { UserName = userName, Password = password }, string.Empty, supplierId.ToString(), false);

                    foreach (var rentalUnitId in rentalUnitIds)
                    {
                        //Some properties will return a lot of data.
                        //Be sure to increase the MaxReceivedMessageSize property on the appropriate binding element in your config.
                        //We suggest maxReceivedMessageSize="6553600"
                        string dataExtract = client.DataExtract(new AuthHeader { UserName = userName, Password = password }, "STANDARD", string.Empty, string.Empty, rentalUnitId.ToString());

                        var propertyDoc = new XmlDocument();
                        propertyDoc.LoadXml(dataExtract);

                        if (propertyDoc.DocumentElement.ChildNodes.Count == 0)
                        {
                            //The property is inactive, make it inactive in your system
                        }
                        else
                        {
                            //The property is active, cache the data in your system
                            goto EndOfExample;
                        }
                    }
                }

                EndOfExample:;
            }
        }
    }
}
