import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { env } from '../../../environments/environment.production';
import { Observable } from 'rxjs';
import { DashboardData } from '../../models/dashboard.types';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private apiUrl = env.apiBaseUrl;
  private http = inject(HttpClient);

  getSummary(userId?: number | null): Observable<DashboardData> {
    let params = new HttpParams();
    if (userId != null) {
      params = params.set('userId', userId);
    }
    return this.http.get<DashboardData>(`${this.apiUrl}/api/Dashboard/Summary`, { params });
  }
}
