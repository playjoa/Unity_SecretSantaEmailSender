using System.Collections;
using AppWide.AppInitialization.Controller;

namespace AppWideSystems.AppInitialization.Interfaces
{
    public interface IAppWideSystem
    {
        string AppSystemName { get; }
        IEnumerator Initiate(AppInitializationController appInitializationController);
    }
}