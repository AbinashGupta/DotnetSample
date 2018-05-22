namespace StudentApi.Model
{
    public class StudentParam
    {
        public string StudentId { get; set; }                          // external Id, easier to reference: 1,2,3 etc. 
        public string FirstName { get; set; } = string.Empty;    
        public string LastName { get; set; } = string.Empty;     
        public int Age { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
    }
}