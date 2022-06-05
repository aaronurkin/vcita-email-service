using Core31.EventSubscribers.Emails.Data;
using Core31.EventSubscribers.Emails.Data.EntityFramework;
using Core31.Shared.Data.EntityFramework;

namespace Core31.Consumers.CreateEmail.Data.EntityFramework
{
    public class EntityFrameworkEmailsRepository : EntityFrameworkRepository<Email>, IEmailsRepository
    {
        public EntityFrameworkEmailsRepository(EmailsDbContext context)
            : base(context)
        {
        }
    }
}
