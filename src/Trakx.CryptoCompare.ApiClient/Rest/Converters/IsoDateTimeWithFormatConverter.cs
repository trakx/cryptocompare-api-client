using Newtonsoft.Json.Converters;

namespace Trakx.CryptoCompare.ApiClient.Rest.Converters
{
    public class IsoDateTimeWithFormatConverter : IsoDateTimeConverter
    {
        public IsoDateTimeWithFormatConverter(string format)
        {
            this.DateTimeFormat = format;
        }
    }
}
