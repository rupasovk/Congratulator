namespace CongratulatorWebAppAPI.BuisnesObjects.DtoObjects
{
    [Serializable]
    public class UserDto
    {
        public string UserName { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        // public string UserImage { get; set; }
        //public IFormFile UserImage { get; set; }
    }
}
