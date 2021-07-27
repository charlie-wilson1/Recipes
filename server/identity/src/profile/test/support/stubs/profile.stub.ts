import { Role } from '../../../../models/roles';

export const getProfileStub = () => {
  return {
    email: 'test@test.com',
    username: 'test',
    roles: [Role.Member],
    isActive: true,
  };
};
