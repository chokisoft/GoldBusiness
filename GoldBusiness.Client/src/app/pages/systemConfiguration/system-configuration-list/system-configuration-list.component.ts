import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-system-configuration-list',
  templateUrl: './system-configuration-list.component.html',
  styleUrls: ['./system-configuration-list.component.css']
})
export class SystemConfigurationListComponent implements OnInit, OnDestroy {
  configurations: SystemConfigurationDTO[] = [];
  // Compatibility with existing template (keeps same names used in HTML)
  filteredConfigurations: SystemConfigurationDTO[] = [];
  paginatedConfigurations: SystemConfigurationDTO[] = [];

  loading = false;
  error: string | null = null;

  // Propiedades de búsqueda
  searchTerm: string = '';

  // Propiedades de paginación (server-side)
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;

  // Valores calculados para UI
  startItem: number = 0;
  endItem: number = 0;

  // Exponer Math para usarlo en el template
  Math = Math;

  private languageSubscription?: Subscription;

  constructor(
    private systemConfigService: SystemConfigurationService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.loadData();

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        this.loadData();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadData(page?: number): void {
    this.loading = true;
    this.error = null;

    if (page) this.currentPage = page;

    this.systemConfigService.getPaged(this.currentPage, this.pageSize, this.searchTerm)
      .pipe(finalize(() => this.loading = false))
      .subscribe({
        next: resp => {
          // server returns page of items -> treat them as paginatedConfigurations
          this.configurations = resp.items;

          // Keep template-compatible collections in sync
          this.filteredConfigurations = [...resp.items];
          this.paginatedConfigurations = [...resp.items];

          this.totalItems = resp.total;
          this.startItem = this.totalItems === 0 ? 0 : (this.currentPage - 1) * this.pageSize + 1;
          this.endItem = Math.min(this.currentPage * this.pageSize, this.totalItems);
          this.totalPages = this.totalItems === 0 ? 0 : Math.ceil(this.totalItems / this.pageSize);
        },
        error: (err: unknown) => {
          this.error = 'Error al cargar las configuraciones';
          console.error('Error:', err);
        }
      });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadData();
  }

  goToPage(page: number): void {
    if (page >= 1 && (this.totalPages === 0 || page <= this.totalPages)) {
      this.currentPage = page;
      this.loadData(page);
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadData(this.currentPage);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadData(this.currentPage);
    }
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);

    if (endPage - startPage < maxVisiblePages - 1) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }

    for (let i = startPage; i <= endPage; i++) pages.push(i);

    return pages;
  }

  onPageSizeChange(newSize?: number | string): void {
    const parsed = Number(newSize);
    if (!isNaN(parsed) && parsed > 0) this.pageSize = parsed;
    else if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;

    this.currentPage = 1;
    this.loadData();
  }

  delete(id: number, nombreNegocio: string): void {
    if (confirm(`¿Está seguro de eliminar la configuración "${nombreNegocio}"?`)) {
      this.systemConfigService.delete(id).subscribe({
        next: () => this.loadData(),
        error: (err: unknown) => {
          this.error = 'Error al eliminar la configuración';
          console.error('Error:', err);
        }
      });
    }
  }

  getEstadoLicencia(caducidad: Date | string): string {
    const fechaCaducidad = new Date(caducidad);
    const hoy = new Date();
    const diasRestantes = Math.floor((fechaCaducidad.getTime() - hoy.getTime()) / (1000 * 60 * 60 * 24));

    if (diasRestantes < 0) return 'vencida';
    else if (diasRestantes <= 30) return 'porVencer';
    else return 'vigente';
  }
}
