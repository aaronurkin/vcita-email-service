import { Controller, Inject } from '@nestjs/common';
import { EmailCreatedEvent, KafkaEvent } from '@dto';
import { EventPattern } from '@nestjs/microservices';
import { EMAIL_SERVICE, IEmailService } from '@services';

@Controller()
export class EmailsController {
  constructor(
    @Inject(EMAIL_SERVICE) private readonly emailService: IEmailService,
  ) {}

  @EventPattern(process.env.EVENT_CREATE_EMAIL_REQUESTED)
  public onEmailCreated(event: KafkaEvent<EmailCreatedEvent>): void {
    this.emailService.onEmailCreated(event);
  }
}
