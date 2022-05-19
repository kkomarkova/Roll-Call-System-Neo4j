namespace Roll_Call_System_Neo4j.Models
{
    public class User
    {

        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string email { get; set; }

       
        //public User()
        //{
        //}

        //public User(int id, string firstName, string lastName, string password, string email)
        //{
        //    this.id = id;
        //    this.firstName = firstName;
        //    this.lastName = lastName;
        //    this.password = password;
        //    this.email = email;
        //}
    }
}
