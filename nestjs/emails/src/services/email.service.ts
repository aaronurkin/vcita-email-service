import { EmailCreatedEvent, KafkaEvent } from '@dto';

export interface IEmailService {
  onEmailCreated(data: KafkaEvent<EmailCreatedEvent>): void;
}
