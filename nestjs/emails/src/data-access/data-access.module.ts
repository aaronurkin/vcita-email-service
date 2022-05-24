import { Module } from '@nestjs/common';
import { TYPE_ORM_CONFIG } from '@configs';
import { TypeOrmModule } from '@nestjs/typeorm';

@Module({
  imports: [TypeOrmModule.forRoot(TYPE_ORM_CONFIG)],
  exports: [TypeOrmModule],
})
export class DataAccessModule {}
