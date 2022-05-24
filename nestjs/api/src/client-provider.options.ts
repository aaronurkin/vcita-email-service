import { ClientProviderOptions, Transport } from '@nestjs/microservices';

export const CLIENT_PROVIDER_OPTIONS_KAFKA_EMAIL: ClientProviderOptions = {
  name: process.env.KAFKA_CLIENT_EMAILS,
  transport: Transport.KAFKA,
  options: {
    client: {
      clientId: process.env.KAFKA_CLIENT_ID_EMAILS,
      brokers: process.env.KAFKA_BROKERS.split(','),
    },
    consumer: {
      groupId: process.env.KAFKA_CONSUMERS_GROUP_EMAILS,
      allowAutoTopicCreation: true,
    },
  },
};
