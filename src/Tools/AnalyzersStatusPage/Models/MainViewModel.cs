namespace RoslynAnalyzersStatus.Web.Models
{
    using System.Collections.Generic;

    public class MainViewModel
    {
        public IEnumerable<AnalyzersStatusInfo> Diagnostics { get; set; }
    }
}