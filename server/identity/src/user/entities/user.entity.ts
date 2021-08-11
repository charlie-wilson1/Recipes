import { Column, Entity, ObjectIdColumn, PrimaryColumn } from 'typeorm';

@Entity()
export class User {
  @ObjectIdColumn()
  _id: string;

  @Column()
  roles: string[];

  @Column()
  username: string;

  @Column()
  email: string;

  @Column()
  isActive: boolean;
}
