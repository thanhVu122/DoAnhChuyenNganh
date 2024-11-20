namespace Vfurniture.Areas.Admin.Reponsitory
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email,string subject,string massage);
    }
}
