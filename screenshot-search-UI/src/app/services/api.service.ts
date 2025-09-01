import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GLOBAL_CONFIG, AppConfig } from '../config/app.config';

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(
    private http: HttpClient,
    @Inject(GLOBAL_CONFIG) private config: AppConfig
  ) {}

  upload(file: File) {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.config.apiUrl}/api/upload`, formData);
  }

  search(query: string) {
    return this.http.get<any[]>(`${this.config.apiUrl}/api/search?query=${encodeURIComponent(query)}`);
  }
}
