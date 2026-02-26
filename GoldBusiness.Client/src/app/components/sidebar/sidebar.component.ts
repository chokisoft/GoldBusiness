import { Component, OnInit, OnDestroy } from '@angular/core';
import { TranslationService } from '../../services/translation.service';
import { SidebarService } from '../../services/sidebar.service';
import { Subscription } from 'rxjs';

interface MenuItem {
  title: string;
  titleKey?: string;
  icon: string;
  route?: string;
  children?: MenuItem[];
  expanded?: boolean;
}

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit, OnDestroy {
  menuItems: MenuItem[] = [];
  isCollapsed = false;
  private sidebarSubscription?: Subscription;

  constructor(
    public translationService: TranslationService,
    private sidebarService: SidebarService
  ) { }

  ngOnInit(): void {
    console.log('📂 Sidebar inicializado');

    // Suscribirse al estado del sidebar
    this.sidebarSubscription = this.sidebarService.collapsed$.subscribe(collapsed => {
      console.log('📂 Sidebar estado cambiado:', collapsed);
      this.isCollapsed = collapsed;
    });

    this.menuItems = [
      {
        title: 'Nomencladores',
        titleKey: 'sidebar.nomencladores',
        icon: '🗂️',
        expanded: false,
        children: [
          {
            title: 'Plan de Cuentas',
            titleKey: 'sidebar.planCuentas',
            icon: '📊',
            expanded: false,
            children: [
              { title: 'Grupos de Cuenta', titleKey: 'grupoCuenta.title', icon: '📁', route: '/nomencladores/grupo-cuenta' },
              { title: 'SubGrupos de Cuenta', titleKey: 'subGrupoCuenta.title', icon: '📂', route: '/nomencladores/subgrupo-cuenta' },
              { title: 'Cuentas', titleKey: 'cuenta.title', icon: '📄', route: '/nomencladores/cuenta' }
            ]
          }
        ]
      },
      {
        title: 'Configuración',
        titleKey: 'sidebar.configuracion',
        icon: '⚙️',
        expanded: false,
        children: [
          { title: 'Negocio', titleKey: 'sidebar.negocio', icon: '🏢', route: '/configuracion' },
          { title: 'Usuarios', titleKey: 'sidebar.usuarios', icon: '👤', route: '/usuarios' },
          { title: 'Prueba de Conexión', titleKey: 'sidebar.testConnection', icon: '🔌', route: '/test-conexion' }
        ]
      }
    ];
  }

  ngOnDestroy(): void {
    console.log('🧹 Sidebar destruido');
    this.sidebarSubscription?.unsubscribe();
  }

  getTitle(item: MenuItem): string {
    return item.titleKey
      ? this.translationService.translate(item.titleKey)
      : item.title;
  }

  toggleItem(item: MenuItem): void {
    if (item.children) {
      item.expanded = !item.expanded;
    }
  }

  toggleSidebar(): void {
    console.log('🔄 Toggle sidebar');
    this.sidebarService.toggle();
  }

  getToggleTitle(): string {
    return this.isCollapsed 
      ? this.translationService.translate('sidebar.expandMenu')
      : this.translationService.translate('sidebar.collapseMenu');
  }
}
