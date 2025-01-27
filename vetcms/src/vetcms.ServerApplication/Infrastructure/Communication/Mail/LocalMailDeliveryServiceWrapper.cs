using vetcms.ServerApplication.Common.Abstractions;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Infrastructure.Communication.Mail
{
    internal class LocalMailDeliveryServiceWrapper(IRepositoryBase<SentEmail> repository) : IMailDeliveryProviderWrapper
    {
        public async Task<int> SendEmailAsync(string toEmail, string subject, string body)
        {
            SentEmail newEmail = new SentEmail();
            newEmail.From = "no-reply@vetcms.hu";
            newEmail.To = toEmail;
            newEmail.Subject = subject;
            newEmail.Timestamp = DateTime.Now;
            newEmail.HtmlContent = body;

           await repository.AddAsync(newEmail);

            return newEmail.Id;
        }

    }
}