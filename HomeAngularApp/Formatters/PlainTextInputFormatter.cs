using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HomeAngularApp.Formatters
{
    public class PlainTextInputFormatter : TextInputFormatter
    {
        public PlainTextInputFormatter()
        {
            SupportedMediaTypes.Add("text/plain");

            // TODO: Should probably add more here
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.ASCII);
            SupportedEncodings.Add(Encoding.UTF32);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(string);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            try
            {
                using var reader = new StreamReader(context.HttpContext.Request.Body, encoding);
                var text = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(text))
                {
                    return await InputFormatterResult.NoValueAsync();
                }

                return await InputFormatterResult.SuccessAsync(text);
            }
            catch
            {
                return await InputFormatterResult.FailureAsync();
            }
        }
    }
}
