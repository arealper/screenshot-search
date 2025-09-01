import { InjectionToken } from '@angular/core';

export const GLOBAL_CONFIG = new InjectionToken<AppConfig>('app-config');

export interface AppConfig {
  apiUrl: string;
}

export const APP_CONFIG: AppConfig = {
  apiUrl: 'http://localhost:5174'
};
