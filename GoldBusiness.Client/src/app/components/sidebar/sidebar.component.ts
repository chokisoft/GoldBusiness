import { Component, OnInit } from '@angular/core';
import { TranslationService } from '../../services/translation.service';

interface MenuItem {
  title: string;
  titleKey?: string; // ← AGREGAR
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

  constructor(public translationService: TranslationService) { } // ← INYECTAR

  ngOnInit(): void {
    this.menuItems = [
      {
        title: 'Nomencladores',
        titleKey: 'sidebar.nomencladores', // ← AGREGAR
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
              { title: 'SubGrupos de Cuenta', titleKey: 'subgrupoCuenta.title', icon: '📂', route: '/nomencladores/subgrupo-cuenta' },
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
          { title: 'Usuarios', titleKey: 'sidebar.usuarios', icon: '👤', route: '/usuarios' }
        ]
      }
    ];
  }

  // ← AGREGAR MÉTODO
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
    this.isCollapsed = !this.isCollapsed;
  }
}
