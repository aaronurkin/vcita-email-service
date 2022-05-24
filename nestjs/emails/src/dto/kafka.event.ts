import { KafkaEventBase } from './kafka-event-base';

export class KafkaEvent<T> extends KafkaEventBase {
  value: T;
}
