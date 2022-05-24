import { Module } from '@nestjs/common';
import { ClientsModule } from '@nestjs/microservices';
import { EmailsController } from './emails.controller';
import { CLIENT_PROVIDER_OPTIONS_KAFKA_EMAIL } from './client-provider.options';
import { KAFKA_EMAIL_SERVICE_PROVIDER } from '@providers/kafka-email-service.provider';

@Module({
  imports: [ClientsModule.register([CLIENT_PROVIDER_OPTIONS_KAFKA_EMAIL])],
  controllers: [EmailsController],
  providers: [KAFKA_EMAIL_SERVICE_PROVIDER],
})
export class EmailsModule {}
