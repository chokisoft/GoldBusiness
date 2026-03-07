import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
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
  isOpen = false; // overlay móvil
  private sidebarSubscription?: Subscription;
  private mobileSubscription?: Subscription;

  constructor(
    public translationService: TranslationService,
    private sidebarService: SidebarService
  ) { }

  ngOnInit(): void {
    this.sidebarSubscription = this.sidebarService.collapsed$.subscribe(collapsed => {
      this.isCollapsed = collapsed;
      if (window.innerWidth > 768) {
        this.isOpen = false;
      }
    });

    this.mobileSubscription = this.sidebarService.mobileOpen$.subscribe(open => {
      this.isOpen = open;
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
          },

          // ============================================
          // 👥 TERCEROS
          // ============================================
          {
            title: 'Terceros',
            titleKey: 'sidebar.terceros',
            icon: '👥',
            expanded: false,
            children: [
              {
                title: 'Proveedores',
                titleKey: 'proveedores.title',
                icon: '🏭',
                route: '/nomencladores/proveedores'
              },
              {
                title: 'Clientes',
                titleKey: 'clientes.title',
                icon: '👤',
                route: '/nomencladores/clientes'
              }
            ]
          },

          // ============================================
          // 🏢 ORGANIZACIÓN
          // ============================================
          {
            title: 'Organización',
            titleKey: 'sidebar.organizacion',
            icon: '🏢',
            expanded: false,
            children: [
              {
                title: 'Establecimiento',
                titleKey: 'establecimiento.title',
                icon: '🏛️',
                route: '/nomencladores/establecimiento'
              },
              {
                title: 'Localidad',
                titleKey: 'localidad.title',
                icon: '📍',
                route: '/nomencladores/localidad'
              },
              {
                title: 'Moneda',
                titleKey: 'moneda.title',
                icon: '💱',
                route: '/nomencladores/moneda'
              },
              {
                title: 'País',
                titleKey: 'pais.title',
                icon: '🌍',
                route: '/nomencladores/pais'
              },
              {
                title: 'Provincia',
                titleKey: 'provincia.title',
                icon: '🗺️',
                route: '/nomencladores/provincia'
              },
              {
                title: 'Municipio',
                titleKey: 'municipio.title',
                icon: '🏘️',
                route: '/nomencladores/municipio'
              },
              {
                title: 'Código Postal',
                titleKey: 'codigoPostal.title',
                icon: '📮',
                route: '/nomencladores/codigo-postal'
              }
            ]
          },

          // ============================================
          // 📋 CLASIFICADOR
          // ============================================
          {
            title: 'Clasificador',
            titleKey: 'sidebar.clasificador',
            icon: '📋',
            expanded: false,
            children: [
              {
                title: 'Línea',
                titleKey: 'linea.title',
                icon: '📏',
                route: '/nomencladores/linea'
              },
              {
                title: 'Sublínea',
                titleKey: 'subLinea.title',
                icon: '📐',
                route: '/nomencladores/sublinea'
              },
              {
                title: 'Unidad Medida',
                titleKey: 'unidadMedida.title',
                icon: '⚖️',
                route: '/nomencladores/unidad-medida'
              }
            ]
          },

          // ============================================
          // 🔄 OPERACIONES
          // ============================================
          {
            title: 'Operaciones',
            titleKey: 'sidebar.operaciones',
            icon: '🔄',
            expanded: false,
            children: [
              {
                title: 'Transacción',
                titleKey: 'transaccion.title',
                icon: '💹',
                route: '/nomencladores/transaccion'
              },
              {
                title: 'Concepto Ajuste',
                titleKey: 'conceptoAjuste.title',
                icon: '⚙️',
                route: '/nomencladores/concepto-ajuste'
              }
            ]
          },

          // ============================================
          // 📦 PRODUCTO
          // ============================================
          {
            title: 'Producto',
            titleKey: 'sidebar.producto',
            icon: '📦',
            expanded: false,
            children: [
              {
                title: 'Productos',
                titleKey: 'producto.title',
                icon: '🏷️',
                route: '/nomencladores/producto'
              }
            ]
          }
        ]
      },

      // ============================================
      // ⚙️ CONFIGURACIÓN
      // ============================================
      {
        title: 'Configuración',
        titleKey: 'sidebar.configuracion',
        icon: '⚙️',
        expanded: false,
        children: [
          {
            title: 'Negocio',
            titleKey: 'sidebar.negocio',
            icon: '🏢',
            route: '/configuracion/negocio'  // Actualizado según el routing
          },
          {
            title: 'Usuarios',
            titleKey: 'sidebar.usuarios',
            icon: '👥',
            route: '/configuracion/usuarios'  // Actualizado según el routing
          },
          {
            title: 'Prueba de Conexión',
            titleKey: 'sidebar.testConnection',
            icon: '🔌',
            route: '/configuracion/test-conexion'  // Actualizado según el routing
          }
        ]
      }
    ];
  }

  ngOnDestroy(): void {
    this.sidebarSubscription?.unsubscribe();
    this.mobileSubscription?.unsubscribe();
  }

  getTitle(item: MenuItem): string {
    return item.titleKey ? this.translationService.translate(item.titleKey) : item.title;
  }

  toggleItem(item: MenuItem): void {
    if (item.children) {
      item.expanded = !item.expanded;
    }
  }

  toggleSidebar(): void {
    if (window.innerWidth <= 768) {
      this.sidebarService.toggleMobile();
      return;
    }
    this.sidebarService.toggle();
  }

  closeMobile(): void {
    if (this.isOpen) {
      this.sidebarService.setMobileOpen(false);
    }
  }

  @HostListener('window:resize')
  onResize(): void {
    if (window.innerWidth > 768 && this.isOpen) {
      this.sidebarService.setMobileOpen(false);
    }
  }

  getToggleTitle(): string {
    return this.isCollapsed
      ? this.translationService.translate('sidebar.expandMenu')
      : this.translationService.translate('sidebar.collapseMenu');
  }
}
