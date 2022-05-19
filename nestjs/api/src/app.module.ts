import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { ClientsModule, Transport } from '@nestjs/microservices';

@Module({
  imports: [
    ClientsModule.register([
      {
        name: 'EMAILS_PRODUCER',
        transport: Transport.KAFKA,
        options: {
          client: {
            clientId: 'emails',
            brokers: ['redpanda'],
          },
          consumer: {
            groupId: 'emails-consumer',
          },
        },
      },
    ]),
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
