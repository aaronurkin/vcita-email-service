import { KafkaEventbatchContext } from './kafka-event-batch-context';

export class KafkaEventBase {
  magicByte: number;
  attributes: number;
  timestamp: string;
  offset: string;
  key: string;
  headers: { [name: string]: any } = {};
  isControlRecord: boolean;
  batchContext: KafkaEventbatchContext;
  topic: string;
  partition: number;
}
