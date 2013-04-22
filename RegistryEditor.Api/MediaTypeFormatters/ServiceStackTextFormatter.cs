using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace RegistryEditor.Api.MediaTypeFormatters
{
	public class ServiceStackTextFormatter : MediaTypeFormatter
	{
		public ServiceStackTextFormatter()
		{
			JsConfig.DateHandler = JsonDateHandler.ISO8601;
			SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			SupportedEncodings.Add(new UTF8Encoding(false, true));
		}

		public override bool CanReadType(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			return true;
		}

		public override bool CanWriteType(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			return true;
		}

		public override Task<object> ReadFromStreamAsync(Type type, System.IO.Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
		{
			return Task<object>.Factory.StartNew(() => JsonSerializer.DeserializeFromStream(type, readStream));
		}

		public override Task WriteToStreamAsync(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
		{
			return Task.Factory.StartNew(() => JsonSerializer.SerializeToStream(value, type, writeStream));
		}
	}
}