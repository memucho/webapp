using System;
using System.Web;
using RollbarSharp;
using Serilog;

public class Logg
{
    private static ILogger _logger;

    public static ILogger r()
    {
        if (_logger == null) {     
            _logger = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
        }

        return _logger;
    }

    public static void Error(Exception exception)
    {
        try
        {
            if (HttpContext.Current == null)
            {
                Logg.r().Error(exception, "Error");
                return;
            }

            var request = HttpContext.Current.Request;

            Logg.r().Error(exception, "PageError {Url} {Headers}",
                request.RawUrl,
                request.Headers.ToString());

            if (!request.IsLocal)
                new RollbarClient().SendException(exception);
        }
        catch
        {
        }
    }
}