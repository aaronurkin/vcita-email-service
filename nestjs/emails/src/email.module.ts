import { Module } from '@nestjs/common';
import { EmailsController } from './emails.controller';
import { KAFKA_EMAIL_SERVICE_PROVIDER } from '@providers/kafka-email-service.provider';

@Module({
  controllers: [EmailsController],
  providers: [KAFKA_EMAIL_SERVICE_PROVIDER],
})
export class EmailModule {}
