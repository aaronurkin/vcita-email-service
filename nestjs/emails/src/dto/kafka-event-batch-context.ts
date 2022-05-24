export class KafkaEventbatchContext {
  firstOffset: string;
  firstTimestamp: string;
  partitionLeaderEpoch: number;
  inTransaction: boolean;
  isControlBatch: boolean;
  lastOffsetDelta: number;
  producerId: string;
  producerEpoch: number;
  firstSequence: number;
  maxTimestamp: string;
  timestampType: number;
  magicByte: number;
}
