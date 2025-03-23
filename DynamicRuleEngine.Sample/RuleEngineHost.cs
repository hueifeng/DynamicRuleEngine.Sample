using System.Reflection;
using System.Runtime.Loader;

namespace DynamicRuleEngine.Sample
{
    public class RuleEngineHost : IDisposable
    {
        private readonly AssemblyLoadContext _loadContext;
        private Assembly _loadedAssembly;

        public RuleEngineHost()
        {
            _loadContext = new AssemblyLoadContext("RuleContext", isCollectible: true);
        }

        public IRuleEngine<CustomerData> LoadRule(string dllPath)
        {
            using var fs = new FileStream(dllPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var assembly = _loadContext.LoadFromStream(fs);
            var typeName = Path.GetFileNameWithoutExtension(dllPath) + "RuleEngine";
            return (IRuleEngine<CustomerData>)Activator.CreateInstance(assembly.GetType(typeName));
        }

        public void Dispose()
        {
            _loadContext.Unload();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
