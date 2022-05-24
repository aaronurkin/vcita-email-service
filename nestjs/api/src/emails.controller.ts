import { CreateEmailRequest } from './dto';
import { EMAIL_SERVICE, IEmailService } from '@services';
import { Body, Controller, Inject, Post } from '@nestjs/common';

@Controller('emails')
export class EmailsController {
  constructor(
    @Inject(EMAIL_SERVICE) private readonly emailService: IEmailService,
  ) {}

  @Post()
  createEmail(@Body() request: CreateEmailRequest): void {
    this.emailService.createEmail(request);
  }
}
