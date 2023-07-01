using Mihcelle.Hwavmvid.Server.Data;

namespace Mihcelle.Hwavmvid.Server.Tasks
{
    public interface IHostedservicebase
    {

        public int Interval { get; set; }
        public Task Runtaskimplementation(Applicationdbcontext frameworkapplicationdbcontext);

    }
}
