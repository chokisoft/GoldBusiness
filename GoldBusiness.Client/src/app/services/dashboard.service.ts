import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

// Interfaces que coinciden con los DTOs del backend
export interface DashboardStatsDto {
  totalAccounts: number;
  accountsChange: string;
  accountsChangeType: string;
  activeUsers: number;
  usersChange: string;
  usersChangeType: string;
  accountGroups: number;
  groupsChange: string;
  groupsChangeType: string;
  pendingTasks: number;
  tasksChange: string;
  tasksChangeType: string;
}

export interface RecentActivityDto {
  id: number;
  icon: string;
  actionType: string;
  targetName: string;
  createdAt: Date;
  userName: string;
}

export interface DashboardDataDto {
  stats: DashboardStatsDto;
  recentActivities: RecentActivityDto[];
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  // ✅ SIN /api/ porque ApiService ya lo agrega
  private readonly baseUrl = 'dashboard';

  constructor(private apiService: ApiService) { }

  /**
   * Obtener todos los datos del dashboard
   */
  getDashboardData(): Observable<DashboardDataDto> {
    console.log('📊 DashboardService: Llamando a API /api/dashboard');
    return this.apiService.get<DashboardDataDto>(this.baseUrl);
  }

  /**
   * Obtener solo las estadísticas
   */
  getStats(): Observable<DashboardStatsDto> {
    console.log('📊 DashboardService: Llamando a API /api/dashboard/stats');
    return this.apiService.get<DashboardStatsDto>(`${this.baseUrl}/stats`);
  }

  /**
   * Obtener solo las actividades recientes
   */
  getRecentActivities(): Observable<RecentActivityDto[]> {
    console.log('📊 DashboardService: Llamando a API /api/dashboard/recent-activities');
    return this.apiService.get<RecentActivityDto[]>(`${this.baseUrl}/recent-activities`);
  }
}
