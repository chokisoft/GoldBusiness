import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skip, finalize } from 'rxjs/operators';
import { ProvinciaService, ProvinciaDTO } from '../../../services/provincia.service';
import { LanguageService } from '../../../services/language.service';

@Component({
  selector: 'app-provincia-list',
  templateUrl: './provincia-list.component.html',
  styleUrls: ['./provincia-list.component.css']
})
export class ProvinciaListComponent implements OnInit, OnDestroy {
  provincias: ProvinciaDTO[] = [];
  loading = false;
  error: string | null = null;

  // Propiedades de búsqueda
  searchTerm: string = '';

  // Propiedades de paginación
  currentPage: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;
  totalPages: number = 0;

  // Exponer Math para usarlo en el template
  Math = Math;

  private languageSubscription?: Subscription;
  private searchDebounceTimer?: any;

  constructor(
    private provinciaService: ProvinciaService,
    private languageService: LanguageService
  ) { }

  ngOnInit(): void {
    this.loadData();

    this.languageSubscription = this.languageService.currentLanguage$
      .pipe(skip(1))
      .subscribe(() => {
        console.log('🔄 ProvinciaList: Idioma cambiado, recargando datos...');
        this.loadData();
      });
  }

  ngOnDestroy(): void {
    this.languageSubscription?.unsubscribe();
    if (this.searchDebounceTimer) clearTimeout(this.searchDebounceTimer);
  }

  loadData(): void {
    this.loading = true;
    this.error = null;

    const searchTerm = this.searchTerm.trim() || undefined;
    this.provinciaService.getPaged(this.currentPage, this.pageSize, searchTerm)
      .pipe(finalize(() => this.loading = false))
      .subscribe({
        next: (response) => {
          console.log('✅ GET /Provincia/paged response:', response);
          this.provincias = response.items;
          this.totalItems = response.total;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        },
        error: (err) => {
          console.error('❌ Error loading Provincia list:', err);
          this.error = `Error al cargar las provincias: ${err?.message ?? err.statusText ?? err.status}`;
        }
      });
  }

  onSearch(): void {
    if (this.searchDebounceTimer) clearTimeout(this.searchDebounceTimer);
    this.searchDebounceTimer = setTimeout(() => {
      this.currentPage = 1;
      this.loadData();
    }, 300);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadData();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadData();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadData();
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

  delete(id: number, descripcion: string): void {
    if (confirm(`¿Estás seguro de eliminar la provincia "${descripcion}"?`)) {
      this.provinciaService.delete(id).subscribe({
        next: () => {
          if (this.provincias.length === 1 && this.currentPage > 1) {
            this.currentPage--;
          }
          this.loadData();
        },
        error: (err) => {
          this.error = 'Error al eliminar la provincia';
          console.error('Error:', err);
        }
      });
    }
  }
}
