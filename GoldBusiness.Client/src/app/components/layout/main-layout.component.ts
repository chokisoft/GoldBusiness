import { Component } from '@angular/core';

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
      margin-left: 280px;
      margin-top: 70px;
      padding: 2rem;
      min-height: calc(100vh - 70px);
      transition: margin-left 0.3s ease;
    }

    @media (max-width: 768px) {
      .main-content {
        margin-left: 0;
      }
    }
  `]
})
export class MainLayoutComponent {}
