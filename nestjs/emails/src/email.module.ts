import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { EmailEntity } from '@data-access/entities';
import { EmailsController } from './emails.controller';
import { DataAccessModule } from '@data-access/data-access.module';
import { KAFKA_EMAIL_SERVICE_PROVIDER } from '@providers/kafka-email-service.provider';

@Module({
  imports: [DataAccessModule, TypeOrmModule.forFeature([EmailEntity])],
  controllers: [EmailsController],
  providers: [KAFKA_EMAIL_SERVICE_PROVIDER],
})
export class EmailModule {}
