import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { TranslationService } from '../../services/translation.service';
import { LanguageService } from '../../services/language.service';
import { 
  DashboardService, 
  DashboardDataDto, 
  DashboardAlertDto 
} from '../../services/dashboard.service';
import { Subscription } from 'rxjs';

type SupportedLanguage = 'es' | 'en' | 'fr';

interface StatCard {
  title: string;
  value: string | number;
  icon: string;
  color: string;
  change?: string;
  changeType?: 'positive' | 'negative' | 'neutral';
  suffix?: string; // Para agregar sufijos como "$", "%" 
}

interface RecentActivity {
  icon: string;
  actionKey: string;
  target: string;
  timeValue: Date;
  user: string;
  amount?: number;
}

interface QuickLink {
  titleKey: string;
  icon: string;
  route: string;
  color: string;
}

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class DashboardComponent implements OnInit, OnDestroy {
  stats: StatCard[] = [];
  recentActivities: RecentActivity[] = [];
  quickLinks: QuickLink[] = [];
  alerts: DashboardAlertDto[] = [];
  
  isLoading = true;
  errorMessage: string | null = null;
  
  private languageSubscription?: Subscription;

  constructor(
    public translationService: TranslationService,
    private languageService: LanguageService,
    private dashboardService: DashboardService
  ) { }

  ngOnInit(): void {
    console.log('📊 Dashboard enriquecido inicializado');
    
    this.loadDashboardData();
    this.loadQuickLinks();

    this.languageSubscription = this.translationService.translations$.subscribe(() => {
      console.log('🔄 Dashboard: Idioma cambiado, recargando datos...');
      this.loadDashboardData();
    });
  }

  ngOnDestroy(): void {
    console.log('🧹 Dashboard destruido');
    this.languageSubscription?.unsubscribe();
  }

  loadDashboardData(): void {
    console.log('🌐 Llamando a la API del dashboard completo...');
    this.isLoading = true;
    this.errorMessage = null;

    this.dashboardService.getDashboardData().subscribe({
      next: (data: DashboardDataDto) => {
        console.log('✅ Datos completos recibidos:', data);
        this.mapStatsFromDto(data.stats);
        this.mapActivitiesFromDto(data.recentActivities);
        this.alerts = data.alerts?.alerts || [];
        this.isLoading = false;
      },
      error: (error) => {
        console.error('❌ Error al cargar dashboard:', error);
        console.error('❌ Error status:', error.status);
        console.error('❌ Error message:', error.message);
        console.error('❌ Error details:', error.error);
        
        // Mensaje de error más descriptivo
        if (error.status === 0) {
          this.errorMessage = 'No se puede conectar con el servidor. Verifica que el backend esté ejecutándose.';
        } else if (error.status === 401) {
          this.errorMessage = 'No autorizado. Por favor, inicia sesión nuevamente.';
        } else if (error.status === 500) {
          this.errorMessage = `Error del servidor: ${error.error?.message || 'Error interno'}`;
        } else {
          this.errorMessage = this.translationService.translate('error.loading') + ': ' + (error.error?.message || error.message);
        }
        
        this.isLoading = false;
        this.loadMockData();
      }
    });
  }

  private mapStatsFromDto(dto: any): void {
    const safeDto = dto || {};

    this.stats = [
      // FILA 1: Operaciones y Ventas
      {
        title: this.translationService.translate('dashboard.salesThisMonth'),
        value: this.formatCurrency(safeDto.salesThisMonth || 0),
        icon: '💰',
        color: '#2ecc71',
        change: safeDto.salesMonthChange,
        changeType: safeDto.salesMonthChangeType
      },
      {
        title: this.translationService.translate('dashboard.totalProducts'),
        value: safeDto.totalProducts || 0,
        icon: '📦',
        color: '#3498db',
        change: safeDto.productsChange,
        changeType: safeDto.productsChangeType
      },
      {
        title: this.translationService.translate('dashboard.totalClients'),
        value: safeDto.totalClients || 0,
        icon: '👥',
        color: '#9b59b6',
        change: safeDto.clientsChange,
        changeType: safeDto.clientsChangeType
      },
      {
        title: this.translationService.translate('dashboard.totalSuppliers'),
        value: safeDto.totalSuppliers || 0,
        icon: '🏭',
        color: '#e67e22',
        change: safeDto.suppliersChange,
        changeType: safeDto.suppliersChangeType
      },
      
      // FILA 2: Inventario y Finanzas
      {
        title: this.translationService.translate('dashboard.lowStockProducts'),
        value: safeDto.lowStockProducts || 0,
        icon: '⚠️',
        color: '#f39c12',
        change: safeDto.lowStockProducts > 0 ? 'Atención' : 'OK',
        changeType: safeDto.lowStockProducts > 0 ? 'negative' : 'positive'
      },
      {
        title: this.translationService.translate('dashboard.totalReceivable'),
        value: this.formatCurrency(safeDto.totalReceivable || 0),
        icon: '💸',
        color: '#16a085',
        change: safeDto.overdueReceivable > 0 ? `${this.formatCurrency(safeDto.overdueReceivable)} vencido` : 'Al día',
        changeType: safeDto.overdueReceivable > 0 ? 'negative' : 'positive'
      },
      {
        title: this.translationService.translate('dashboard.totalPayable'),
        value: this.formatCurrency(safeDto.totalPayable || 0),
        icon: '💳',
        color: '#c0392b',
        change: safeDto.overduePayable > 0 ? `${this.formatCurrency(safeDto.overduePayable)} vencido` : 'Al día',
        changeType: safeDto.overduePayable > 0 ? 'negative' : 'positive'
      },
      {
        title: this.translationService.translate('dashboard.totalAccounts'),
        value: safeDto.totalAccounts || 0,
        icon: '📊',
        color: '#34495e',
        change: safeDto.accountsChange,
        changeType: safeDto.accountsChangeType
      }
    ];
  }

  private mapActivitiesFromDto(dtos: any[]): void {
    this.recentActivities = (dtos || []).map(dto => ({
      icon: dto.icon,
      actionKey: this.getActionKey(dto.actionType),
      target: dto.targetName,
      timeValue: new Date(dto.createdAt),
      user: dto.userName,
      amount: dto.amount
    }));
  }

  private getActionKey(actionType: string): string {
    const actionKeys: { [key: string]: string } = {
      'accountCreated': 'dashboard.activity.accountCreated',
      'accountModified': 'dashboard.activity.accountModified',
      'saleCompleted': 'dashboard.activity.saleCompleted',
      'productCreated': 'dashboard.activity.productCreated',
      'clientCreated': 'dashboard.activity.clientCreated',
      'supplierCreated': 'dashboard.activity.supplierCreated',
      'configUpdated': 'dashboard.activity.configUpdated'
    };
    
    return actionKeys[actionType] || 'dashboard.activity.accountModified';
  }

  private loadMockData(): void {
    console.log('⚠️ Cargando datos mock como fallback');
    
    this.stats = [
      {
        title: this.translationService.translate('dashboard.salesThisMonth'),
        value: '$0.00',
        icon: '💰',
        color: '#2ecc71',
        change: '0%',
        changeType: 'neutral'
      },
      {
        title: this.translationService.translate('dashboard.totalProducts'),
        value: 0,
        icon: '📦',
        color: '#3498db',
        change: '0%',
        changeType: 'neutral'
      },
      {
        title: this.translationService.translate('dashboard.totalClients'),
        value: 0,
        icon: '👥',
        color: '#9b59b6',
        change: '0%',
        changeType: 'neutral'
      },
      {
        title: this.translationService.translate('dashboard.lowStockProducts'),
        value: 0,
        icon: '⚠️',
        color: '#f39c12',
        change: 'OK',
        changeType: 'positive'
      }
    ];

    this.recentActivities = [];
    this.alerts = [];
  }

  private loadQuickLinks(): void {
    this.quickLinks = [
      {
        titleKey: 'dashboard.quickLinks.newAccount',
        icon: '➕',
        route: '/plan-cuentas/cuenta/nuevo',
        color: '#4a90e2'
      },
      {
        titleKey: 'dashboard.quickLinks.viewAccounts',
        icon: '📋',
        route: '/plan-cuentas/cuenta',
        color: '#50c878'
      },
      {
        titleKey: 'dashboard.quickLinks.configuration',
        icon: '⚙️',
        route: '/configuracion',
        color: '#f39c12'
      },
      {
        titleKey: 'dashboard.quickLinks.reports',
        icon: '📈',
        route: '/reportes',
        color: '#9b59b6'
      }
    ];
  }

  getTranslation(key: string, params: any[] = []): string {
    return this.translationService.translate(key, params);
  }

  getTimeAgo(date: Date): string {
    const now = new Date();
    const diffMs = now.getTime() - new Date(date).getTime();
    const diffHours = Math.floor(diffMs / (1000 * 60 * 60));
    
    if (diffHours < 1) {
      const diffMinutes = Math.floor(diffMs / (1000 * 60));
      return this.translationService.translate('dashboard.time.minutes', [diffMinutes.toString()]);
    } else if (diffHours === 1) {
      return this.translationService.translate('dashboard.time.oneHour');
    } else if (diffHours < 24) {
      return this.translationService.translate('dashboard.time.hours', [diffHours.toString()]);
    } else {
      const days = Math.floor(diffHours / 24);
      if (days === 1) {
        return this.translationService.translate('dashboard.time.oneDay');
      } else {
        return this.translationService.translate('dashboard.time.days', [days.toString()]);
      }
    }
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('es-ES', {
      style: 'currency',
      currency: 'EUR',
      minimumFractionDigits: 2
    }).format(value);
  }

  getAlertClass(type: string): string {
    const classes: { [key: string]: string } = {
      'warning': 'alert-warning',
      'error': 'alert-error',
      'info': 'alert-info',
      'success': 'alert-success'
    };
    return classes[type] || 'alert-info';
  }
}

