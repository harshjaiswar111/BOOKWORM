namespace BookWormNET.dto
{
    public class AdminRegisterDto
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string AdminSecretKey { get; set; }
    }

}
