using Nancy;

namespace BackOffice.Web
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            this.Get["/"] = parameters =>
            {
                return View["index.html"];
            };
        }
    }
}
