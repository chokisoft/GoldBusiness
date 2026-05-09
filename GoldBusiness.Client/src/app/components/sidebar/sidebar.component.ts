import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { Router } from '@angular/router';
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
    private sidebarService: SidebarService,
    private router: Router
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

    // Reordenamiento sugerido: inicio, nomencladores, compras, ventas, inventario, contabilidad, activos fijos, consultas, nómina, configuración
    this.menuItems = [
      { title: 'Inicio', titleKey: 'header.inicio', icon: '🏠', route: '/inicio' },

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
              { title: 'Grupos de Cuenta', titleKey: 'grupoCuenta.title', icon: '📁', route: '/plan-cuentas/grupo-cuenta' },
              { title: 'SubGrupos de Cuenta', titleKey: 'subGrupoCuenta.title', icon: '📂', route: '/plan-cuentas/subgrupo-cuenta' },
              { title: 'Cuentas', titleKey: 'cuenta.title', icon: '📄', route: '/plan-cuentas/cuenta' }
            ]
          },
          {
            title: 'Terceros',
            titleKey: 'sidebar.terceros',
            icon: '👥',
            expanded: false,
            children: [
              { title: 'Proveedores', titleKey: 'proveedores.title', icon: '🏭', route: '/nomencladores/proveedor' },
              { title: 'Clientes', titleKey: 'clientes.title', icon: '👤', route: '/nomencladores/clientes' }
            ]
          },
          {
            title: 'Organización',
            titleKey: 'sidebar.organizacion',
            icon: '🏢',
            expanded: false,
            children: [
              { title: 'Establecimiento', titleKey: 'establecimiento.title', icon: '🏛️', route: '/configuracion/establecimiento' },
              { title: 'Localidad', titleKey: 'localidad.title', icon: '📍', route: '/configuracion/localidad' },
              { title: 'Moneda', titleKey: 'moneda.title', icon: '💱', route: '/nomencladores/moneda' },
              { title: 'País', titleKey: 'pais.title', icon: '🌍', route: '/nomencladores/pais' },
              { title: 'Provincia', titleKey: 'provincia.title', icon: '🗺️', route: '/nomencladores/provincia' },
              { title: 'Municipio', titleKey: 'municipio.title', icon: '🏘️', route: '/nomencladores/municipio' },
              { title: 'Código Postal', titleKey: 'codigoPostal.title', icon: '📮', route: '/nomencladores/codigo-postal' }
            ]
          },
          {
            title: 'Clasificador',
            titleKey: 'sidebar.clasificador',
            icon: '📋',
            expanded: false,
            children: [
              { title: 'Línea', titleKey: 'linea.title', icon: '📏', route: '/inventario/linea' },
              { title: 'Sublínea', titleKey: 'subLinea.title', icon: '📐', route: '/inventario/sublinea' },
              { title: 'Unidad Medida', titleKey: 'unidadMedida.title', icon: '⚖️', route: '/inventario/unidad-medida' }
            ]
          }
        ]
      },

      // Compras
      {
        title: 'Compras',
        titleKey: 'sidebar.compras',
        icon: '🛒',
        expanded: false,
        children: [
          { title: 'Informe de Recepción', titleKey: 'compras.informeRecepcion', icon: '📥', route: '/compras/informe-recepcion' },
          { title: 'Nota de Crédito', titleKey: 'compras.notaCreditoCompras', icon: '🧾', route: '/compras/nota-credito' },
          { title: 'Nota de Débito', titleKey: 'compras.notaDebitoCompras', icon: '🧾', route: '/compras/nota-debito' },
          { title: 'Devolución', titleKey: 'compras.devolucionCompra', icon: '↩️', route: '/compras/devolucion' },
          { title: 'Transferencias Entrada', titleKey: 'compras.transferenciasEntrada', icon: '🔁', route: '/compras/transferencias-entrada' },
          { title: 'Vales Entrada', titleKey: 'compras.valesEntrada', icon: '🎫', route: '/compras/vales-entrada' }
        ]
      },

      // Ventas
      {
        title: 'Ventas',
        titleKey: 'sidebar.ventas',
        icon: '🧾',
        expanded: false,
        children: [
          { title: 'Factura', titleKey: 'ventas.factura', icon: '🧾', route: '/ventas/factura' },
          { title: 'Nota de Crédito', titleKey: 'ventas.notaCreditoVentas', icon: '🧾', route: '/ventas/nota-credito' },
          { title: 'Nota de Débito', titleKey: 'ventas.notaDebitoVentas', icon: '🧾', route: '/ventas/nota-debito' },
          { title: 'Devolución', titleKey: 'ventas.devolucionVenta', icon: '↩️', route: '/ventas/devolucion' },
          { title: 'Caja', titleKey: 'ventas.cajaRegistradora', icon: '💳', route: '/ventas/caja' }
        ]
      },

      // Inventario
      {
        title: 'Inventario',
        titleKey: 'sidebar.inventario',
        icon: '📦',
        expanded: false,
        children: [
          { title: 'Productos', titleKey: 'producto.title', icon: '🏷️', route: '/inventario/productos' },
          { title: 'Movimientos', titleKey: 'sidebar.movimientos', icon: '🔄', route: '/movimientos' }
        ]
      },

      // Contabilidad
      {
        title: 'Contabilidad',
        titleKey: 'sidebar.contabilidad',
        icon: '💼',
        expanded: false,
        children: [
          { title: 'Generar Comprobante', titleKey: 'contabilidad.generarComprobante', icon: '🧾', route: '/contabilidad/generar-comprobante' },
          { title: 'Comprobante', titleKey: 'contabilidad.comprobante', icon: '📄', route: '/contabilidad/comprobante' },
          { title: 'Cuenta x Pagar', titleKey: 'contabilidad.cuentasPorPagar', icon: '📉', route: '/contabilidad/cuentas-por-pagar' },
          { title: 'Cuenta x Cobrar', titleKey: 'contabilidad.cuentasPorCobrar', icon: '📈', route: '/contabilidad/cuentas-por-cobrar' },
          { title: 'Balance General', titleKey: 'contabilidad.balanceGeneral', icon: '📊', route: '/contabilidad/balance-general' }
        ]
      },

      // Activos Fijos
      {
        title: 'Activos Fijos',
        titleKey: 'sidebar.activosFijos',
        icon: '🏛️',
        expanded: false,
        children: [
          { title: 'Catálogo de Activos', titleKey: 'activos.catalogo', icon: '📚', route: '/activos/catalogo' },
          { title: 'Mantenimientos', titleKey: 'activos.mantenimientos', icon: '🛠️', route: '/activos/mantenimientos' }
        ]
      },

      // Consultas / Reportes
      {
        title: 'Consultas',
        titleKey: 'sidebar.consultas',
        icon: '🔍',
        expanded: false,
        children: [
          { title: 'Consulta General', titleKey: 'consultas.consultaGeneral', icon: '📋', route: '/consultas/general' },
          { title: 'Histórico Producto', titleKey: 'consultas.historicoProducto', icon: '📜', route: '/consultas/historico-producto' }
        ]
      },

      // Nómina
      {
        title: 'Nómina',
        titleKey: 'sidebar.nomina',
        icon: '👥',
        expanded: false,
        children: [
          { title: 'Entrada de registros', titleKey: 'nomina.entradaRegistros', icon: '📝', route: '/nomina/entrada-registros' },
          { title: 'Cálculo de Nómina', titleKey: 'nomina.calculoNomina', icon: '🧮', route: '/nomina/calculo' }
        ]
      },

      // Configuración
      {
        title: 'Configuración',
        titleKey: 'sidebar.configuracion',
        icon: '⚙️',
        expanded: false,
        children: [
          { title: 'Negocio', titleKey: 'sidebar.negocio', icon: '🏢', route: '/configuracion/negocio' },
          { title: 'Usuarios', titleKey: 'sidebar.usuarios', icon: '👥', route: '/configuracion/usuarios' },
          { title: 'Prueba de Conexión', titleKey: 'sidebar.testConnection', icon: '🔌', route: '/configuracion/test-conexion' }
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

  onHeaderClick(item: MenuItem): void {
    if (item.children) {
      this.toggleItem(item);
      return;
    }
    if (item.route) {
      // navigate to route and close mobile overlay if open
      this.router.navigate([item.route]);
      this.closeMobile();
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
