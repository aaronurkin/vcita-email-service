import { join } from 'path';
import { TypeOrmModuleOptions } from '@nestjs/typeorm';

export const TYPE_ORM_CONFIG: TypeOrmModuleOptions = {
  type: 'postgres',
  host: process.env.TYPE_ORM_DB_HOST,
  username: process.env.TYPE_ORM_DB_USERNAME,
  password: process.env.TYPE_ORM_DB_PASSWORD,
  database: process.env.TYPE_ORM_DB_NAME,
  entities: [join(__dirname, process.env.TYPE_ORM_DB_ENTITIES_PATH_PATTERN)],
  // Make false. It's true for demo purposes only.
  // Must be false in the production environment!
  synchronize: true,
};
