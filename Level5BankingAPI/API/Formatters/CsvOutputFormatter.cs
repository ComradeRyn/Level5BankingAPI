using System.Text;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace API.Formatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add("text/csv");
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
        => typeof(ICsvFormatter).IsAssignableFrom(type) || typeof(IEnumerable<ICsvFormatter>).IsAssignableFrom(type);

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        if (context.Object is IEnumerable<ICsvFormatter> formattableEnumerable)
        {
            using var formattableEnumerator = formattableEnumerable.GetEnumerator();
            if (!formattableEnumerator.MoveNext())
            {
                await context.HttpContext.Response.WriteAsync("", selectedEncoding);
                
                return;
            }
            
            var buffer = new StringBuilder();
            buffer.Append(formattableEnumerator.Current.CreateHeader());
            do
            {
                buffer.Append(formattableEnumerator.Current.FormatCsv());
            } while (formattableEnumerator.MoveNext());
            
            await context.HttpContext.Response.WriteAsync(buffer.ToString(), selectedEncoding);
            
            return;
        }
        
        var formattableObject = context.Object as ICsvFormatter;
        var formattedData = $"{formattableObject!.CreateHeader()}{formattableObject.FormatCsv()}";
        
        await context.HttpContext.Response.WriteAsync(formattedData, selectedEncoding);
    }
}