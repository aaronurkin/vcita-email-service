import { Provider } from '@nestjs/common';
import { EMAIL_SERVICE, IEmailService } from '@services';
import { KafkaEmailService } from '@implementations/kafka-email.service';

export const KAFKA_EMAIL_SERVICE_PROVIDER: Provider<IEmailService> = {
  provide: EMAIL_SERVICE,
  useClass: KafkaEmailService,
};
