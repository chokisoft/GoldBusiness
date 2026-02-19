import { Component, OnInit } from '@angular/core';
import { SystemConfigurationService, SystemConfigurationDTO } from '../../../services/system-configuration.service';

@Component({
  selector: 'app-system-configuration-list',
  templateUrl: './system-configuration-list.component.html',
  styleUrls: ['./system-configuration-list.component.css']
})
export class SystemConfigurationListComponent implements OnInit {
  configurations: SystemConfigurationDTO[] = [];
  filteredConfigurations: SystemConfigurationDTO[] = [];
  paginatedConfigurations: SystemConfigurationDTO[] = [];
  loading = false;
  error: string | null = null;

  // Propiedades de búsqueda
  searchTerm: string = '';

  // Propiedades de paginación
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;

  // Exponer Math para usarlo en el template
  Math = Math;

  constructor(private systemConfigService: SystemConfigurationService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    this.systemConfigService.getAll().subscribe({
      next: (data) => {
        this.configurations = data.sort((a, b) => a.codigoSistema.localeCompare(b.codigoSistema));
        this.filteredConfigurations = [...this.configurations];
        this.applyPagination();
        this.loading = false;
      },
      error: (err: unknown) => {
        this.error = 'Error al cargar las configuraciones';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();
    
    if (!term) {
      this.filteredConfigurations = [...this.configurations];
    } else {
      this.filteredConfigurations = this.configurations.filter(config =>
        config.codigoSistema.toLowerCase().includes(term) ||
        config.nombreNegocio.toLowerCase().includes(term) ||
        config.licencia?.toLowerCase().includes(term)
      );
    }
    
    this.currentPage = 1;
    this.applyPagination();
  }

  applyPagination(): void {
    this.totalPages = Math.ceil(this.filteredConfigurations.length / this.pageSize);
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedConfigurations = this.filteredConfigurations.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.applyPagination();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.applyPagination();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.applyPagination();
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

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.applyPagination();
  }

  delete(id: number, nombreNegocio: string): void {
    if (confirm(`¿Está seguro de eliminar la configuración "${nombreNegocio}"?`)) {
      this.systemConfigService.delete(id).subscribe({
        next: () => {
          this.loadData();
        },
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

    if (diasRestantes < 0) {
      return 'vencida';
    } else if (diasRestantes <= 30) {
      return 'porVencer';
    } else {
      return 'vigente';
    }
  }
}
