namespace MusicInstructor.Web.Domain
{
    public class UserProfile : Entity
    {
        public bool IsNull { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static UserProfile NullProfile
        {
            get { return new UserProfile{ IsNull = true}; }
        }
    }
}