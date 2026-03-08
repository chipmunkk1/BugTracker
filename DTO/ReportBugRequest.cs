namespace BugTrackerAPI.DTO
{
    public class ReportBugRequest
    {
        public string file_name { get; set; }
        public string bug_name { get; set; }
        public string bug_description { get; set; }
        public int bug_severity { get; set; }
        public string bug_group_number { get; set; }
    }
}
