import { Profile } from '../../models/profile.schema';
import { MockModel } from '../../../../test/support/mock.model';
import { getProfileStub } from '../../../../test/support/stubs/profile.stub';

export class ProfileModelMock extends MockModel<Profile> {
  protected entityStub = getProfileStub();
}
