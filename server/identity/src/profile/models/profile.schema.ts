import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { Document } from 'mongoose';
import { Role } from '../../models/roles';

export type ProfileDocument = Profile & Document;

@Schema()
export class Profile {
  @Prop({ required: false, minlength: 4 })
  username: string;

  @Prop({ required: true })
  email: string;

  @Prop({ required: true, minlength: 1, default: [Role.Member] })
  roles: Role[];

  @Prop({ required: true, default: true })
  isActive: boolean;
}

export const ProfileSchema = SchemaFactory.createForClass(Profile);
