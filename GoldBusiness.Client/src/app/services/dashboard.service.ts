import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

// ═══════════════════════════════════════════════════════════════
// 📊 INTERFACES QUE COINCIDEN CON LOS DTOs DEL BACKEND
// ═══════════════════════════════════════════════════════════════

export interface DashboardStatsDto {
  // Contabilidad
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
  
  // Inventario
  totalProducts: number;
  productsChange: string;
  productsChangeType: string;
  lowStockProducts: number;
  totalInventoryValue: number;
  
  // Clientes y Proveedores
  totalClients: number;
  clientsChange: string;
  clientsChangeType: string;
  newClientsLastMonth: number;
  totalSuppliers: number;
  suppliersChange: string;
  suppliersChangeType: string;
  
  // Finanzas
  totalReceivable: number;
  totalPayable: number;
  overdueReceivable: number;
  overduePayable: number;
  
  // Operaciones
  salesToday: number;
  salesThisMonth: number;
  salesMonthChange: string;
  salesMonthChangeType: string;
  transactionsToday: number;
  averageTicket: number;
}

export interface RecentActivityDto {
  id: number;
  icon: string;
  actionType: string;
  targetName: string;
  createdAt: Date;
  userName: string;
  entityType: string;
  amount?: number;
}

export interface SalesChartDto {
  labels: string[];
  sales: number[];
  purchases: number[];
}

export interface TopProductsChartDto {
  productNames: string[];
  quantities: number[];
  totalSales: number[];
}

export interface InventoryStatusChartDto {
  inStock: number;
  lowStock: number;
  outOfStock: number;
}

export interface ReceivablePayableChartDto {
  receivableCurrent: number;
  receivableOverdue1_30: number;
  receivableOverdue30Plus: number;
  payableCurrent: number;
  payableOverdue1_30: number;
  payableOverdue30Plus: number;
}

export interface ClientTrendChartDto {
  labels: string[];
  newClients: number[];
}

export interface DashboardChartDataDto {
  salesChart: SalesChartDto;
  topProducts: TopProductsChartDto;
  inventoryStatus: InventoryStatusChartDto;
  receivablePayable: ReceivablePayableChartDto;
  clientTrend: ClientTrendChartDto;
}

export interface DashboardAlertDto {
  type: string;
  icon: string;
  messageKey: string;
  message: string;
  count: number;
  actionRoute: string;
}

export interface DashboardAlertsDto {
  alerts: DashboardAlertDto[];
  totalAlerts: number;
  criticalAlerts: number;
}

export interface DashboardDataDto {
  stats: DashboardStatsDto;
  recentActivities: RecentActivityDto[];
  charts: DashboardChartDataDto;
  alerts: DashboardAlertsDto;
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private readonly baseUrl = 'dashboard';

  constructor(private apiService: ApiService) { }

  /**
   * Obtener todos los datos del dashboard (stats, actividades, gráficas, alertas)
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

  /**
   * Obtener datos de gráficas
   */
  getCharts(): Observable<DashboardChartDataDto> {
    console.log('📊 DashboardService: Llamando a API /api/dashboard/charts');
    return this.apiService.get<DashboardChartDataDto>(`${this.baseUrl}/charts`);
  }

  /**
   * Obtener alertas y notificaciones
   */
  getAlerts(): Observable<DashboardAlertsDto> {
    console.log('📊 DashboardService: Llamando a API /api/dashboard/alerts');
    return this.apiService.get<DashboardAlertsDto>(`${this.baseUrl}/alerts`);
  }
}

