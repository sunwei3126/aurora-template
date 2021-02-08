import { MockRequest } from '@delon/mock';

export const DEMOS = {
  'POST /api/mock/demo': (req: MockRequest) => {
    return {
      code: 200
    };
  }
};
