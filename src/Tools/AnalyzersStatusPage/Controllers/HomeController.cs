namespace RoslynAnalyzersStatus.Web.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using RoslynAnalyzersStatus.Web.Models;

    public class HomeController : Controller
    {
        public ActionResult Index(string category = null,
            bool? hasCSharpImplementation = null,
            bool? hasVBImplementation = null,
            string status = null,
            bool? codeFixStatus = null)
        {
            MainViewModel viewModel = JsonConvert.DeserializeObject<MainViewModel>(System.IO.File.ReadAllText(this.Server.MapPath("~/status.json")));

            var diagnostics = (from x in viewModel.Diagnostics
                               where category == null || x.Category == category
                               where hasCSharpImplementation == null || x.HasCSharpImplementation == hasCSharpImplementation
                               where hasVBImplementation == null || x.HasVBImplementation == hasVBImplementation
                               where status == null || x.IsEnabledByDefault == status
                               where codeFixStatus == null || x.HasCodeFix == codeFixStatus
                               select x).ToArray();

            if (diagnostics.Length == 0)
            {
                // No entries found
                return this.HttpNotFound();
            }

            viewModel.Diagnostics = diagnostics;

            return this.View(viewModel);
        }
    }
}