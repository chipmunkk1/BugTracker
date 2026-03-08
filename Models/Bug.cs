namespace BugTrackerAPI.Models
{
    public class Bug
    {
        public int id { get; set; }   
        public string file_name { get; set; }
        public string bug_name { get; set; }
        public string bug_description{ get; set; }
        public int bug_severity{ get; set; }
        public bool is_deleted{ get; set; }
        public bool is_solved{ get; set; }
        public string bug_group_number {  get; set; }

        public DateTime creation_date {get; set; }
    }
}
