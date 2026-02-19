import { Component, OnInit, OnDestroy, Renderer2 } from '@angular/core';
import { SidebarService } from '../../services/sidebar.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-main-layout',
  template: `
    <div class="main-layout">
      <app-navbar></app-navbar>
      <app-sidebar></app-sidebar>
      <main class="main-content">
        <router-outlet></router-outlet>
      </main>
    </div>
  `,
  styles: [`
    .main-layout {
      min-height: 100vh;
      background: #f5f6fa;
    }

    .main-content {
      margin-left: 280px; /* Sidebar expandido por defecto */
      margin-top: 70px;
      padding: 0; /* Sin padding interno, cada página controla su padding */
      min-height: calc(100vh - 70px);
      transition: margin-left 0.3s cubic-bezier(0.4, 0, 0.2, 1);
      width: calc(100% - 280px);
    }

    /* Responsive tablets */
    @media (max-width: 1024px) {
      .main-content {
        margin-left: 75px;
        width: calc(100% - 75px);
      }
    }

    /* Responsive móviles */
    @media (max-width: 768px) {
      .main-content {
        margin-left: 0;
        width: 100%;
      }
    }
  `]
})
export class MainLayoutComponent implements OnInit, OnDestroy {
  private sidebarSubscription?: Subscription;

  constructor(
    private sidebarService: SidebarService,
    private renderer: Renderer2
  ) { }

  ngOnInit(): void {
    console.log('🏗️ MainLayout inicializado');
    
    // Escuchar cambios del sidebar y agregar/quitar clase al body
    this.sidebarSubscription = this.sidebarService.collapsed$.subscribe(collapsed => {
      console.log('📐 Sidebar collapsed:', collapsed);
      
      if (collapsed) {
        this.renderer.addClass(document.body, 'sidebar-collapsed');
        this.renderer.removeClass(document.body, 'sidebar-expanded');
      } else {
        this.renderer.addClass(document.body, 'sidebar-expanded');
        this.renderer.removeClass(document.body, 'sidebar-collapsed');
      }
    });
  }

  ngOnDestroy(): void {
    console.log('🧹 MainLayout destruido');
    this.sidebarSubscription?.unsubscribe();
    // Limpiar clases del body
    this.renderer.removeClass(document.body, 'sidebar-collapsed');
    this.renderer.removeClass(document.body, 'sidebar-expanded');
  }
}
