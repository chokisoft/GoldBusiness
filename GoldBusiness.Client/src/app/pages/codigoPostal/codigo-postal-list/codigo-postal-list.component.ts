import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { CodigoPostalService, CodigoPostalDTO } from '../../../services/codigo-postal.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-codigo-postal-list',
  templateUrl: './codigo-postal-list.component.html',
  styleUrls: ['./codigo-postal-list.component.css']
})
export class CodigoPostalListComponent implements OnInit, OnDestroy {
  codigosPostales: CodigoPostalDTO[] = [];
  filteredCodigosPostales: CodigoPostalDTO[] = [];
  paginatedCodigosPostales: CodigoPostalDTO[] = [];
  loading = false;
  error: string | null = null;

  // Propiedades de búsqueda
  searchTerm: string = '';

  // Propiedades de paginación
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 0;

  // Valores calculados para UI
  startItem: number = 0;
  endItem: number = 0;

  // Exponer Math para usarlo en el template
  Math = Math;

  private languageSubscription?: Subscription;

  constructor(
    private codigopostalService: CodigoPostalService,
    private languageService: LanguageService
  ) { }

  ngOnInit(): void {
    this.loadData();

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 CodigoPostalList: Idioma cambiado, recargando datos...');
        this.loadData();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    // Deferir la suscripción para que Angular renderice `loading = true` primero
    Promise.resolve().then(() => {
      this.codigopostalService.getAll()
        .pipe(finalize(() => this.loading = false))
        .subscribe({
          next: (data) => {
            console.log('✅ GET /CodigoPostal response:', data);
            this.codigosPostales = data.sort((a, b) => a.codigo.localeCompare(b.codigo));
            this.filteredCodigosPostales = [...this.codigosPostales];

            if (!this.pageSize || this.pageSize <= 0) this.pageSize = 10;
            const totalItems = this.filteredCodigosPostales.length;
            this.totalPages = totalItems === 0 ? 0 : Math.ceil(totalItems / this.pageSize);

            if (this.totalPages === 0) {
              this.currentPage = 1;
            } else {
              this.currentPage = Math.min(Math.max(1, this.currentPage), this.totalPages);
            }

            this.applyPagination();
          },
          error: (err) => {
            console.error('❌ Error loading CodigoPostal list:', err);
            this.error = `Error al cargar los códigos postales: ${err?.message ?? err.statusText ?? err.status}`;
          }
        });
    });
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();

    if (!term) {
      this.filteredCodigosPostales = [...this.codigosPostales];
    } else {
      this.filteredCodigosPostales = this.codigosPostales.filter(item =>
        item.codigo.toLowerCase().includes(term) ||
        (item.municipioDescripcion || '').toLowerCase().includes(term) ||
        (item.provinciaDescripcion || '').toLowerCase().includes(term)
      );
    }

    // Volver a la primera página al filtrar — esto es intencional para UX consistente
    this.currentPage = 1;
    this.applyPagination();
  }

  applyPagination(): void {
    if (!this.pageSize || this.pageSize <= 0) {
      this.pageSize = 10;
    }

    const totalItems = this.filteredCodigosPostales.length;
    this.totalPages = totalItems === 0 ? 0 : Math.ceil(totalItems / this.pageSize);

    if (this.totalPages === 0) {
      this.currentPage = 1;
      this.paginatedCodigosPostales = [];
      this.startItem = 0;
      this.endItem = 0;
      return;
    }

    this.currentPage = Math.min(Math.max(1, this.currentPage), this.totalPages);

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = Math.min(startIndex + this.pageSize, totalItems);

    this.paginatedCodigosPostales = this.filteredCodigosPostales.slice(startIndex, endIndex);

    this.startItem = totalItems === 0 ? 0 : startIndex + 1;
    this.endItem = endIndex;
  }

  goToPage(page: number): void {
    if (page >= 1 && (this.totalPages === 0 || page <= this.totalPages)) {
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

  onPageSizeChange(newSize?: number | string): void {
    const parsed = Number(newSize);
    if (!isNaN(parsed) && parsed > 0) {
      this.pageSize = parsed;
    } else if (!this.pageSize || this.pageSize <= 0) {
      this.pageSize = 10;
    }

    // Reiniciar página y recalcular con el nuevo pageSize
    this.currentPage = 1;
    this.applyPagination();
  }

  delete(id: number, codigo: string): void {
    if (confirm(`¿Estás seguro de eliminar el código postal "${codigo}"?`)) {
      this.codigopostalService.delete(id).subscribe({
        next: () => {
          this.loadData();
        },
        error: (err) => {
          this.error = 'Error al eliminar el código postal';
          console.error('Error:', err);
        }
      });
    }
  }
}
