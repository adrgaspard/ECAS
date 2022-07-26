import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl: 'http://localhost:4200/',
    name: 'ECAS',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:7000',
    redirectUri: baseUrl,
    clientId: 'ECAS_App',
    responseType: 'code',
    scope: 'offline_access IdentityService AdministrationService role email openid profile',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:7500',
      rootNamespace: 'ECAS',
    }
  },
} as Environment;

