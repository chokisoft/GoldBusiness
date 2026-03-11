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
  loading = false;
  searching = false; // ✅ NUEVO: Estado separado para búsqueda
  error: string | null = null;

  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;

  Math = Math;

  private languageSubscription?: Subscription;
  private searchDebounceTimer?: any;

  constructor(
    private codigopostalService: CodigoPostalService,
    private languageService: LanguageService
  ) { }

  ngOnInit(): void {
    this.loadData(true); // Carga inicial

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 CodigoPostalList: Idioma cambiado, recargando datos...');
        this.loadData(true);
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    if (this.searchDebounceTimer) clearTimeout(this.searchDebounceTimer);
  }

  // ✅ MODIFICADO: Parámetro para distinguir carga inicial vs búsqueda
  loadData(isInitialLoad: boolean = false): void {
    if (isInitialLoad) {
      this.loading = true; // Solo mostrar loader en carga inicial
    } else {
      this.searching = true; // Indicador sutil para búsquedas
    }
    
    this.error = null;

    const searchTerm = this.searchTerm.trim() || undefined;
    this.codigopostalService.getPaged(this.currentPage, this.pageSize, searchTerm)
      .pipe(finalize(() => {
        this.loading = false;
        this.searching = false;
      }))
      .subscribe({
        next: (response) => {
          console.log('✅ GET /CodigoPostal/paged response:', response);
          this.codigosPostales = response.items;
          this.totalItems = response.total;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        },
        error: (err) => {
          console.error('❌ Error loading CodigoPostal list:', err);
          this.error = `Error al cargar los códigos postales: ${err?.message ?? err.statusText ?? err.status}`;
        }
      });
  }

  onSearch(): void {
    if (this.searchDebounceTimer) clearTimeout(this.searchDebounceTimer);
    
    this.searchDebounceTimer = setTimeout(() => {
      this.currentPage = 1;
      this.loadData(false); // ✅ Búsqueda (no muestra loader grande)
    }, 500); // ✅ Aumentado a 500ms para mejor UX
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadData(false);
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadData(false);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadData(false);
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
    this.loadData(false);
  }

  delete(id: number, codigo: string): void {
    if (confirm(`¿Estás seguro de eliminar el código postal "${codigo}"?`)) {
      this.codigopostalService.delete(id).subscribe({
        next: () => {
          if (this.codigosPostales.length === 1 && this.currentPage > 1) {
            this.currentPage--;
          }
          this.loadData(false);
        },
        error: (err) => {
          this.error = 'Error al eliminar el código postal';
          console.error('Error:', err);
        }
      });
    }
  }
}
