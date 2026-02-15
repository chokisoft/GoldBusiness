import { Component, OnInit } from '@angular/core';

interface MenuItem {
  title: string;
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
export class SidebarComponent implements OnInit {
  menuItems: MenuItem[] = [];
  isCollapsed = false;

  ngOnInit(): void {
    this.menuItems = [
      {
        title: 'Nomencladores',
        icon: '🗂️',
        expanded: false,
        children: [
          {
            title: 'Plan de Cuentas',
            icon: '📊',
            expanded: false,
            children: [
              { title: 'Grupos de Cuenta', icon: '📁', route: '/nomencladores/grupo-cuenta' },
              { title: 'SubGrupos de Cuenta', icon: '📂', route: '/nomencladores/subgrupo-cuenta' },
              { title: 'Cuentas', icon: '📄', route: '/nomencladores/cuenta' }
            ]
          },
          {
            title: 'Gestión Financiera',
            icon: '💰',
            expanded: false,
            children: [
              { title: 'Monedas', icon: '💵', route: '/nomencladores/moneda' },
              { title: 'Conceptos de Ajuste', icon: '⚖️', route: '/nomencladores/concepto-ajuste' },
              { title: 'Transacciones', icon: '💳', route: '/nomencladores/transaccion' }
            ]
          },
          {
            title: 'Ubicaciones',
            icon: '📍',
            expanded: false,
            children: [
              { title: 'Establecimientos', icon: '🏢', route: '/nomencladores/establecimiento' },
              { title: 'Localidades', icon: '🌍', route: '/nomencladores/localidad' }
            ]
          },
          {
            title: 'Terceros',
            icon: '👥',
            expanded: false,
            children: [
              { title: 'Clientes', icon: '🛒', route: '/nomencladores/cliente' },
              { title: 'Proveedores', icon: '🚚', route: '/nomencladores/proveedor' }
            ]
          },
          {
            title: 'Productos',
            icon: '📦',
            expanded: false,
            children: [
              { title: 'Líneas', icon: '📋', route: '/nomencladores/linea' },
              { title: 'SubLíneas', icon: '📝', route: '/nomencladores/sublinea' },
              { title: 'Unidades de Medida', icon: '📏', route: '/nomencladores/unidad-medida' },
              { title: 'Productos', icon: '🏷️', route: '/nomencladores/producto' },
              { title: 'Fichas de Producto (BOM)', icon: '🔧', route: '/nomencladores/ficha-producto' }
            ]
          }
        ]
      },
      // ═══════════════════════════════════════════════════════════
      // ⚙️ CONFIGURACIÓN
      // ═══════════════════════════════════════════════════════════
      {
        title: 'Configuración',
        icon: '⚙️',
        expanded: false,
        children: [
          { title: 'Negocio', icon: '🏢', route: '/configuracion' },
          { title: 'Usuarios', icon: '👤', route: '/usuarios' }
        ]
      }
    ];
  }

  toggleItem(item: MenuItem): void {
    if (item.children) {
      item.expanded = !item.expanded;
    }
  }

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
  }
}
