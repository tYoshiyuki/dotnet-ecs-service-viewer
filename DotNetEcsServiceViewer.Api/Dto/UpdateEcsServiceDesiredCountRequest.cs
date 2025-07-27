namespace DotNetEcsServiceViewer.Api.Dto
{
    public class UpdateEcsServiceDesiredCountRequest
    {
        public string ClusterName { get; set; }
        public string ServiceName { get; set; }
        public int DesiredCount { get; set; }
    }
}
