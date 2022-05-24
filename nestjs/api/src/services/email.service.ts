import { CreateEmailRequest } from '@dto';

export interface IEmailService {
  createEmail(request: CreateEmailRequest): void;
}
