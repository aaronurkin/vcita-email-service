using Core31.EventSubscribers.Emails.Data;
using Core31.Shared.Data;

namespace Core31.Consumers.CreateEmail.Data
{
    public interface IEmailsRepository : IRepository<Email>
    {
    }
}
