using Mihcelle.Hwavmvid.Shared.Models;

namespace Mihcelle.Hwavmvid.Server
{
    public interface Moduleinstallerinterface
    {

        Task Removemodule(string id);
        Task Install();
        Task Deinstall();
        Applicationmodulepackage applicationmodulepackage { get; }

    }
}
