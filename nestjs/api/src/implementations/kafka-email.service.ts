import { Inject } from '@nestjs/common';
import { IEmailService } from '@services';
import { ClientKafka } from '@nestjs/microservices';
import { CreateEmailRequest, EmailCreatedEvent } from '@dto';

export class KafkaEmailService implements IEmailService {
  constructor(
    @Inject(process.env.KAFKA_CLIENT_EMAILS)
    private readonly emailsClientKafka: ClientKafka,
  ) {}

  createEmail(request: CreateEmailRequest): void {
    this.emailsClientKafka.emit(
      process.env.EVENT_CREATE_EMAIL_REQUESTED,
      new EmailCreatedEvent(request.email),
    );
  }
}
