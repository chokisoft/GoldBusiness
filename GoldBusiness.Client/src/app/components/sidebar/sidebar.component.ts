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
  isOpen = false; // controla apertura en móvil
  private sidebarSubscription?: Subscription;

  constructor(
    public translationService: TranslationService,
    private sidebarService: SidebarService
  ) { }

  ngOnInit(): void {
    console.log('📂 Sidebar inicializado');

    // Suscribirse al estado del sidebar (desktop)
    this.sidebarSubscription = this.sidebarService.collapsed$.subscribe(collapsed => {
      console.log('📂 Sidebar estado cambiado:', collapsed);
      this.isCollapsed = collapsed;
      // Si estamos en escritorio y se colapsa/expande, aseguramos isOpen cerrado
      if (window.innerWidth > 768) {
        this.isOpen = false;
      }
    });

    this.menuItems = [
      {
        title: 'Nomencladores',
        titleKey: 'sidebar.nomencladores',
        icon: '🗂️',
        expanded: false,
        children: [
          // ============================================
          // 📊 PLAN DE CUENTAS
          // ============================================
          {
            title: 'Plan de Cuentas',
            titleKey: 'sidebar.planCuentas',
            icon: '📊',
            expanded: false,
            children: [
              {
                title: 'Grupos de Cuenta',
                titleKey: 'grupoCuenta.title',
                icon: '📁',
                route: '/nomencladores/grupo-cuenta'
              },
              {
                title: 'SubGrupos de Cuenta',
                titleKey: 'subGrupoCuenta.title',
                icon: '📂',
                route: '/nomencladores/subgrupo-cuenta'
              },
              {
                title: 'Cuentas',
                titleKey: 'cuenta.title',
                icon: '📄',
                route: '/nomencladores/cuenta'
              }
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
    // En móvil, la acción debe abrir/cerrar con la clase .open (no usar collapsed)
    if (window.innerWidth <= 768) {
      this.isOpen = !this.isOpen;
      return;
    }

    // En escritorio/tablet grande usar el servicio que controla collapsed
    this.sidebarService.toggle();
    // Aseguramos que cualquier estado "open" de móvil quede cerrado
    this.isOpen = false;
  }

  // Cerrar sidebar móvil al navegar a una ruta
  onNavigate(): void {
    if (window.innerWidth <= 768) {
      this.isOpen = false;
    }
  }

  getToggleTitle(): string {
    return this.isCollapsed
      ? this.translationService.translate('sidebar.expandMenu')
      : this.translationService.translate('sidebar.collapseMenu');
  }

  // Si el usuario redimensiona a escritorio, cerramos el overlay móvil
  @HostListener('window:resize')
  onResize(): void {
    if (window.innerWidth > 768 && this.isOpen) {
      this.isOpen = false;
    }
  }
}
