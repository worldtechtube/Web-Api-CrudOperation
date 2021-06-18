namespace StudentApi.Models
{
    public class StudentClass
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string gender{get;set;}
        public string course{get;set;}
        public bool sendUpdates { get; set; }
        public string bloodGroup {get;set;}
        public AddressClass address { get; set; }
    }
}

