export class EmailCreatedEvent {
  constructor(public readonly email: string) {}

  toString() {
    return JSON.stringify(this);
  }
}
