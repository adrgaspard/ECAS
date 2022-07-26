import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'ECAS',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44389',
    redirectUri: baseUrl,
    clientId: 'ECAS_App',
    responseType: 'code',
    scope: 'offline_access ECAS',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44367',
      rootNamespace: 'ECAS',
    },
  },
} as Environment;
