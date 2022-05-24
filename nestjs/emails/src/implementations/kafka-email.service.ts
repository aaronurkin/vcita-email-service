import { IEmailService } from '@services';
import { EmailCreatedEvent, KafkaEvent } from '@dto';

export class KafkaEmailService implements IEmailService {
  onEmailCreated(data: KafkaEvent<EmailCreatedEvent>): void {
    // TODO: Save email into the database and remove the log line.
    console.log('KafkaEmailService.onEmailCreated:', data);
  }
}
