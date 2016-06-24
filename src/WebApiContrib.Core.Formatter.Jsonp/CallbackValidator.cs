using System.Text.RegularExpressions;

namespace WebApiContrib.Core.Formatter.Jsonp
{
    public static class CallbackValidator
    {
        private static readonly Regex JsonpCallbackFormat = new Regex(@"[^0-9a-zA-Z\$_\.]|^(abstract|boolean|break|byte|case|catch|char|class|const|continue|debugger|default|delete|do|double|else|enum|export|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|volatile|void|while|with|NaN|Infinity|undefined)$");

        public static bool IsValid(string callback)
        {
            return !JsonpCallbackFormat.IsMatch(callback);
        }
    }
}