import { Repository } from 'typeorm';
import { IEmailService } from '@services';
import { InjectRepository } from '@nestjs/typeorm';
import { EmailEntity } from '@data-access/entities';
import { EmailCreatedEvent, KafkaEvent } from '@dto';

export class KafkaEmailService implements IEmailService {
  constructor(
    @InjectRepository(EmailEntity)
    private emailsRepository: Repository<EmailEntity>,
  ) {}

  async onEmailCreated(
    kafkaEvent: KafkaEvent<EmailCreatedEvent>,
  ): Promise<void> {
    try {
      const createdEvent = kafkaEvent.value as EmailCreatedEvent;
      const createdEntity = { address: createdEvent.email } as EmailEntity;
      await this.emailsRepository.save(createdEntity);
      // TODO: Notify a user about successful email creation
      console.log('Email entity successfully created:', createdEntity);
    } catch (error) {
      this.onInsertError(error, kafkaEvent);
    }
  }

  private onInsertError(error: any, kafkaEvent: any) {
    switch (true) {
      // Email already exists
      case (error?.message || '').indexOf(
        'duplicate key value violates unique constraint',
      ) !== -1:
        // TODO: Notify a user about existing email
        console.log('Email entity already exists:', kafkaEvent.value);
        break;
      // Other errors resend to the queue
      default:
        // TODO: Return the event back to the queue to retry creation
        // console.log('event sent back to the queue:', kafkaEvent);
        console.log(
          `Sending back to the topic '${kafkaEvent.topic}':`,
          kafkaEvent.value,
        );
        console.error({ error });
        break;
    }
  }
}
