using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("WebApi.Template.Tests")]
//Below statement will avoid System.ArgumentException
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
