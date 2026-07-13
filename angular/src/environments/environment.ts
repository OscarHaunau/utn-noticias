import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'UtnNoticias',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44311/',
    redirectUri: baseUrl,
    clientId: 'UtnNoticias_App',
    responseType: 'code',
    scope: 'offline_access UtnNoticias',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44311',
      rootNamespace: 'UtnNoticias',
    },
  },
} as Environment;
