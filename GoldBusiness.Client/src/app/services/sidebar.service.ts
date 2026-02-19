import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private collapsedSubject = new BehaviorSubject<boolean>(false);
  public collapsed$: Observable<boolean> = this.collapsedSubject.asObservable();

  constructor() {
    // Cargar estado guardado del localStorage (opcional)
    const savedState = localStorage.getItem('sidebarCollapsed');
    if (savedState !== null) {
      this.collapsedSubject.next(savedState === 'true');
    }
  }

  /**
   * Alternar el estado del sidebar (colapsado/expandido)
   */
  toggle(): void {
    const newState = !this.collapsedSubject.value;
    this.collapsedSubject.next(newState);
    // Guardar estado en localStorage
    localStorage.setItem('sidebarCollapsed', String(newState));
  }

  /**
   * Establecer el estado del sidebar manualmente
   */
  setCollapsed(collapsed: boolean): void {
    this.collapsedSubject.next(collapsed);
    localStorage.setItem('sidebarCollapsed', String(collapsed));
  }

  /**
   * Obtener el estado actual del sidebar
   */
  isCollapsed(): boolean {
    return this.collapsedSubject.value;
  }
}
