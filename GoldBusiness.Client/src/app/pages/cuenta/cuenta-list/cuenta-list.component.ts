import { Component, OnInit } from '@angular/core';
import { CuentaService, CuentaDTO } from '../../../services/cuenta.service';

@Component({
  selector: 'app-cuenta-list',
  templateUrl: './cuenta-list.component.html',
  styleUrls: ['./cuenta-list.component.css']
})
export class CuentaListComponent implements OnInit {
  cuentas: CuentaDTO[] = [];
  filteredCuentas: CuentaDTO[] = [];
  paginatedCuentas: CuentaDTO[] = [];
  loading = false;
  error: string | null = null;

  // Propiedades de búsqueda
  searchTerm: string = '';

  // Propiedades de paginación
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;

  // ✅ Exponer Math para usarlo en el template
  Math = Math;

  constructor(private cuentaService: CuentaService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    this.cuentaService.getAll().subscribe({
      next: (data) => {
        this.cuentas = data.sort((a, b) => a.codigo.localeCompare(b.codigo));
        this.filteredCuentas = [...this.cuentas];
        this.applyPagination();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar las cuentas';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();
    
    if (!term) {
      this.filteredCuentas = [...this.cuentas];
    } else {
      this.filteredCuentas = this.cuentas.filter(cuenta =>
        cuenta.codigo.toLowerCase().includes(term) ||
        cuenta.descripcion.toLowerCase().includes(term) ||
        cuenta.subGrupoCuentaCodigo?.toLowerCase().includes(term) ||
        cuenta.subGrupoCuentaDescripcion?.toLowerCase().includes(term)
      );
    }
    
    this.currentPage = 1; // Resetear a la primera página
    this.applyPagination();
  }

  applyPagination(): void {
    this.totalPages = Math.ceil(this.filteredCuentas.length / this.pageSize);
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedCuentas = this.filteredCuentas.slice(startIndex, endIndex);
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

  delete(id: number, descripcion: string): void {
    if (confirm(`¿Está seguro de eliminar la cuenta "${descripcion}"?`)) {
      this.cuentaService.delete(id).subscribe({
        next: () => {
          this.loadData();
        },
        error: (err) => {
          this.error = 'Error al eliminar la cuenta';
          console.error('Error:', err);
        }
      });
    }
  }
}
