import { getProfileStub } from '../../../../test/support/stubs/profile.stub';

export const ProfileServiceMock = jest.fn().mockReturnValue({
  create: jest.fn().mockResolvedValue(getProfileStub()),
  getAll: jest.fn().mockResolvedValue(getProfileStub()),
  findByEmail: jest.fn().mockResolvedValue(getProfileStub()),
  findByUsername: jest.fn().mockResolvedValue(getProfileStub()),
  delete: jest.fn(),
  updateRoles: jest.fn().mockResolvedValue(getProfileStub()),
  updateProfileUsername: jest.fn().mockResolvedValue(getProfileStub()),
});
