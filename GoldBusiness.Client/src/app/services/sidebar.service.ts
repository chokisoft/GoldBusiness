import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private collapsedSubject = new BehaviorSubject<boolean>(false);
  public collapsed$: Observable<boolean> = this.collapsedSubject.asObservable();

  // Estado para overlay móvil
  private mobileOpenSubject = new BehaviorSubject<boolean>(false);
  public mobileOpen$: Observable<boolean> = this.mobileOpenSubject.asObservable();

  constructor() {
    const savedState = localStorage.getItem('sidebarCollapsed');
    if (savedState !== null) {
      this.collapsedSubject.next(savedState === 'true');
    }
    // Alinear clases del body al iniciar
    this.updateBodyClasses();
  }

  private updateBodyClasses(): void {
    if (typeof document === 'undefined' || !document.body) { return; }

    // Desktop collapsed/expanded classes
    if (this.collapsedSubject.value) {
      document.body.classList.add('sidebar-collapsed');
      document.body.classList.remove('sidebar-expanded');
    } else {
      document.body.classList.remove('sidebar-collapsed');
      document.body.classList.add('sidebar-expanded');
    }

    // Mobile overlay
    if (this.mobileOpenSubject.value) {
      document.body.classList.add('sidebar-mobile-open');
    } else {
      document.body.classList.remove('sidebar-mobile-open');
    }
  }

  /**
   * Alternar el estado del sidebar (colapsado/expandido)
   */
  toggle(): void {
    const newState = !this.collapsedSubject.value;
    this.collapsedSubject.next(newState);
    localStorage.setItem('sidebarCollapsed', String(newState));
    this.updateBodyClasses();
  }

  /**
   * Establecer el estado del sidebar manualmente
   */
  setCollapsed(collapsed: boolean): void {
    this.collapsedSubject.next(collapsed);
    localStorage.setItem('sidebarCollapsed', String(collapsed));
    this.updateBodyClasses();
  }

  /**
   * Obtener el estado actual del sidebar
   */
  isCollapsed(): boolean {
    return this.collapsedSubject.value;
  }

  // Mobile overlay control
  /**
   * Alternar el estado del overlay móvil (abierto/cerrado)
   */
  toggleMobile(): void {
    const newState = !this.mobileOpenSubject.value;
    this.mobileOpenSubject.next(newState);
    this.updateBodyClasses();
  }

  /**
   * Establecer el estado del overlay móvil manualmente
   */
  setMobileOpen(open: boolean): void {
    this.mobileOpenSubject.next(open);
    this.updateBodyClasses();
  }

  /**
   * Obtener el estado actual del overlay móvil
   */
  isMobileOpen(): boolean {
    return this.mobileOpenSubject.value;
  }
}
