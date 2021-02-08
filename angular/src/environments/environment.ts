export const environment = {
  SERVER_URL: `https://localhost:44305`,
  AUTH_SERVER: `https://localhost:44305`,
  AURORA_WEB: {
    CLIENT_ID: 'a600f787-f286-4a03-8fb9-7398f00b4f78',
    CLIENT_SECRET: 'b274d57b-9a50-44cc-bf45-ee50ff24670e',
    SCOPE: 'openid profile aurora-api'
  },
  AURORA_HOST_WEB: {
    CLIENT_ID: 'cdc9a0e1-8d4d-461b-aa3e-52165eb4ceb4',
    CLIENT_SECRET: 'd26b27ad-5972-47a4-99d4-ac29da439ee9',
    SCOPE: 'openid profile aurora-host-api'
  },
  production: false,
  useHash: true
};
