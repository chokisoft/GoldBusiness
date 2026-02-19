import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslationService } from '../../services/translation.service';
import { LanguageService } from '../../services/language.service';
import { DashboardService, DashboardDataDto } from '../../services/dashboard.service';
import { Subscription } from 'rxjs';

type SupportedLanguage = 'es' | 'en' | 'fr';

interface StatCard {
  title: string;
  value: string | number;
  icon: string;
  color: string;
  change?: string;
  changeType?: 'positive' | 'negative' | 'neutral';
}

interface RecentActivity {
  icon: string;
  actionKey: string;
  target: string;
  timeValue: Date;
  user: string;
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
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
  stats: StatCard[] = [];
  recentActivities: RecentActivity[] = [];
  quickLinks: QuickLink[] = [];
  
  isLoading = true;
  errorMessage: string | null = null;
  
  private languageSubscription?: Subscription;

  constructor(
    public translationService: TranslationService,
    private languageService: LanguageService,
    private dashboardService: DashboardService
  ) { }

  ngOnInit(): void {
    console.log('📊 Dashboard inicializado');
    
    // Cargar datos desde la API
    this.loadDashboardData();
    
    // Cargar quick links (estáticos)
    this.loadQuickLinks();

    // Recargar cuando cambia el idioma
    this.languageSubscription = this.translationService.translations$.subscribe(() => {
      console.log('🔄 Dashboard: Idioma cambiado, recargando datos desde API...');
      this.loadDashboardData();
    });
  }

  ngOnDestroy(): void {
    console.log('🧹 Dashboard destruido');
    this.languageSubscription?.unsubscribe();
  }

  /**
   * Cargar datos desde la API - MÉTODO PÚBLICO PARA PODER LLAMARLO DESDE EL TEMPLATE
   */
  loadDashboardData(): void { // ← CAMBIADO DE private A public (sin modificador = public)
    console.log('🌐 Llamando a la API del dashboard...');
    this.isLoading = true;
    this.errorMessage = null;

    this.dashboardService.getDashboardData().subscribe({
      next: (data: DashboardDataDto) => {
        console.log('✅ Datos recibidos de la API:', data);
        this.mapStatsFromDto(data.stats);
        this.mapActivitiesFromDto(data.recentActivities);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('❌ Error al cargar dashboard desde API:', error);
        this.errorMessage = this.translationService.translate('error.loading');
        this.isLoading = false;
        
        // Cargar datos mock como fallback
        this.loadMockData();
      }
    });
  }

  /**
   * Mapear stats del DTO al formato del componente
   */
  private mapStatsFromDto(dto: any): void {
    this.stats = [
      {
        title: this.translationService.translate('dashboard.totalAccounts'),
        value: dto.totalAccounts,
        icon: '📊',
        color: '#4a90e2',
        change: dto.accountsChange,
        changeType: dto.accountsChangeType
      },
      {
        title: this.translationService.translate('dashboard.activeUsers'),
        value: dto.activeUsers,
        icon: '👥',
        color: '#50c878',
        change: dto.usersChange,
        changeType: dto.usersChangeType
      },
      {
        title: this.translationService.translate('dashboard.accountGroups'),
        value: dto.accountGroups,
        icon: '📁',
        color: '#f39c12',
        change: dto.groupsChange,
        changeType: dto.groupsChangeType
      },
      {
        title: this.translationService.translate('dashboard.pendingTasks'),
        value: dto.pendingTasks,
        icon: '⚠️',
        color: '#e74c3c',
        change: dto.tasksChange,
        changeType: dto.tasksChangeType
      }
    ];
  }

  /**
   * Mapear actividades del DTO al formato del componente
   */
  private mapActivitiesFromDto(dtos: any[]): void {
    this.recentActivities = dtos.map(dto => ({
      icon: dto.icon,
      actionKey: this.getActionKey(dto.actionType),
      target: dto.targetName,
      timeValue: new Date(dto.createdAt),
      user: dto.userName
    }));
  }

  /**
   * Convertir actionType del backend a clave de traducción
   */
  private getActionKey(actionType: string): string {
    const actionKeys: { [key: string]: string } = {
      'accountCreated': 'dashboard.activity.accountCreated',
      'accountModified': 'dashboard.activity.accountModified',
      'accountDeleted': 'dashboard.activity.accountDeleted',
      'configUpdated': 'dashboard.activity.configUpdated'
    };
    
    return actionKeys[actionType] || 'dashboard.activity.accountModified';
  }

  /**
   * Cargar datos mock como fallback si falla la API
   */
  private loadMockData(): void {
    console.log('⚠️ Cargando datos mock como fallback');
    
    this.stats = [
      {
        title: this.translationService.translate('dashboard.totalAccounts'),
        value: 0,
        icon: '📊',
        color: '#4a90e2',
        change: '0%',
        changeType: 'neutral'
      },
      {
        title: this.translationService.translate('dashboard.activeUsers'),
        value: 0,
        icon: '👥',
        color: '#50c878',
        change: '0',
        changeType: 'neutral'
      },
      {
        title: this.translationService.translate('dashboard.accountGroups'),
        value: 0,
        icon: '📁',
        color: '#f39c12',
        change: '0%',
        changeType: 'neutral'
      },
      {
        title: this.translationService.translate('dashboard.pendingTasks'),
        value: 0,
        icon: '⚠️',
        color: '#e74c3c',
        change: '0',
        changeType: 'neutral'
      }
    ];

    this.recentActivities = [];
  }

  private loadQuickLinks(): void {
    this.quickLinks = [
      {
        titleKey: 'dashboard.quickLinks.newAccount',
        icon: '➕',
        route: '/nomencladores/cuenta/nuevo',
        color: '#4a90e2'
      },
      {
        titleKey: 'dashboard.quickLinks.viewAccounts',
        icon: '📋',
        route: '/nomencladores/cuenta',
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

  /**
   * Obtener el tiempo formateado traducido desde una fecha
   */
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
}
