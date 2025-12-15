namespace CleanArchMonolit.Application.HttpContext
{
    public interface IHttpContextService
    {
        public int UserId { get; }
        public int ProfileId { get; }
        public string ProfileName { get; }
        public string UserName { get; }
        public bool IsAdmin { get; }
    }
}
